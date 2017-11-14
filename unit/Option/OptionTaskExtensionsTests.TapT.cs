using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to tapping from <see cref="OptionTaskExtensions"/>.</context>
    public static partial class OptionTaskExtensionsTests
    {
        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs no action.")]
        static async Task FuncTap_None(int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option<string>.None)
                .TapT(_ => output = sentinel)
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs an action.")]
        static async Task FuncTap_Some(NonNull<string> some, int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option.From(some.Get))
                .TapT(_ => output = sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option and performs no action.")]
        static async Task TaskTap_None(int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option<string>.None)
                .TapT(_ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option and performs an action.")]
        static async Task TaskTap_Some(NonNull<string> some, int before, int sentinel)
        {
            var output = before;
            var actual = await FromResult(Option.From(some.Get))
                .TapT(_ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }
    }
}
