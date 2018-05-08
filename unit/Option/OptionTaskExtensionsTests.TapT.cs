using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to tapping from <see cref="OptionTaskExtensions"/>.</summary>
    public static partial class OptionTaskExtensionsTests
    {
        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs no action.")]
        public static async Task FuncTap_None(int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option<string>.None)
                .TapT(_ => output = sentinel)
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs an action.")]
        public static async Task FuncTap_Some(NonEmptyString some, int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option.From(some.Get))
                .TapT(_ => output = sentinel)
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option and performs no action.")]
        public static async Task TaskTap_None(int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option<string>.None)
                .TapTAsync(_ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option and performs an action.")]
        public static async Task TaskTap_Some(NonEmptyString some, int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option.From(some.Get))
                .TapTAsync(_ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }
    }
}
