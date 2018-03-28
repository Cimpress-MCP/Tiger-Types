using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to matching from <see cref="OptionTaskExtensions"/>.</summary>
    public static partial class OptionTaskExtensionsTests
    {
        [Property(DisplayName = "Matching a None Option returns the None value branch, not the Some func branch.")]
        public static async Task ValueFuncMatchTReturn_None(int noneValue, Func<string, int> someFunc)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: noneValue,
                some: someFunc)
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None value branch.")]
        public static async Task ValueFuncMatchTReturn_Some(NonEmptyString some, int noneValue, Func<string, int> someFunc)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: noneValue,
                some: someFunc)
                .ConfigureAwait(false);

            Assert.Equal(someFunc(some.Get), actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, not the Some func branch.")]
        public static async Task FuncFuncMatchTReturn_None(Func<int> noneFunc, Func<string, int> someFunc)
        {
            var actual = await FromResult(Option<string>.None).MatchT(
                none: noneFunc,
                some: someFunc)
                .ConfigureAwait(false);

            Assert.Equal(noneFunc(), actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None func branch.")]
        public static async Task FuncFuncMatchTReturn_Some(
            NonEmptyString some,
            Func<int> noneFunc,
            Func<string, int> someFunc)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchT(
                none: noneFunc,
                some: someFunc)
                .ConfigureAwait(false);

            Assert.Equal(someFunc(some.Get), actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, not the Some task branch.")]
        public static async Task FuncTaskMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchTAsync(
                none: () => none,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, not the None func branch.")]
        public static async Task FuncTaskMatchTReturn_Some(NonEmptyString some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchTAsync(
                none: () => none,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, not the Some func branch.")]
        public static async Task TaskFuncMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchTAsync(
                none: () => FromResult(none),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, not the None task branch.")]
        public static async Task TaskFuncMatchTReturn_Some(NonEmptyString some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchTAsync(
                none: () => FromResult(none),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, not the Some task branch.")]
        public static async Task TaskTaskMatchTReturn_None(int none)
        {
            var actual = await FromResult(Option<string>.None).MatchTAsync(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, not the None task branch.")]
        public static async Task TaskTaskMatchTReturn_Some(NonEmptyString some, int none)
        {
            var actual = await FromResult(Option.From(some.Get)).MatchTAsync(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }
    }
}
