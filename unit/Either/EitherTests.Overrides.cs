using System;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "A Left Either stringifies to Left.")]
        public static void ToString_Left(Guid left) =>
            Assert.Equal($"Left({left})", Either.Left<Guid, Version>(left).ToString());

        [Property(DisplayName = "A Right Either stringifies to Right.")]
        public static void ToString_Right(Version right) =>
            Assert.Equal($"Right({right})", Either.Right<Guid, Version>(right).ToString());

        [Property(DisplayName = "A Bottom Either stringifies to Bottom.")]
        public static void ToString_Bottom() => Assert.Equal("Bottom", default(Either<Guid, Version>).ToString());

        [Property(DisplayName = "A Left Either is not equal to null.")]
        public static void ObjectEquals_LeftNull(Guid leftValue) =>
            Assert.False(Either.Left<Guid, Version>(leftValue).Equals(null));

        [Property(DisplayName = "A Right Either is not equal to null.")]
        public static void ObjectEquals_RightNull(Version rightValue) =>
            Assert.False(Either.Right<Guid, Version>(rightValue).Equals(null));

        [Property(DisplayName = "A Bottom Either is not equal to null.")]
        public static void ObjectEquals_BottomNull() => Assert.False(default(Either<Guid, Version>).Equals(null));

        [Property(DisplayName = "Two Eithers of different type, in different state, with different value are not equal.")]
        public static void ObjectEquals_DifferentType_DifferentState_DifferentValue(
            NonEmptyString leftValue,
            Version rightValue) =>
            Assert.False(Either.Left<string, int>(leftValue.Get).Equals(Either.Right<Guid, Version>(rightValue)));

        [Property(DisplayName = "Two Eithers of different type, in different state, with same value are not equal.")]
        public static void ObjectEquals_DifferentType_DifferentState_SameValue(NonEmptyString value) =>
            Assert.False(Either.Left<string, int>(value.Get).Equals(Either.Right<Guid, string>(value.Get)));

        [Property(DisplayName = "Two Eithers of different type, in same state, with different value are not equal.")]
        public static void ObjectEquals_DifferentType_SameState_DifferentValue(
            NonEmptyString leftValue,
            Guid rightValue) =>
            Assert.False(Either.Left<string, int>(leftValue.Get).Equals(Either.Left<Guid, Version>(rightValue)));

        [Property(DisplayName = "Two Eithers of different type, in same state, with same value are not equal.")]
        public static void ObjectEquals_DifferentType_SameState_SameValue(NonEmptyString value) =>
            Assert.False(Either.Left<string, int>(value.Get).Equals(Either.Left<string, Version>(value.Get)));

        [Property(DisplayName = "Two Eithers of same type, in different state, with different value are not equal.")]
        public static void ObjectEquals_SameType_DifferentState_DifferentValue(NonEmptyString leftValue, int rightValue) =>
            Assert.False(Either.Left<string, int>(leftValue.Get).Equals(Either.Right<string, int>(rightValue)));

        [Property(DisplayName = "Two Eithers of same type, in same state, with different value are not equal.")]
        public static void ObjectEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values) =>
            Assert.False(Either.Left<string, int>(values.Left).Equals(Either.Left<string, int>(values.Right)));

        [Property(DisplayName = "Two Eithers of same type, in same state, with same value are equal.")]
        public static void ObjectEquals_SameType_SameState_SameValue(NonEmptyString value) =>
            Assert.True(Either.Left<string, int>(value.Get).Equals(Either.Left<string, int>(value.Get)));

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public static void ObjectEquals_BottomBottom() =>
            Assert.True(default(Either<string, int>).Equals(default(Either<string, int>)));

        [Property(DisplayName = "A Left Either has a hashcode of its Left value.")]
        public static void GetHashCode_Left(Guid left) =>
            Assert.Equal(left.GetHashCode(), Either.Left<Guid, Version>(left).GetHashCode());

        [Property(DisplayName = "A Right Either has a hashcode of its Right value.")]
        public static void GetHashCode_Right(Version right) =>
            Assert.Equal(right.GetHashCode(), Either.Right<Guid, Version>(right).GetHashCode());

        [Fact(DisplayName = "A Bottom Either has a hashcode of 0.")]
        public static void GetHashCode_Bottom() => Assert.Equal(0, default(Either<Guid, Version>).GetHashCode());
    }
}
