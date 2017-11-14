using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to recovering <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        static void ValueRecover_None(NonNull<string> recovery)
        {
            var actual = Option<string>.None.Recover(recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        static void ValueRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            var actual = Option.From(some.Get).Recover(recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        static void FuncRecover_None(NonNull<string> recovery)
        {
            var actual = Option<string>.None.Recover(() => recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        static void FuncRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            var actual = Option.From(some.Get).Recover(() => recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        static async Task TaskRecover_None(NonNull<string> recovery)
        {
            var actual = await Option<string>.None
                .Recover(() => FromResult(recovery.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        static async Task TaskRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            var actual = await Option.From(some.Get)
                .Recover(() => FromResult(recovery.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }
    }
}
