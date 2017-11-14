using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to mapping from <see cref="OptionTaskExtensions"/>.</context>
    public static partial class OptionTaskExtensionsTests
    {
        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        static async Task FuncMapT_None()
        {
            var actual = await FromResult(Option<string>.None)
                .MapT(v => v.Length)
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        static async Task FuncMapT_Some(NonNull<string> some)
        {
            var actual = await FromResult(Option.From(some.Get))
                .MapT(v => v.Length)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        static async Task TaskMapT_None()
        {
            var actual = await FromResult(Option<string>.None)
                .MapT(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        static async Task TaskMapT_Some(NonNull<string> some)
        {
            var actual = await FromResult(Option.From(some.Get))
                .MapT(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }
    }
}
