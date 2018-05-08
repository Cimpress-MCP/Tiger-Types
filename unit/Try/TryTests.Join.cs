using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Try{TErr, TOk}"/>.</summary>
    public static partial class TryTests
    {
        [Property(DisplayName = "A None Try joins to a None Try.")]
        public static void Join_OkSide_None() =>
            Assert.True(Try<string, Try<string, Version>>.None.Join().IsNone);

        [Property(DisplayName = "An Err Try joins to an Err Try.")]
        public static void Join_OkSide_Err(NonEmptyString err)
        {
            var actual = Try<string, Try<string, Version>>.From(err: err.Get).Join();

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try containing a None Try joins to a None Try.")]
        public static void Join_OkSide_OkNone() =>
            Assert.True(Try<string, Try<string, Version>>.From(ok: Try<string, Version>.None).Join().IsNone);

        [Property(DisplayName = "An Ok Try containing an Err Try joins to an Err Try.")]
        public static void Join_OkSide_OkErr(NonEmptyString err)
        {
            var actual = Try<string, Try<string, Version>>.From(ok: Try<string, Version>.From(err: err.Get)).Join();

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try containing an Err Try joins to an Err Try.")]
        public static void Join_OkSide_OkOk(Version ok)
        {
            var actual = Try<string, Try<string, Version>>.From(ok: Try<string, Version>.From(ok: ok)).Join();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "A None Try joins to a None Try.")]
        public static void Join_ErrSide_None() =>
            Assert.True(Try<Try<string, Version>, Version>.None.Join().IsNone);

        [Property(DisplayName = "An Ok Try joins to an Ok Try.")]
        public static void Join_ErrSide_Ok(Version ok)
        {
            var actual = Try<Try<string, Version>, Version>.From(ok: ok).Join();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "An Err Try containing a None Try joins to a None Try.")]
        public static void Join_ErrSide_ErrNone() =>
            Assert.True(Try<Try<string, Version>, Version>.From(err: Try<string, Version>.None).Join().IsNone);

        [Property(DisplayName = "An Err Try containing an Err Try joins to an Err Try.")]
        public static void Join_ErrSide_ErrErr(NonEmptyString err)
        {
            var actual = Try<Try<string, Version>, Version>.From(err: Try<string, Version>.From(err: err.Get)).Join();

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Err Try containing an Ok Try joins to an Ok Try.")]
        public static void Join_ErrSide_ErrOk(Version ok)
        {
            var actual = Try<Try<string, Version>, Version>.From(err: Try<string, Version>.From(ok: ok)).Join();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }
    }
}
