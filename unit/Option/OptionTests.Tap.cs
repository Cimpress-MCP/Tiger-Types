using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to tapping <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs no action.")]
        static void FuncTap_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Tap(v => output = sentinel.Get);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs an action.")]
        static void FuncTap_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            var output = before.Get;
            var actual = Option.From(some.Get).Tap(v => output = sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option and performs no action.")]
        static async Task TaskTap_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None
                .Tap(v => Run(() => output = sentinel.Get))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option and performs an action.")]
        static async Task TaskTap_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get)
                .Tap(v => Run(() => output = sentinel.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }
    }
}
