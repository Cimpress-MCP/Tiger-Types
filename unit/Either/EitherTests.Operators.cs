using System;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;
using static Tiger.Types.Resources;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Two Eithers of same type, in different state, with different value are not equal.")]
        public static void OperatorEquals_SameType_DifferentState_DifferentValue(Guid leftValue, Version rightValue)
        {
            var left = Either.From<Guid, Version>(leftValue);
            var right = Either.From<Guid, Version>(rightValue);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with different value are not equal.")]
        public static void OperatorEquals_SameType_SameState_DifferentValue(
            Guid leftValue,
            Guid rightValue)
        {
            var left = Either.From<Guid, Version>(leftValue);
            var right = Either.From<Guid, Version>(rightValue);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with same value are equal.")]
        public static void OperatorEquals_SameType_SameState_SameValue(Guid value)
        {
            var left = Either.From<Guid, Version>(value);
            var right = Either.From<Guid, Version>(value);

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public static void OperatorEquals_BottomBottom()
        {
            var left = default(Either<Guid, Version>);
            var right = default(Either<Guid, Version>);

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Property(DisplayName = "Two Eithers of same type, in different state, with different value are not equal.")]
        public static void OperatorNotEquals_SameType_DifferentState_DifferentValue(Guid leftValue, Version rightValue)
        {
            var left = Either.From<Guid, Version>(leftValue);
            var right = Either.From<Guid, Version>(rightValue);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with different value are not equal.")]
        public static void OperatorNotEquals_SameType_SameState_DifferentValue(UnequalPair<Guid> values)
        {
            var left = Either.From<Guid, Version>(values.Left);
            var right = Either.From<Guid, Version>(values.Right);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Eithers of same type, in same state, with same value are equal.")]
        public static void OperatorNotEquals_SameType_SameState_SameValue(Guid value)
        {
            var left = Either.From<Guid, Version>(value);
            var right = Either.From<Guid, Version>(value);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public static void OperatorNotEquals_BottomBottom()
        {
            var left = default(Either<Guid, Version>);
            var right = default(Either<Guid, Version>);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Property(DisplayName = "An EitherLeft converts implicitly to a Left Either.")]
        public static void EitherLeft_ToEither(NonEmptyString left)
        {
            Either<string, int> actual = Either.Left(left: left.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "An EitherRight converts implicitly to a Right Either.")]
        public static void EitherRight_ToEither(int right)
        {
            Either<string, int> actual = Either.Right(right: right);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "An EitherLeft and EitherRight behave together with type inference.")]
        public static void EitherSided_Combine(PositiveInt value, Guid left, Version right)
        {
            Either<Guid, Version> Func(int i)
            {
                if (i <= 0)
                {
                    return Either.Right(right: right);
                }
                return Either.Left(left: left);
            }

            var actual = Func(value.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(left, innerValue);
        }

        [Property(DisplayName = "A value of the Left type converts to a Left Either.")]
        public static void Left_IsLeft(NonEmptyString left)
        {
            Either<string, int> actual = left.Get;

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "A value of the Left type converts to a Left Either.")]
        public static void Named_Left_IsLeft(NonEmptyString left)
        {
            var actual = Either<string, int>.ToEither(left.Get);

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "A value of the Right type converts to a Right Either.")]
        public static void Right_IsRight(int right)
        {
            Either<string, int> actual = right;

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "A value of the Right type converts to a Right Either.")]
        public static void Named_Right_IsRight(int right)
        {
            var actual = Either<string, int>.ToEither(right);

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Fact(DisplayName = "Null converts to a Bottom Either.")]
        public static void Bottom_IsBottom()
        {
            Either<int, string> actual = null;

            Assert.False(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Unwrapping a Left Either throws.")]
        public static void Cast_Left_Throws(NonEmptyString left)
        {
            var actual = Record.Exception(() => (int)Either.From<string, int>(left.Get));

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(EitherIsNotRight, ex.Message, Ordinal);
        }

        [Property(DisplayName = "Unwrapping a Left Either returns the Left value.")]
        public static void Cast_Left(NonEmptyString left) =>
            Assert.Equal(left.Get, (string)Either.From<string, int>(left.Get));

        [Property(DisplayName = "Unwrapping a Right Either returns its Right value.")]
        public static void Cast_Right(int right) => Assert.Equal(right, (int)Either.From<string, int>(right));

        [Fact(DisplayName = "Unwrapping a Bottom Either throws.")]
        public static void Cast_Bottom_Throws()
        {
            var actual = Record.Exception(() => (int)default(Either<string, int>));

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(EitherIsNotRight, ex.Message, Ordinal);
        }
    }
}
