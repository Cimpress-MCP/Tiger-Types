using System;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparer;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Asking a Left Either for any returns false.")]
        static void Any_Left(Guid left) => Assert.False(Either.Left<Guid, Version>(left).Any());

        [Property(DisplayName = "Asking a Right Either for any returns true.")]
        static void Any_Right(Version right) => Assert.True(Either.Right<Guid, Version>(right).Any());

        [Property(DisplayName = "Asking a Left Either for any returns false.")]
        static void PredicateAny_LeftFalse(Guid left, bool predicate) =>
            Assert.False(Either.Left<Guid, Version>(left).Any(_ => predicate));

        [Property(DisplayName = "Asking a Right Either for any with a false predicate returns false.")]
        static void PredicateAny_RightFalse(Version right) =>
            Assert.False(Either.Right<Guid, Version>(right).Any(_ => false));

        [Property(DisplayName = "Asking a Right Either for any with a true predicate returns true.")]
        static void PredicateAny_RightTrue(Version right) => Assert.True(Either.Right<Guid, Version>(right).Any(_ => true));

        [Property(DisplayName = "Asking a Left Either for all returns true.")]
        static void PredicateAll_LeftFalse(Guid left, bool predicate) =>
            Assert.True(Either.Left<Guid, Version>(left).All(_ => predicate));

        [Property(DisplayName = "Asking a Left Either for all with a true predicate returns true.")]
        static void PredicateAll_LeftTrue(Guid left) => Assert.True(Either.Left<Guid, Version>(left).All(_ => true));

        [Property(DisplayName = "Asking a Right Either for all with a false predicate returns false.")]
        static void PredicateAll_RightFalse(Version right) =>
            Assert.False(Either.Right<Guid, Version>(right).All(_ => false));

        [Property(DisplayName = "Asking a Right Either for all with a true predicate returns true.")]
        static void PredicateAll_RightTrue(Version right) => Assert.True(Either.Right<Guid, Version>(right).All(_ => true));

        [Property(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        static void Contains_Left(Guid left, Version contains) =>
            Assert.False(Either.Left<Guid, Version>(left).Contains(contains));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it doesn't returns false.")]
        static void Contains_Right_False(UnequalPair<Version> values) =>
            Assert.False(Either.Right<Guid, Version>(values.Right).Contains(values.Left));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it does returns true.")]
        static void Contains_Right_True(Version right) =>
            Assert.True(Either.Right<Guid, Version>(right).Contains(right));

        [Property(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        static void ComparerContains_Left(Guid left, NonNull<string> contains) =>
            Assert.False(Either.Left<Guid, string>(left).Contains(contains.Get, Ordinal));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it doesn't returns false.")]
        static void ComparerContains_Right_False(UnequalNonNullPair<string> values) =>
            Assert.False(Either.Right<Guid, string>(values.Right).Contains(values.Left, Ordinal));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it does returns true.")]
        static void ComparerContains_Right_True(NonNull<string> right) =>
            Assert.True(Either.Right<Guid, string>(right.Get).Contains(right.Get, Ordinal));

        [Property(DisplayName = "Recovering a Left Either returns the recovery value.")]
        static void DefaultIfEmpty_Left(NonNull<string> left)
        {
            var actual = Either.Left<string, int>(left.Get).DefaultIfEmpty();

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Either returns the Right value.")]
        static void DefaultIfEmpty_Right(UnequalPair<int> values)
        {
            var actual = Either.Right<string, int>(values.Right).Recover(values.Left);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(values.Right, innerValue);
        }

        [Fact(DisplayName = "Recovering a Bottom Either returns the recovery value.")]
        static void DefaultIfEmpty_Bottom()
        {
            var actual = default(Either<string, int>).DefaultIfEmpty();

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Tapping a Left Either over a func returns a Left Either and performs no action.")]
        static void Do_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Left<string, int>(left.Get).Do(_ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Right Either over a func returns a Right Either and performs an action.")]
        static void Do_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Right<string, int>(right).Do(_ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Left Either does not execute.")]
        static void ForEach_Left(Guid left, Version before)
        {
            var actual = before;
            Either.Left<Guid, Version>(left).ForEach(v => actual = v);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Right Either executes.")]
        static void ForEach_Right(Version right, Version before)
        {
            var actual = before;
            Either.Right<Guid, Version>(right).ForEach(v => actual = v);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Selecting a Left Either produces a Left Either.")]
        static void Select_Left(NonNull<string> left)
        {
            var actual = Either.Left<string, int>(left.Get).Select(v => v + 1);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Selecting a Right Either produces a Right Either.")]
        static void Select_Right(int right)
        {
            var actual = Either.Right<string, int>(right).Select(v => v + 1);

            // assert
            Assert.True(actual.IsRight);
            Assert.Equal(right + 1, actual.Value);
        }

        [Fact(DisplayName = "Selecting a Bottom Either throws.")]
        static void Select_Bottom_Throws()
        {
            var actual = Record.Exception(() => default(Either<string, int>).Select(v => v + 1));

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsBottom, ex.Message);
        }

        [Property(DisplayName = "Selecting from two Left eithers produces a Left either.")]
        static void SelectManyResult_LeftLeft(NonNull<string> leftValue, NonNull<string> rightValue)
        {
            var actual = from l in Either.Left<string, int>(leftValue.Get)
                         from r in Either.Left<string, int>(rightValue.Get)
                         select l + r;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(leftValue.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from a Left Either and a Right Either produces a Left Either.")]
        static void SelectManyResult_LeftRight(NonNull<string> leftValue, int rightValue)
        {
            var actual = from l in Either.Left<string, int>(leftValue.Get)
                         from r in Either.Right<string, int>(rightValue)
                         select l + r;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(leftValue.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from two Right eithers produces a Right either.")]
        static void SelectManyResult_RightRight(int leftValue, int rightValue)
        {
            var actual = from l in Either.Right<string, int>(leftValue)
                         from r in Either.Right<string, int>(rightValue)
                         select l + r;

            // assert
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(leftValue + rightValue, innerValue);
        }

        [Property(DisplayName = "Folding over a Left Either returns the seed value.")]
        static void Aggregate_Left(NonNull<string> left, int seed) =>
            Assert.Equal(seed, Either.Left<string, int>(left.Get).Aggregate(seed, (s, v) => s + v));

        [Property(DisplayName = "Folding over a Right Either returns the result of invoking the accumulator over the seed value and the Right value.")]
        static void Aggregate_Right(int right, int seed) =>
            Assert.Equal(seed + right, Either.Right<string, int>(right).Aggregate(seed, (s, v) => s + v));

        [Property(DisplayName = "Folding over a Left Either returns the seed value.")]
        static void ResultAggregate_Left(NonNull<string> left, int seed) =>
            Assert.Equal(seed * 2, Either.Left<string, int>(left.Get).Aggregate(seed, (s, v) => s + v, v => v * 2));

        [Property(DisplayName = "Folding over a Right Either returns the result of invoking the accumulator over the seed value and the Right value.")]
        static void ResultAggregate_Right(int right, int seed)
        {
            var actual = Either.Right<string, int>(right).Aggregate(seed, (s, v) => s + v, v => v * 2);

            var expected = (seed + right) * 2;
            Assert.Equal(expected, actual);
        }
    }
}
