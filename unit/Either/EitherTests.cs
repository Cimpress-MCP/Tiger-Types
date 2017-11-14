using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [Properties(Arbitrary = new [] { typeof(Generators) })]
    public static partial class EitherTests
    {
        [Property(DisplayName = "A Left Either is in the Left state.")]
        static void FromLeftBoth_IsLeft_Left(NonNull<string> left) =>
            Assert.True(Either<string, int>.FromLeft(left.Get).IsLeft);

        [Property(DisplayName = "A Left Either is in the Left state.")]
        static void FromLeftOne_IsLeft_Left(NonNull<string> left) =>
            Assert.True(Either.Left<string, int>(left.Get).IsLeft);

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        static void FromRightBoth_IsLeft_Right(int right) => Assert.False(Either<string, int>.FromRight(right).IsLeft);

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        static void FromRightOne_IsLeft_Right(int right) => Assert.False(Either.Right<string, int>(right).IsLeft);

        [Fact(DisplayName = "A Bottom Either is not in the Left state.")]
        static void Default_IsLeft_Bottom() => Assert.False(default(Either<string, int>).IsLeft);

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        static void FromLeftBoth_IsRight_Left(NonNull<string> left) =>
            Assert.False(Either<string, int>.FromLeft(left.Get).IsRight);

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        static void FromLeftOne_IsRight_Left(NonNull<string> left) =>
            Assert.False(Either.Left<string, int>(left.Get).IsRight);

        [Property(DisplayName = "A Right Either is in the Right state.")]
        static void FromRightBoth_IsRight_Right(int right) => Assert.True(Either<string, int>.FromRight(right).IsRight);

        [Property(DisplayName = "A Right Either is in the Right state.")]
        static void FromRightOne_IsRight_Right(int right) => Assert.True(Either.Right<string, int>(right).IsRight);

        [Fact(DisplayName = "A Bottom Either is not in the Right state.")]
        static void Default_IsRight_Bottom() => Assert.False(default(Either<string, int>).IsRight);

        [Property(DisplayName = "Forcibly unwrapping a Left Either throws.")]
        static void Value_Left_Throws(NonNull<string> left)
        {
            var actual = Record.Exception(() => Either.Left<string, int>(left.Get).Value);

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }

        [Property(DisplayName = "Forcibly unwrapping a Right Either returns the Right value.")]
        static void Value_Right(int right) => Assert.Equal(right, Either.Right<string, int>(right).Value);

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        static void GetValueOrDefault_Left(NonNull<string> left) =>
            Assert.Equal(default, Either.Left<string, int>(left.Get).GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        static void GetValueOrDefault_Right(int right) =>
            Assert.Equal(right, Either.Right<string, int>(right).GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        static void ValueGetValueOrDefault_Left(NonNull<string> left, int alternative) =>
            Assert.Equal(alternative, Either.Left<string, int>(left.Get).GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        static void ValueGetValueOrDefault_Right(int right, int alternative) =>
            Assert.Equal(right, Either.Right<string, int>(right).GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        static void FuncGetValueOrDefault_Left(NonNull<string> left, int alternative) =>
            Assert.Equal(alternative, Either.Left<string, int>(left.Get).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        static void FuncGetValueOrDefault_Right(int right, int alternative) =>
            Assert.Equal(right, Either.Right<string, int>(right).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        static async Task TaskGetValueOrDefault_Left(NonNull<string> left, int alternative)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .GetValueOrDefault(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(alternative, actual);
        }

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        static async Task TaskGetValueOrDefault_Right(int right, int alternative)
        {
            var actual = await Either.Right<string, int>(right)
                .GetValueOrDefault(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }
    }
}
