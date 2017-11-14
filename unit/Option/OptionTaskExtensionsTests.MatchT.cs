using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to matching from <see cref="OptionTaskExtensions"/>.</context>
    public static partial class OptionTaskExtensionsTests
    {
        [Property(DisplayName = "Matching a None Option returns the None value branch, not the Some func branch.")]
        static async Task ValueFuncMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: none,
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None value branch.")]
        static async Task ValueFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: none,
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, not the Some func branch.")]
        static async Task FuncFuncMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: () => none,
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None func branch.")]
        static async Task FuncFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: () => none,
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, not the Some task branch.")]
        static async Task FuncTaskMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, not the None func branch.")]
        static async Task FuncTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, not the Some func branch.")]
        static async Task TaskFuncMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: () => FromResult(none),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None task branch.")]
        static async Task TaskFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: () => FromResult(none),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, not the Some task branch.")]
        static async Task TaskTaskMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, not the None task branch.")]
        static async Task TaskTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }
    }
}
