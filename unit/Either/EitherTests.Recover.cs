using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        static void ValueRecover_Left(NonNull<string> left, int recoverer)
        {
            var actual = Either.Left<string, int>(left.Get).Recover(recoverer);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        static void ValueRecover_Right(int right, int recoverer)
        {
            var actual = Either.Right<string, int>(right).Recover(recoverer);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        static void FuncRecover_Left(NonNull<string> left, int recoverer)
        {
            var actual = Either.Left<string, int>(left.Get).Recover(() => recoverer);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        static void FuncRecover_Right(int right, int recoverer)
        {
            var actual = Either.Right<string, int>(right).Recover(() => recoverer);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Recovering a Left Option returns the recovery value.")]
        static async Task TaskRecover_Left(NonNull<string> left, int recoverer)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Recover(() => FromResult(recoverer))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(recoverer, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Option returns the original value.")]
        static async Task TaskRecover_Right(int right, int recoverer)
        {
            var actual = await Either.Right<string, int>(right)
                .Recover(() => FromResult(recoverer))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }
    }
}
