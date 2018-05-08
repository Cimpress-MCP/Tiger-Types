using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Conditionally executing an action based on a None Option with a null action throws.")]
        public static void ActionLet_None_Null_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Let(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing an action based on a None Option does not execute.")]
        public static void ActionLet_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            Option<string>.None.Let(_ => actual = sentinel.Get);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option with a null action throws.")]
        public static void ActionLet_Some_Null_Throws(NonEmptyString some)
        {
            var actual = Record.Exception(() => Option.From(some.Get).Let(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        public static void ActionLet_Some(NonEmptyString some, NonEmptyString before)
        {
            var actual = before.Get;
            Option.From(some.Get).Let(v => actual = v);

            Assert.Equal(some.Get, actual);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a None Option with a null asynchronous action throws.")]
        public static async Task TaskLet_None_Null_Throws()
        {
            var actual = await Record.ExceptionAsync(() => Option<string>.None.LetAsync(null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing a task based on a None Option does not execute.")]
        public static async Task TaskLet_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            await Option<string>.None
                .LetAsync(_ => Run(() => actual = sentinel.Get))
                .ConfigureAwait(false);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option with a null asynchronous action throws.")]
        public static async Task TaskLet_Some_Null_Throws(NonEmptyString some)
        {
            var actual = await Record.ExceptionAsync(() => Option.From(some.Get).LetAsync(null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing a task based on a Some Option executes.")]
        public static async Task TaskLet_Some(NonEmptyString some, NonEmptyString before)
        {
            var actual = before.Get;
            await Option.From(some.Get)
                .LetAsync(v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }
    }
}
