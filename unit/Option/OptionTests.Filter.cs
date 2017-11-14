using System.Threading.Tasks;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to filtering <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        static void FuncFilter_NoneFalse(bool filter)
        {
            var actual = Option<int>.None.Filter(_ => filter);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        static void FuncFilter_SomeFalse(int some)
        {
            var actual = Option.From(some).Filter(_ => false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        static void FuncFilter_SomeTrue(int some)
        {
            var actual = Option.From(some).Filter(_ => true);

            // assert
            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        static async Task TaskFilter_None(bool filter)
        {
            var actual = await Option<int>.None
                .Filter(v => FromResult(filter))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        static async Task TaskFilter_SomeFalse(int some)
        {
            var actual = await Option.From(some)
                .Filter(_ => FromResult(false))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a None Option.")]
        static async Task TaskFilter_SomeTrue(int some)
        {
            var actual = await Option.From(some)
                .Filter(v => FromResult(true))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }
    }
}
