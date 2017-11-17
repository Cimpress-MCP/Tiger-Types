using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        static void ValueRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = Either.Left<string, int>(left.Get).Recover(recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        static void ValueRecover_Right(int right, int recoverer)
        {
            var actual = Either.Right<string, int>(right).Recover(recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        static void FuncRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = Either.Left<string, int>(left.Get).Recover(() => recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        static void FuncRecover_Right(int right, int recoverer)
        {
            var actual = Either.Right<string, int>(right).Recover(() => recoverer);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public static async Task TaskRecover_Left(NonEmptyString left, int recoverer)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Recover(() => FromResult(recoverer))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        public static async Task TaskRecover_Right(int right, int recoverer)
        {
            var actual = await Either.Right<string, int>(right)
                .Recover(() => FromResult(recoverer))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }
    }
}
