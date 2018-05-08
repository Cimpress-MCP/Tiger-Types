using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="TryCollectionExtensions"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class TryCollectionExtensionsTests
    {
        [Fact(DisplayName = "Err catting a null collection of tries throws.")]
        public static void CatErr_Null_Throws() =>
            Assert.NotNull(Record.Exception(() => TryCollectionExtensions.CatErr<string, Version>(null)));

        [Fact(DisplayName = "Ok catting a null collection of tries throws.")]
        public static void CatOk_Null_Throws() =>
            Assert.NotNull(Record.Exception(() => TryCollectionExtensions.CatOk<string, Version>(null)));

        [Property(DisplayName = "A collection of None tries is empty afer being Err catted.")]
        public static void CatErr_Empty(PositiveInt count) =>
            Assert.Empty(Enumerable.Repeat(Try<string, Version>.None, count.Get).CatErr());

        [Property(DisplayName = "A collection of None tries is empty afer being Ok catted.")]
        public static void CatOk_Empty(PositiveInt count) =>
            Assert.Empty(Enumerable.Repeat(Try<string, Version>.None, count.Get).CatOk());

        [Property(DisplayName = "A collection all Err tries becomes a collection of its Err values " +
                                "after being Err catted.")]
        [SuppressMessage("Style", "RCS1196", Justification = "Balance.")]
        public static void CatErr_AllErr(NonEmptyString[] errs)
        {
            var pairs = Enumerable.Zip(
                errs.Select(err => err.Get),
                errs.Select(err => err.Get).Select(Try<string, Version>.From).CatErr(),
                (l, r) => (l, r));
            Assert.All(pairs, p => Assert.Equal(p.l, p.r, Ordinal));
        }

        [Property(DisplayName = "A collection all Ok tries becomes a collection of its Ok values " +
                                "after being Err catted.")]
        [SuppressMessage("Style", "RCS1196", Justification = "Balance.")]
        public static void CatErr_AllOk(Version[] oks)
        {
            var pairs = Enumerable.Zip(
                oks,
                oks.Select(Try<string, Version>.From).CatOk(),
                (l, r) => (l, r));
            Assert.All(pairs, p => Assert.Equal(p.l, p.r));
        }

        [Property(DisplayName = "A collection of tries is never larger afer being Err catted.")]
        public static void CatErr(Try<string, Version>[] tries) =>
            Assert.True(tries.Length >= tries.CatErr().Count());

        [Property(DisplayName = "A collection of tries is never larger afer beingOk catted.")]
        public static void CatOk(Try<string, Version>[] tries) =>
            Assert.True(tries.Length >= tries.CatOk().Count());

        [Fact(DisplayName = "Partitioning a null collection of tries throws.")]
        public static void Partition_Null_Throws() =>
            Assert.NotNull(Record.Exception(() => TryCollectionExtensions.Partition<string, Version>(null)));

        [Fact(DisplayName = "Partitioning an empty collection produces empty partitions.")]
        public static void Partition_Empty_Empties()
        {
            var (errs, oks) = ImmutableArray<Try<string, Version>>.Empty.Partition();

            Assert.Empty(errs);
            Assert.Empty(oks);
        }

        [Property(DisplayName = "Partitioning a collection of Err tries produces an empty partition " +
                                "and a partition the size of the original collection.")]
        public static void Partition_AllErr_ErrsFullOksEmpty(NonEmptyString[] errValues)
        {
            var (errs, oks) = ImmutableArray.CreateRange(
                errValues.Select(err => err.Get).Select(Try<string, Version>.From)).Partition();

            Assert.Equal(errValues.Length, errs.Length);
            Assert.Empty(oks);
        }

        [Property(DisplayName = "Partitioning a collection of Ok tries produces an empty partition " +
                                "and a partition the size of the original collection.")]
        public static void Partition_AllOk_ErrsEmptyOksFull(Version[] okValues)
        {
            var (errs, oks) = ImmutableArray.CreateRange(okValues.Select(Try<string, Version>.From)).Partition();

            Assert.Empty(errs);
            Assert.Equal(okValues.Length, oks.Length);
        }
    }
}
