using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to matching <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
                                "not the Some func branch.")]
        static void ValueFuncMatchReturn_None(int noneValue)
        {
            var actual = Option<string>.None.Match(
                none: noneValue,
                some: v => v.Length);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None value branch.")]
        static void ValueFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = Option.From(some.Get).Match(
                none: noneValue,
                some: v => v.Length);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
                                "not the Some task branch.")]
        static async Task ValueTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.Match(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None value branch.")]
        static async Task ValueTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = await Option.From(some.Get).Match(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
                                "not the Some func branch.")]
        static void FuncFuncMatchReturn_None(int noneValue)
        {
            var actual = Option<string>.None.Match(
                none: () => noneValue,
                some: v => v.Length);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None func branch.")]
        static void FuncFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = Option.From(some.Get).Match(
                none: () => noneValue,
                some: v => v.Length);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
                                "not the Some task branch.")]
        static async Task FuncTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.Match(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None func branch.")]
        static async Task FuncTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = await Option.From(some.Get).Match(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
                                "not the Some func branch.")]
        static async Task TaskFuncMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None task branch.")]
        static async Task TaskFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = await Option.From(some.Get).Match(
                none: () => FromResult(noneValue),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
                                "not the Some task branch.")]
        static async Task TaskTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None task branch.")]
        static async Task TaskTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            var actual = await Option.From(some.Get).Match(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
                                "not the Some action branch.")]
        static void ActionActionMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            var actual = before.Get;
            var unit = Option<string>.None.Match(
                none: () => actual = sentinel.Get,
                some: v => { });

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, not the None action branch.")]
        static void ActionActionMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            var actual = before.Get;
            var unit = Option.From(some.Get).Match(
                none: () => { },
                some: v => actual = v);

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
                                "not the Some task branch.")]
        static async Task ActionTaskMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            var actual = before.Get;
            await Option<string>.None.Match(
                none: () => actual = sentinel.Get,
                some: v => CompletedTask)
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
                                "not the None action branch.")]
        static async Task ActionTaskMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            var actual = before.Get;
            await Option.From(some.Get).Match(
                none: () => { },
                some: v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
                                "not the Some action branch.")]
        static async Task TaskActionMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            var actual = before.Get;
            await Option<string>.None.Match(
                none: () => Run(() => actual = sentinel.Get),
                some: v => { })
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, " +
                                "not the None task branch.")]
        static async Task TaskActionMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            var actual = before.Get;
            await Option.From(some.Get).Match(
                none: () => CompletedTask,
                some: v => actual = v)
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
                                "not the Some task branch.")]
        static async Task TaskTaskMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            var actual = before.Get;
            await Option<string>.None.Match(
                none: () => Run(() => actual = sentinel.Get),
                some: v => CompletedTask)
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
                                "not the None task branch.")]
        static async Task TaskTaskMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            var actual = before.Get;
            await Option.From(some.Get).Match(
                none: () => CompletedTask,
                some: v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }
    }
}
