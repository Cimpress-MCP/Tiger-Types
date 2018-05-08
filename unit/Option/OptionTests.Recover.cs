using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to recovering <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Recovering a None Option with null throws.")]
        public static void ValueRecover_None_Null_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Recover(recoverer: (string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void ValueRecover_None(NonEmptyString recovery)
        {
            var actual = Option<string>.None.Recover(recovery.Get);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option with null throws.")]
        public static void ValueRecover_Some_Null_Throws(NonEmptyString some)
        {
            var actual = Record.Exception(() => Option.From(some.Get).Recover(recoverer: (string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static void ValueRecover_Some(NonEmptyString some, NonEmptyString recovery)
        {
            var actual = Option.From(some.Get).Recover(recovery.Get);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a None Option with null throws.")]
        public static void FuncRecover_None_Null_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Recover(recoverer: (Func<string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void FuncRecover_None(NonEmptyString recovery)
        {
            var actual = Option<string>.None.Recover(() => recovery.Get);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option with null throws.")]
        public static void FuncRecover_Some_Null_Throws(NonEmptyString some)
        {
            var actual = Record.Exception(() => Option.From(some.Get).Recover(recoverer: (Func<string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static void FuncRecover_Some(NonEmptyString some, NonEmptyString recovery)
        {
            var actual = Option.From(some.Get).Recover(() => recovery.Get);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a None Option with null throws.")]
        public static async Task TaskRecover_None_Null_Throws()
        {
            var actual = await Record.ExceptionAsync(() => Option<string>.None
                .RecoverAsync(recoverer: null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static async Task TaskRecover_None(NonEmptyString recovery)
        {
            var actual = await Option<string>.None
                .RecoverAsync(() => FromResult(recovery.Get))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option with null throws.")]
        public static async Task TaskRecover_Some_Null_Throws(NonEmptyString some)
        {
            var actual = await Record.ExceptionAsync(() => Option.From(some.Get)
                .RecoverAsync(recoverer: null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static async Task TaskRecover_Some(NonEmptyString some, NonEmptyString recovery)
        {
            var actual = await Option.From(some.Get)
                .RecoverAsync(() => FromResult(recovery.Get))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }
    }
}
