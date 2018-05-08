using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to tapping <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Tapping an option with a null action throws.")]
        public static void FuncTap_OneWayNone_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Tap(none: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs an action.")]
        public static void FuncTap_OneWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Tap(() => output = sentinel.Get);

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs no action.")]
        public static void FuncTap_OneWayNone_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option.From(some.Get).Tap(() => output = sentinel.Get);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null action throws.")]
        public static void FuncTap_OneWaySome_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Tap(some: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs no action.")]
        public static void FuncTap_OneWaySome_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Tap(_ => output = sentinel.Get);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs an action.")]
        public static void FuncTap_OneWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option.From(some.Get).Tap(_ => output = sentinel.Get);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null task throws.")]
        public static async Task TaskTap_OneWayNone_Null_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(none: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option and performs an action.")]
        public static async Task TaskTap_OneWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None.TapAsync(() => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option and performs no action.")]
        public static async Task TaskTap_OneWayNone_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get).TapAsync(() => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null task throws.")]
        public static async Task TaskTap_OneWaySome_Null_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(some: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option and performs no action.")]
        public static async Task TaskTap_OneWaySome_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None.TapAsync(_ => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option and performs an action.")]
        public static async Task TaskTap_OneWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get).TapAsync(_ => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null None Action throws.")]
        public static void FuncFuncTap_TwoWay_NoneNull_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Tap(
                none: null,
                some: _ => { }));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping an option with a null Some Action throws.")]
        public static void FuncFuncTap_TwoWay_SomeNull_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Tap(
                none: () => { },
                some: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a func and a func returns a None Option and performs " +
                                "the None action, not the Some action.")]
        public static void FuncFuncTap_TwoWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Tap(
                none: () => output = sentinel.Get,
                some: _ => { });

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func and a func returns a Some Option and performs " +
                                "the Some action, not the None action.")]
        public static void FuncFuncTap_TwoWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option.From(some.Get).Tap(
                none: () => { },
                some: _ => output = sentinel.Get);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null None task throws.")]
        public static async Task TaskFuncTap_TwoWay_NoneNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: null,
                some: _ => { })).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping an option with a null Some task throws.")]
        public static async Task TaskFuncTap_TwoWay_SomeNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: () => CompletedTask,
                some: (Action<string>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a task and a func returns a None Option and performs " +
                                "the None task, not the Some action.")]
        public static async Task TaskFuncTap_TwoWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None.TapAsync(
                none: () => Run(() => output = sentinel.Get),
                some: _ => { }).ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task and a func returns a None Option and performs " +
                                "the Some action, not the None task.")]
        public static async Task TaskFuncMap_TwoWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get).TapAsync(
                none: () => CompletedTask,
                some: _ => output = sentinel.Get).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null None func throws.")]
        public static async Task FuncTaskTap_TwoWay_NoneNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: (Action)null,
                some: _ => CompletedTask)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping an option with a null Some task throws.")]
        public static async Task FuncTaskTap_TwoWay_SomeNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: () => { },
                some: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a func and a task returns a None Option and performs " +
                                "the None action, not the Some task.")]
        public static async Task FuncTaskTap_TwoWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None.TapAsync(
                none: () => output = sentinel.Get,
                some: _ => CompletedTask).ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func and a task returns a None Option and performs " +
                                "the Some task, not the None func.")]
        public static async Task FuncTaskMap_TwoWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get).TapAsync(
                none: () => { },
                some: _ => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping an option with a null None task throws.")]
        public static async Task TaskTaskTap_TwoWay_NoneNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: null,
                some: _ => CompletedTask)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping an option with a null Some task throws.")]
        public static async Task TaskTaskTap_TwoWay_SomeNull_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.TapAsync(
                none: () => CompletedTask,
                some: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a task and a task returns a None Option and performs " +
                                "the None task, not the Some task.")]
        public static async Task TaskTaskTap_TwoWayNone_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option<string>.None.TapAsync(
                none: () => Run(() => output = sentinel.Get),
                some: _ => CompletedTask).ConfigureAwait(false);

            Assert.True(actual.IsNone);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task and a task returns a None Option and performs " +
                                "the Some task, not the None task.")]
        public static async Task TaskTaskMap_TwoWaySome_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = await Option.From(some.Get).TapAsync(
                none: () => CompletedTask,
                some: _ => Run(() => output = sentinel.Get)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }
    }
}
