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
        [Property(DisplayName = "A Left Either asserts itself with an exception.")]
        public static void Assert_Left(NonEmptyString left)
        {
            var actual = Record.Exception(() =>
                Either<string, int>.From(left.Get).Assert(s => new InvalidOperationException(s)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(left.Get, ioe.Message, Ordinal);
        }

        [Property(DisplayName ="A Right Either asserts itself to be its Right value.")]
        public static void Assert_Right(int right, Func<string, Exception> left) =>
            Assert.Equal(right, Either<string, int>.From(right).Assert(left));

        [Property(DisplayName = "Asking a Left Either for any returns false.")]
        public static void Any_Left(Guid left) => Assert.False(Either.From<Guid, Version>(left).Any());

        [Property(DisplayName = "Asking a Right Either for any returns true.")]
        public static void Any_Right(Version right) => Assert.True(Either.From<Guid, Version>(right).Any());

        [Property(DisplayName = "Asking a Left Either for any returns false.")]
        public static void PredicateAny_LeftFalse(Guid left, bool predicate) =>
            Assert.False(Either.From<Guid, Version>(left).Any(_ => predicate));

        [Property(DisplayName = "Asking a Right Either for any with a false predicate returns false.")]
        public static void PredicateAny_RightFalse(Version right) =>
            Assert.False(Either.From<Guid, Version>(right).Any(_ => false));

        [Property(DisplayName = "Asking a Right Either for any with a true predicate returns true.")]
        public static void PredicateAny_RightTrue(Version right) => Assert.True(Either.From<Guid, Version>(right).Any(_ => true));

        [Property(DisplayName = "Asking a Left Either for all returns true.")]
        public static void PredicateAll_LeftFalse(Guid left, Func<Version, bool> predicate) =>
            Assert.True(Either.From<Guid, Version>(left).All(predicate));

        [Property(DisplayName = "Asking a Left Either for all with a true predicate returns true.")]
        public static void PredicateAll_LeftTrue(Guid left) => Assert.True(Either.From<Guid, Version>(left).All(_ => true));

        [Property(DisplayName = "Asking a Right Either for all with a false predicate returns false.")]
        public static void PredicateAll_RightFalse(Version right) =>
            Assert.False(Either.From<Guid, Version>(right).All(_ => false));

        [Property(DisplayName = "Asking a Right Either for all with a true predicate returns true.")]
        public static void PredicateAll_RightTrue(Version right) => Assert.True(Either.From<Guid, Version>(right).All(_ => true));

        [Property(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        public static void Contains_Left(Guid left, Version contains) =>
            Assert.False(Either.From<Guid, Version>(left).Contains(contains));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it doesn't returns false.")]
        public static void Contains_Right_False(UnequalPair<Version> values) =>
            Assert.False(Either.From<Guid, Version>(values.Right).Contains(values.Left));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it does returns true.")]
        public static void Contains_Right_True(Version right) =>
            Assert.True(Either.From<Guid, Version>(right).Contains(right));

        [Property(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        public static void ComparerContains_Left(Guid left, NonEmptyString contains) =>
            Assert.False(Either.From<Guid, string>(left).Contains(contains.Get, StringComparer.Ordinal));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it doesn't returns false.")]
        public static void ComparerContains_Right_False(UnequalNonNullPair<string> values) =>
            Assert.False(Either.From<Guid, string>(values.Right).Contains(values.Left, StringComparer.Ordinal));

        [Property(DisplayName = "Asking a Right Either whether it contains a value that it does returns true.")]
        public static void ComparerContains_Right_True(NonEmptyString right) =>
            Assert.True(Either.From<Guid, string>(right.Get).Contains(right.Get, StringComparer.Ordinal));

        [Property(DisplayName = "Recovering a Left Either returns the recovery value.")]
        public static void DefaultIfEmpty_Left(NonEmptyString left)
        {
            var actual = Either.From<string, int>(left.Get).DefaultIfEmpty();

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Recovering a Right Either returns the Right value.")]
        public static void DefaultIfEmpty_Right(UnequalPair<int> values)
        {
            var actual = Either.From<string, int>(values.Right).Recover(values.Left);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(values.Right, innerValue);
        }

        [Fact(DisplayName = "Recovering a Bottom Either returns the recovery value.")]
        public static void DefaultIfEmpty_Bottom()
        {
            var actual = default(Either<string, int>).DefaultIfEmpty();

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Tapping a Left Either over a func returns a Left Either and performs no action.")]
        public static void Do_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.From<string, int>(left.Get).Do(_ => output = sentinel);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Right Either over a func returns a Right Either and performs an action.")]
        public static void Do_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.From<string, int>(right).Do(_ => output = sentinel);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Left Either does not execute.")]
        public static void ForEach_Left(Guid left, Version before)
        {
            var actual = before;
            Either.From<Guid, Version>(left).ForEach(v => actual = v);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Right Either executes.")]
        public static void ForEach_Right(Version right, Version before)
        {
            var actual = before;
            Either.From<Guid, Version>(right).ForEach(v => actual = v);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Selecting a Left Either produces a Left Either.")]
        public static void Select_Left(NonEmptyString left)
        {
            var actual = Either.From<string, int>(left.Get).Select(v => v + 1);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Selecting a Right Either produces a Right Either.")]
        public static void Select_Right(int right)
        {
            var actual = Either.From<string, int>(right).Select(v => v + 1);

            Assert.True(actual.IsRight);
            Assert.Equal(right + 1, actual.Value);
        }

        [Fact(DisplayName = "Selecting a Bottom Either throws.")]
        public static void Select_Bottom_Throws()
        {
            var actual = Record.Exception(() => default(Either<string, int>).Select(v => v + 1));

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(EitherIsBottom, ex.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from two Left eithers produces a Left either.")]
        public static void SelectManyResult_LeftLeft(NonEmptyString leftValue, NonEmptyString rightValue)
        {
            var actual = from l in Either.From<string, int>(leftValue.Get)
                         from r in Either.From<string, int>(rightValue.Get)
                         select l + r;

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(leftValue.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from a Left Either and a Right Either produces a Left Either.")]
        public static void SelectManyResult_LeftRight(NonEmptyString leftValue, int rightValue)
        {
            var actual = from l in Either.From<string, int>(leftValue.Get)
                         from r in Either.From<string, int>(rightValue)
                         select l + r;

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(leftValue.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from two Right eithers produces a Right either.")]
        public static void SelectManyResult_RightRight(int leftValue, int rightValue)
        {
            var actual = from l in Either.From<string, int>(leftValue)
                         from r in Either.From<string, int>(rightValue)
                         select l + r;

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(leftValue + rightValue, innerValue);
        }

        [Property(DisplayName = "Folding over a Left Either returns the seed value.")]
        public static void Aggregate_Left(NonEmptyString left, int seed) =>
            Assert.Equal(seed, Either.From<string, int>(left.Get).Aggregate(seed, (s, v) => s + v));

        [Property(DisplayName = "Folding over a Right Either returns the result of invoking the accumulator over the seed value and the Right value.")]
        public static void Aggregate_Right(int right, int seed) =>
            Assert.Equal(seed + right, Either.From<string, int>(right).Aggregate(seed, (s, v) => s + v));

        [Property(DisplayName = "Folding over a Left Either returns the seed value.")]
        public static void ResultAggregate_Left(NonEmptyString left, int seed) =>
            Assert.Equal(seed * 2, Either.From<string, int>(left.Get).Aggregate(seed, (s, v) => s + v, v => v * 2));

        [Property(DisplayName = "Folding over a Right Either returns the result of invoking the accumulator over the seed value and the Right value.")]
        public static void ResultAggregate_Right(int right, int seed)
        {
            var actual = Either.From<string, int>(right).Aggregate(seed, (s, v) => s + v, v => v * 2);

            var expected = (seed + right) * 2;
            Assert.Equal(expected, actual);
        }
    }
}
