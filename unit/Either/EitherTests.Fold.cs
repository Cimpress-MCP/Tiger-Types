using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to matching <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        static void FuncFoldRight_Left(NonNull<string> left, int seed) =>
            Assert.Equal(seed, Either.Left<string, int>(left.Get).Fold(seed, right: (s, v) => s + v));

        [Property(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator over the seed value and the Right value.")]
        static void FuncFoldRight_Right(int right, int seed) =>
            Assert.Equal(seed + right, Either.Right<string, int>(right).Fold(seed, right: (s, v) => s + v));

        [Property(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        static void FuncFoldLeft_Right(int right, int seed) =>
            Assert.Equal(seed, Either.Right<string, int>(right).Fold(seed, left: (s, v) => s + v.Length));

        [Property(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
                                "over the seed value and the Left value.")]
        static void FuncFoldLeft_Left(NonNull<string> left, int seed) =>
            Assert.Equal(seed + left.Get.Length, Either.Left<string, int>(left.Get).Fold(seed, left: (s, v) => s + v.Length));

        [Property(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        static async Task TaskFoldRight_Left(NonNull<string> left, int seed)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Fold(seed, right: (s, v) => FromResult(s + v))
                .ConfigureAwait(false);

            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator" +
                                "over the seed value and the Right value.")]
        static async Task TaskFoldRight_Right(int right, int seed)
        {
            var actual = await Either.Right<string, int>(right)
                .Fold(seed, right: (s, v) => FromResult(s + v))
                .ConfigureAwait(false);

            Assert.Equal(seed + right, actual);
        }

        [Property(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        static async Task TaskFoldLeft_Right(int right, int seed)
        {
            var actual = await Either.Right<string, int>(right)
                .Fold(seed, left: (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
                                "over the seed value and the Left value.")]
        static async Task TaskFoldLeft_Left(NonNull<string> left, int seed)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Fold(seed, left: (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed + left.Get.Length, actual);
        }
    }
}