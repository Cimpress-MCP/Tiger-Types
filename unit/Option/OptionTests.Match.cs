using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to matching <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Matching an Option with a null None value throws.")]
        public static void ValueFuncMatchReturn_NullNone_Throw(Option<int> option, Func<int, string> some)
        {
            var actual = Record.Exception(() => option.Match(
                none: (string)null,
                some: some));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null some func throws.")]
        public static void ValueFuncMatchReturn_NullSome_Throw(Option<int> option, NonEmptyString none)
        {
            var actual = Record.Exception(() => option.Match(
                none: none.Get,
                some: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
                                "not the Some func branch.")]
        public static void ValueFuncMatchReturn_None(int noneValue)
        {
            var actual = Option<string>.None.Match(
                none: noneValue,
                some: v => v.Length);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None value branch.")]
        public static void ValueFuncMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = Option.From(some.Get).Match(
                none: noneValue,
                some: v => v.Length);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None value throws.")]
        public static async Task ValueTaskMatchReturn_NullNone_Throw(Option<int> option, Func<int, Task<string>> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: (string)null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some task throws.")]
        public static async Task ValueTaskMatchReturn_NullSome_Throw(Option<int> option, NonEmptyString none)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: none.Get,
                some: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
                                "not the Some task branch.")]
        public static async Task ValueTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.MatchAsync(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None value branch.")]
        public static async Task ValueTaskMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = await Option.From(some.Get).MatchAsync(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None func throws.")]
        public static void FuncFuncMatchReturn_NullNone_Throw(Option<int> option, Func<int, string> some)
        {
            var actual = Record.Exception(() => option.Match(
                none: (Func<string>)null,
                some: some));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some func throws.")]
        public static void FuncFuncMatchReturn_NullSome_Throw(Option<int> option, Func<string> none)
        {
            var actual = Record.Exception(() => option.Match(
                none: none,
                some: (Func<int, string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
                                "not the Some func branch.")]
        public static void FuncFuncMatchReturn_None(int noneValue)
        {
            var actual = Option<string>.None.Match(
                none: () => noneValue,
                some: v => v.Length);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None func branch.")]
        public static void FuncFuncMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = Option.From(some.Get).Match(
                none: () => noneValue,
                some: v => v.Length);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None func throws.")]
        public static async Task FuncTaskMatchReturn_NullNone_Throw(Option<int> option, Func<int, Task<string>> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: (Func<string>)null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some task throws.")]
        public static async Task FuncTaskMatchReturn_NullSome_Throw(Option<int> option, Func<string> none)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: none,
                some: (Func<int, Task<string>>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
                                "not the Some task branch.")]
        public static async Task FuncTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.MatchAsync(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None func branch.")]
        public static async Task FuncTaskMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = await Option.From(some.Get).MatchAsync(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None task throws.")]
        public static async Task TaskFuncMatchReturn_NullNone_Throw(Option<int> option, Func<int, string> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some func throws.")]
        public static async Task TaskFuncMatchReturn_NullSome_Throw(Option<int> option, Func<Task<string>> none)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: none,
                some: (Func<int, string>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
                                "not the Some func branch.")]
        public static async Task TaskFuncMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.MatchAsync(
                none: () => FromResult(noneValue),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
                                "not the None task branch.")]
        public static async Task TaskFuncMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = await Option.From(some.Get).MatchAsync(
                none: () => FromResult(noneValue),
                some: v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None task throws.")]
        public static async Task TaskTaskMatchReturn_NullNone_Throw(Option<int> option, Func<int, Task<string>> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: (Func<Task<string>>)null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some task throws.")]
        public static async Task TaskTaskMatchReturn_NullSome_Throw(Option<int> option, Func<Task<string>> none)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: none,
                some: (Func<int, Task<string>>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
                                "not the Some task branch.")]
        public static async Task TaskTaskMatchReturn_None(int noneValue)
        {
            var actual = await Option<string>.None.MatchAsync(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
                                "not the None task branch.")]
        public static async Task TaskTaskMatchReturn_Some(NonEmptyString some, int noneValue)
        {
            var actual = await Option.From(some.Get).MatchAsync(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None action throws.")]
        public static void ActionActionMatchVoid_NullNone_Throw(Option<int> option, Action<int> some)
        {
            var actual = Record.Exception(() => option.Match(
                none: null,
                some: some));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some func throws.")]
        public static void ActionActionMatchVoid_NullSome_Throw(Option<int> option)
        {
            var actual = Record.Exception(() => option.Match(
                none: () => { },
                some: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
                                "not the Some action branch.")]
        public static void ActionActionMatchVoid_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            var unit = Option<string>.None.Match(
                none: () => actual = sentinel.Get,
                some: _ => { });

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, not the None action branch.")]
        public static void ActionActionMatchVoid_Some(NonEmptyString before, NonEmptyString some)
        {
            var actual = before.Get;
            var unit = Option.From(some.Get).Match(
                none: () => { },
                some: v => actual = v);

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None action throws.")]
        public static async Task ActionTaskMatchVoid_NullNone_Throw(Option<int> option, Func<int, Task> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: (Action)null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some task throws.")]
        public static async Task ActionTaskMatchVoid_NullSome_Throw(Option<int> option)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: () => { },
                some: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
                                "not the Some task branch.")]
        public static async Task ActionTaskMatchVoid_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            await Option<string>.None.MatchAsync(
                none: () => actual = sentinel.Get,
                some: _ => CompletedTask)
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
                                "not the None action branch.")]
        public static async Task ActionTaskMatchVoid_Some(NonEmptyString before, NonEmptyString some)
        {
            var actual = before.Get;
            await Option.From(some.Get).MatchAsync(
                none: () => { },
                some: v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None task throws.")]
        public static async Task TaskActionMatchVoid_NullNone_Throw(Option<int> option, Action<int> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some action throws.")]
        public static async Task TaskActionMatchVoid_NullSome_Throw(Option<int> option)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: () => CompletedTask,
                some: (Action<int>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
                                "not the Some action branch.")]
        public static async Task TaskActionMatchVoid_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            await Option<string>.None.MatchAsync(
                none: () => Run(() => actual = sentinel.Get),
                some: _ => { })
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, " +
                                "not the None task branch.")]
        public static async Task TaskActionMatchVoid_Some(NonEmptyString before, NonEmptyString some)
        {
            var actual = before.Get;
            await Option.From(some.Get).MatchAsync(
                none: () => CompletedTask,
                some: v => actual = v)
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching an Option with a null None task throws.")]
        public static async Task TaskTaskMatchVoid_NullNone_Throw(Option<int> option, Func<int, Task> some)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: (Func<Task>)null,
                some: some)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching an Option with a null Some task throws.")]
        public static async Task TaskTaskMatchVoid_NullSome_Throw(Option<int> option)
        {
            var actual = await Record.ExceptionAsync(() => option.MatchAsync(
                none: () => CompletedTask,
                some: (Func<int, Task>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
                                "not the Some task branch.")]
        public static async Task TaskTaskMatchVoid_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            await Option<string>.None.MatchAsync(
                none: () => Run(() => actual = sentinel.Get),
                some: _ => CompletedTask)
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
                                "not the None task branch.")]
        public static async Task TaskTaskMatchVoid_Some(NonEmptyString before, NonEmptyString some)
        {
            var actual = before.Get;
            await Option.From(some.Get).MatchAsync(
                none: () => CompletedTask,
                some: v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }
    }
}
