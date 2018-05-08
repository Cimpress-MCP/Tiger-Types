using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Attemping to recover with a null value throws.")]
        public static void ValueRecover_Null(Either<string, Version> either)
        {
            var actual = Record.Exception(() => either.Recover((Version)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public static void ValueRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = Either.From<string, int>(left.Get).Recover(recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        public static void ValueRecover_Right(int right, int recoverer)
        {
            var actual = Either.From<string, int>(right).Recover(recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Attemping to recover with a null func throws.")]
        public static void FuncRecover_Null(Either<string, Version> either)
        {
            var actual = Record.Exception(() => either.Recover((Func<Version>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public static void FuncRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = Either.From<string, int>(left.Get).Recover(() => recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        public static void FuncRecover_Right(int right, int recoverer)
        {
            var actual = Either.From<string, int>(right).Recover(() => recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Attemping to recover with a null task throws.")]
        public static async Task TaskRecover_Null(Either<string, Version> either)
        {
            var actual = await Record.ExceptionAsync(() => either.RecoverAsync(null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("recoverer", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public static async Task TaskRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = await Either.From<string, int>(left.Get)
                .RecoverAsync(() => FromResult(recoverer))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        public static async Task TaskRecover_Right(int right, int recoverer)
        {
            var actual = await Either.From<string, int>(right)
                .RecoverAsync(() => FromResult(recoverer))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }
    }
}
