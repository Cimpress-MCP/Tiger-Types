using System;
using System.Threading.Tasks;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to filtering <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Filtering an option over a null func throws.")]
        public static void FuncFilter_Null_Throws(Option<int> option)
        {
            var actual = Record.Exception(() => option.Filter(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static void FuncFilter_NoneFalse(bool filter)
        {
            var actual = Option<int>.None.Filter(_ => filter);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static void FuncFilter_SomeFalse(int some)
        {
            var actual = Option.From(some).Filter(_ => false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        public static void FuncFilter_SomeTrue(int some)
        {
            var actual = Option.From(some).Filter(_ => true);

            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }

        [Property(DisplayName = "Filtering an option over a null task throws.")]
        public static async Task TaskFilter_Null_Throws(Option<int> option)
        {
            var actual = await Record.ExceptionAsync(() => option.FilterAsync(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static async Task TaskFilter_None(bool filter)
        {
            var actual = await Option<int>.None
                .FilterAsync(_ => FromResult(filter))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static async Task TaskFilter_SomeFalse(int some)
        {
            var actual = await Option.From(some)
                .FilterAsync(_ => FromResult(false))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a None Option.")]
        public static async Task TaskFilter_SomeTrue(int some)
        {
            var actual = await Option.From(some)
                .FilterAsync(_ => FromResult(true))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }
    }
}
