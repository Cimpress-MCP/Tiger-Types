using System;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Two Eithers of same type, in different state, with different value are not equal.")]
        static void OperatorEquals_SameType_DifferentState_DifferentValue(Guid leftValue, Version rightValue)
        {
            var left = Either.Left<Guid, Version>(leftValue);
            var right = Either.Right<Guid, Version>(rightValue);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with different value are not equal.")]
        static void OperatorEquals_SameType_SameState_DifferentValue(
            Guid leftValue,
            Guid rightValue)
        {
            var left = Either.Left<Guid, Version>(leftValue);
            var right = Either.Left<Guid, Version>(rightValue);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with same value are equal.")]
        static void OperatorEquals_SameType_SameState_SameValue(Guid value)
        {
            var left = Either.Left<Guid, Version>(value);
            var right = Either.Left<Guid, Version>(value);

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        static void OperatorEquals_BottomBottom()
        {
            var left = default(Either<Guid, Version>);
            var right = default(Either<Guid, Version>);

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in different state, with different value are not equal.")]
        static void OperatorNotEquals_SameType_DifferentState_DifferentValue(Guid leftValue, Version rightValue)
        {
            var left = Either.Left<Guid, Version>(leftValue);
            var right = Either.Right<Guid, Version>(rightValue);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with different value are not equal.")]
        static void OperatorNotEquals_SameType_SameState_DifferentValue(UnequalPair<Guid> values)
        {
            var left = Either.Left<Guid, Version>(values.Left);
            var right = Either.Left<Guid, Version>(values.Right);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with same value are equal.")]
        static void OperatorNotEquals_SameType_SameState_SameValue(Guid value)
        {
            var left = Either.Left<Guid, Version>(value);
            var right = Either.Left<Guid, Version>(value);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        static void OperatorNotEquals_BottomBottom()
        {
            var left = default(Either<Guid, Version>);
            var right = default(Either<Guid, Version>);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Property(DisplayName = "An EitherLeft converts implicitly to a Left Either.")]
        static void EitherLeft_ToEither(NonNull<string> left)
        {
            Either<string, int> actual = Either.Left(left.Get);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "An EitherRight converts implicitly to a Right Either.")]
        static void EitherRight_ToEither(int right)
        {
            Either<string, int> actual = Either.Right(right);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "An EitherLeft and EitherRight behave together with type inference.")]
        static void EitherSided_Combine(PositiveInt value, Guid left, Version right)
        {
            Either<Guid, Version> Func(int i)
            {
                if (i <= 0)
                {
                    return Either.Right(right);
                }
                return Either.Left(left);
            }

            var actual = Func(value.Get);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(left, innerValue);
        }

        [Property(DisplayName = "A value of the Left type converts to a Left Either.")]
        static void Left_IsLeft(NonNull<string> left)
        {
            Either<string, int> actual = left.Get;

            // assert
            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "A value of the Right type converts to a Right Either.")]
        static void Right_IsRight(int right)
        {
            Either<string, int> actual = right;

            // assert
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Fact(DisplayName = "Null converts to a Bottom Either.")]
        static void Bottom_IsBottom()
        {
            Either<int, string> actual = null;

            // assert
            Assert.False(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Unwrapping a Left Either throws.")]
        static void Cast_Left_Throws(NonNull<string> left)
        {
            var actual = Record.Exception(() => (int)Either.Left<string, int>(left.Get));

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }

        [Property(DisplayName = "Unwrapping a Left Either returns the Left value.")]
        static void Cast_Left(NonNull<string> left) =>
            Assert.Equal(left.Get, (string)Either.Left<string, int>(left.Get));

        [Property(DisplayName = "Unwrapping a Right Either returns its Right value.")]
        static void Cast_Right(int right) => Assert.Equal(right, (int)Either.Right<string, int>(right));

        [Fact(DisplayName = "Unwrapping a Bottom Either throws.")]
        static void Cast_Bottom_Throws()
        {
            var actual = Record.Exception(() => (int)default(Either<string, int>));

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }
    }
}
