using System;
using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Try{TErr, TOk}"/>.</summary>
    public static partial class TryTests
    {
        [Property(DisplayName = "A None Try asserts itself with an exception.")]
        public static void Assert_None(NonEmptyString noneMessage, NonEmptyString errMessage)
        {
            var actual = Record.Exception(() => Try<string, Version>.None.Assert(
                () => new InvalidOperationException(noneMessage.Get),
                _ => new InvalidOperationException(errMessage.Get)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(noneMessage.Get, ioe.Message, StringComparer.Ordinal);
        }

        [Property(DisplayName = "An Err Try asserts itself with an exception.")]
        public static void Assert_Err(
            NonEmptyString err,
            NonEmptyString noneMessage,
            NonEmptyString errMessage)
        {
            var actual = Record.Exception(() => Try<string, Version>.From(err.Get).Assert(
                () => new InvalidOperationException(noneMessage.Get),
                _ => new InvalidOperationException(errMessage.Get)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(errMessage.Get, ioe.Message, StringComparer.Ordinal);
        }

        [Property(DisplayName = "An Ok Try asserts itself to be its Ok value.")]
        public static void Assert_Ok(
            Version ok,
            NonEmptyString noneMessage,
            NonEmptyString errMessage)
        {
            var actual = Try<string, Version>.From(ok).Assert(
                () => new InvalidOperationException(noneMessage.Get),
                _ => new InvalidOperationException(errMessage.Get));

            Assert.Equal(ok, actual);
        }

        [Property(DisplayName = "A None Try asserts itself with an exception.")]
        public static void ExceptionalAssert_None(NonEmptyString noneMessage)
        {
            var actual = Record.Exception(() => Try<Exception, Version>.None.Assert(
                () => new InvalidOperationException(noneMessage.Get)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(noneMessage.Get, ioe.Message, StringComparer.Ordinal);
        }

        [Property(DisplayName = "An Err Try asserts itself with an exception.")]
        public static void ExceptionalAssert_Err(NonEmptyString noneMessage, NonEmptyString errMessage)
        {
            var ex = new InvalidOperationException(errMessage.Get);
            var actual = Record.Exception(() => Try<Exception, Version>.From(ex).Assert(
                () => new InvalidOperationException(noneMessage.Get)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(errMessage.Get, ioe.Message, StringComparer.Ordinal);
        }

        [Property(DisplayName = "An Ok Try asserts itself to be its Ok value.")]
        public static void ExceptionalAssert_Ok(Version ok, NonEmptyString noneMessage)
        {
            var actual = Try<Exception, Version>.From(ok).Assert(
                () => new InvalidOperationException(noneMessage.Get));

            Assert.Equal(ok, actual);
        }

        [Fact(DisplayName = "Asking a None Try for any returns false.")]
        public static void Any_None() => Assert.False(Try<string, Version>.None.Any());

        [Property(DisplayName = "Asking an Err Try for any returns false.")]
        public static void Any_Err(NonEmptyString err) =>
            Assert.False(Try<string, Version>.From(err.Get).Any());

        [Property(DisplayName = "Asking an Err Try for any returns false.")]
        public static void Any_Ok(Version ok) =>
            Assert.True(Try<string, Version>.From(ok).Any());

        [Property(DisplayName = "Asking an Option for any with a null predicate throws.")]
        public static void PredicateAny_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Any(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Try for any with any predicate returns false.")]
        public static void PredicateAny_None(Func<Version, bool> predicate) =>
            Assert.False(Try<string, Version>.None.Any(predicate));

        [Property(DisplayName = "Asking an Err Try for any with any predicate returns false.")]
        public static void PredicateAny_Err(NonEmptyString err, Func<Version, bool> predicate) =>
            Assert.False(Try<string, Version>.From(err.Get).Any(predicate));

        [Property(DisplayName = "Asking an Ok Try for any returns the result of the predicate.")]
        public static void PredicateAny_OkFalse(Version ok, bool result) =>
            Assert.Equal(result, Try.From<string, Version>(ok).Any(_ => result));

        [Property(DisplayName = "Asking a Try for all with a null predicate throws.")]
        public static void All_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.All(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Try for all returns true.")]
        public static void All_None(Func<Version, bool> predicate) =>
            Assert.True(Try<string, Version>.None.All(predicate));

        [Property(DisplayName = "Asking an Err Try for all returns true.")]
        public static void All_Err(NonEmptyString err, Func<Version, bool> predicate) =>
            Assert.True(Try<string, Version>.From(err.Get).All(predicate));

        [Property(DisplayName = "Asking an Ok Try for all returns the result of the predicate.")]
        public static void All_Ok(Version some, bool result) =>
            Assert.Equal(result, Try<string, Version>.From(some).All(_ => result));

        [Property(DisplayName = "Asking a try whether it contains null throws.")]
        public static void Contains_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Contains(value: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("value", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Try whether it contains a value returns false.")]
        public static void Contains_None(Version contains) =>
            Assert.False(Try<string, Version>.None.Contains(contains));

        [Property(DisplayName = "Asking an Err Try whether it contains a value returns false.")]
        public static void Contains_Err(NonEmptyString err, Version contains) =>
            Assert.False(Try<string, Version>.From(err.Get).Contains(contains));

        [Property(DisplayName = "Asking an Ok Try whether it contains a value that it doesn't returns false.")]
        public static void Contains_Ok_False(UnequalNonNullPair<Version> pair) =>
            Assert.False(Try<string, Version>.From(pair.Left).Contains(pair.Right));

        [Property(DisplayName = "Asking an Ok Try whether it contains a value that it does returns true.")]
        public static void Contains_Ok_True(Version ok) =>
            Assert.True(Try<string, Version>.From(ok).Contains(ok));

        [Property(DisplayName = "Asking a Try whether it contains null throws.")]
        public static void ComparerContains_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Contains(value: null, EqualityComparer<Version>.Default));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("value", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Try whether it contains a value returns false.")]
        public static void ComparerContains_None(Version contains) =>
            Assert.False(Try<string, Version>.None.Contains(contains, EqualityComparer<Version>.Default));

        [Property(DisplayName = "Asking an Err Try whether it contains a value returns false.")]
        public static void ComparerContains_Err(NonEmptyString err, Version contains) =>
            Assert.False(Try<string, Version>.From(err.Get).Contains(contains, EqualityComparer<Version>.Default));

        [Property(DisplayName = "Asking an Ok Try whether it contains a value that it doesn't returns false.")]
        public static void ComparerContains_Ok_False(UnequalNonNullPair<Version> pair) =>
            Assert.False(Try<string, Version>.From(pair.Left).Contains(pair.Right, EqualityComparer<Version>.Default));

        [Property(DisplayName = "Asking an Ok Try whether it contains a value that it does returns true.")]
        public static void ComparerContains_Some_True(Version ok) =>
            Assert.True(Try<string, Version>.From(ok).Contains(ok, EqualityComparer<Version>.Default));

        [Fact(DisplayName = "Recovering a None Try returns the recovery value.")]
        public static void DefaultIfEmpty_None()
        {
            var actual = Try<string, int>.None.DefaultIfEmpty();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(default, innerValue);
        }

        [Property(DisplayName = "Recovering an Err Try returns the recovery value.")]
        public static void DefaultIfEmpty_Err(NonEmptyString err)
        {
            var actual = Try<string, int>.From(err.Get).DefaultIfEmpty();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(default, innerValue);
        }

        [Property(DisplayName = "Recovering an Ok Try returns the Ok value.")]
        public static void DefaultIfEmpty_Ok(int ok)
        {
            var actual = Try<string, int>.From(ok).DefaultIfEmpty();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Recovering a Try with null throws.")]
        public static void ValueDefaultIfEmpty_None_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.DefaultIfEmpty(defaultValue: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("defaultValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a None Try returns the recovery value.")]
        public static void ValueDefaultIfEmpty_None(Version sentinel)
        {
            var actual = Try<string, Version>.None.DefaultIfEmpty(sentinel);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Recovering an Err Try returns the recovery value.")]
        public static void ValueDefaultIfEmpty_Err(NonEmptyString err, Version sentinel)
        {
            var actual = Try<string, Version>.From(err.Get).DefaultIfEmpty(sentinel);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Recovering an Ok Try returns the Ok value.")]
        public static void ValueDefaultIfEmpty_Some(UnequalNonNullPair<Version> pair)
        {
            var actual = Try<string, Version>.From(pair.Left).DefaultIfEmpty(pair.Right);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(pair.Left, innerValue);
        }

        [Property(DisplayName = "Tapping a Try over a null func throws.")]
        public static void Do_Some_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Do(onNext: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("onNext", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Try over a func returns a None Try and performs no action.")]
        public static void Do_None(Version before, Version sentinel)
        {
            var output = before;
            var actual = Try<string, Version>.None.Do(_ => output = sentinel);

            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping an Err Try over a func returns nan Err Try and performs no action.")]
        public static void Do_Err(NonEmptyString err, Version before, Version sentinel)
        {
            var output = before;
            var actual = Try<string, Version>.From(err.Get).Do(_ => output = sentinel);

            Assert.True(actual.IsErr);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping an Ok Try over a func returns an Ok Try and performs an action.")]
        public static void Do_Some(Version ok, Version before, Version sentinel)
        {
            var output = before;
            var actual = Try<string, Version>.From(ok).Do(_ => output = sentinel);

            Assert.True(actual.IsOk);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Conditionally executing a null action based on a Try throws.")]
        public static void ForEach_Some_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.ForEach(onNext: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("onNext", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing an action based on a None Try does not execute.")]
        public static void ForEach_None(Version before, Version sentinel)
        {
            var actual = before;
            Try<string, Version>.None.ForEach(_ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on an Err Try does not execute.")]
        public static void ForEach_Err(NonEmptyString err, Version before, Version sentinel)
        {
            var actual = before;
            Try<string, Version>.From(err.Get).ForEach(_ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on an Ok Try executes.")]
        public static void ForEach_Some(Version ok, Version before)
        {
            var actual = before;
            Try<string, Version>.From(ok).ForEach(v => actual = v);

            Assert.Equal(ok, actual);
        }

        [Property(DisplayName = "Selecting a Try with null throws.")]
        public static void Select_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Select<string, Version, int>(selector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("selector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting a None Try produces a None Try.")]
        public static void Select_None(Func<Version, int> selector) =>
            Assert.True(Try<string, Version>.None.Select(selector).IsNone);

        [Property(DisplayName = "Selecting an Err Try produces an Err Try.")]
        public static void Select_Err(NonEmptyString err, Func<Version, int> selector) =>
            Assert.True(Try<string, Version>.From(err.Get).Select(selector).IsErr);

        [Property(DisplayName = "Selecting an Ok Try produces an Ok Try.")]
        public static void Select_Some(Version ok)
        {
            var value = Try<string, Version>.From(ok);

            var actual = from v in value
                         select v.Major;

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok.Major, innerValue);
        }

        [Property(DisplayName = "Filtering a Try with null throws.")]
        public static void Where_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Where(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Filtering a None Try produces a None Try.")]
        public static void Where_None(Func<Version, bool> predicate) =>
            Assert.True(Try<string, Version>.None.Where(predicate).IsNone);

        [Property(DisplayName = "Filtering an Err Try produces a None Try.")]
        public static void Where_Err(NonEmptyString err, Func<Version, bool> predicate) =>
            Assert.True(Try<string, Version>.From(err.Get).Where(predicate).IsNone);

        [Property(DisplayName = "Filtering an Ok Try with a false predicate produces a None Try.")]
        public static void Where_SomeFalse(Version ok)
        {
            var actual = from v in Try<string, Version>.From(ok)
                         where false
                         select v;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering an Ok Try with a true predicate produces an Ok Try.")]
        public static void Where_SomeTrue(Version ok)
        {
            var actual = from v in Try<string, Version>.From(ok)
                         where true
                         select v;

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Selecting from a Try with null throws.")]
        public static void SelectManyResult_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.SelectMany<string, Version, int>(selector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("selector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from a Try with a null try selector throws.")]
        public static void SelectManyResult_NullTrySelector_Throws(
            Try<string, Version> @try,
            Func<Version, int, string> resultSelector)
        {
            var actual = Record.Exception(() => @try
                .SelectMany(trySelector: null, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("trySelector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from a Try with a null result selector throws.")]
        public static void SelectManyResult_NullResultSelector_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try
                .SelectMany<string, Version, Version, int>(trySelector: Try<string, Version>.From, resultSelector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("resultSelector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from a None Try and anything produces a None Try.")]
        public static void SelectManyResult_NoneAnything(Try<string, Version> right, Func<Version, Version, int> selector)
        {
            var actual = from l in Try<string, Version>.None
                         from r in right
                         select selector(l, r);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from an Err Try and anything produces an Err Try.")]
        public static void SelectManyResult_ErrAnything(
            NonEmptyString err,
            Try<string, Version> right,
            Func<Version, Version, int> selector)
        {
            var actual = from l in Try<string, Version>.From(err.Get)
                         from r in right
                         select selector(l, r);

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from an Ok Try and a None Try produces a None Try.")]
        public static void SelectManyResult_AnythingNone(
            Version ok,
            Func<Version, Version, int> selector)
        {
            var actual = from l in Try<string, Version>.From(ok)
                         from r in Try<string, Version>.None
                         select selector(l, r);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from an Ok Try and an Err Try produces an Err Try.")]
        public static void SelectManyResult_OkErr(
            Version ok,
            NonEmptyString err,
            Func<Version, Version, int> selector)
        {
            var actual = from l in Try<string, Version>.From(ok)
                         from r in Try<string, Version>.From(err.Get)
                         select selector(l, r);

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "Selecting from an Ok Try and an Ok Try produces an Ok Try.")]
        public static void SelectManyResult_OkOk(
            Version left,
            Version right,
            Func<Version, Version, int> selector)
        {
            var actual = from l in Try<string, Version>.From(left)
                         from r in Try<string, Version>.From(right)
                         select selector(l, r);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            var expected = selector(left, right);
            Assert.Equal(expected, innerValue);
        }

        [Property(DisplayName = "Binding a None Try over a func returns a None Try.")]
        public static void SelectMany_None_Any(Func<Version, Try<string, int>> selector) =>
            Assert.True(Try<string, Version>.None.SelectMany(selector).IsNone);

        [Property(DisplayName = "Binding an Err Try over a func returns an Err Try.")]
        public static void SelectMany_Err_Any(NonEmptyString err, Func<Version, Try<string, int>> selector) =>
            Assert.True(Try<string, Version>.From(err.Get).SelectMany(selector).IsErr);

        [Property(DisplayName = "Binding an Ok Try over a func returning a None Try returns a None Try.")]
        public static void SelectMany_OkNone_None(Version ok) =>
            Assert.True(Try<string, Version>.From(ok).SelectMany(_ => Try<string, int>.None).IsNone);

        [Property(DisplayName = "Binding an Ok Try over a func returning an Err Try returns an Err Try.")]
        public static void SelectMany_OkErr_Err(Version ok, NonEmptyString err) =>
            Assert.True(Try<string, Version>.From(ok).SelectMany(_ => Try<string, int>.From(err.Get)).IsErr);

        [Property(DisplayName = "Binding an Ok Try over a func returning an Ok Try returns an Ok Try.")]
        public static void SelectManyReturnSome_Some(Version ok, int major)
        {
            var actual = Try<string, Version>.From(ok).SelectMany(_ => Try<string, int>.From(major));

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(major, innerValue);
        }

        [Property(DisplayName = "Folding over a Try with null throws.")]
        public static void Aggregate_Null_Throws(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.Aggregate<string, Version, int>(func: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Try returns the default.")]
        public static void Aggregate_None(Func<int, Version, int> func) =>
            Assert.Equal(default, Try<string, Version>.None.Aggregate(func));

        [Property(DisplayName = "Folding over a None Try returns the default.")]
        public static void Aggregate_Err(NonEmptyString err, Func<int, Version, int> func) =>
            Assert.Equal(default, Try<string, Version>.From(err.Get).Aggregate(func));

        [Property(DisplayName = "Folding over an Ok Try returns the result of invoking the accumulator over the default.")]
        public static void Aggregate_Some(Version ok, Func<int, Version, int> func) =>
            Assert.Equal(func(default, ok), Option.From(ok).Aggregate(func));

        [Property(DisplayName = "Folding over a Try with a null seed throws.")]
        public static void SeededAggregate_Null_Throws(Try<string, Version> @try, Func<string, Version, string> func)
        {
            var actual = Record.Exception(() => @try.Aggregate(seed: null, func: func));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("seed", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a Try with a null aggregator throws.")]
        public static void SeededAggregate_NullFunc_Throws(Try<string, Version> @try, NonEmptyString seed)
        {
            var actual = Record.Exception(() => @try.Aggregate(seed.Get, func: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Try returns the seed value.")]
        public static void SeededAggregate_None(int seed, Func<int, Version, int> func) =>
            Assert.Equal(seed, Try<string, Version>.None.Aggregate(seed, func));

        [Property(DisplayName = "Folding over an Err Try returns the seed value.")]
        public static void SeededAggregate_Err(NonEmptyString err, int seed, Func<int, Version, int> func) =>
            Assert.Equal(seed, Try<string, Version>.From(err.Get).Aggregate(seed, func));

        [Property(DisplayName = "Folding over an Ok Try returns the result of invoking the accumulator " +
                                "over the seed value and the Ok value.")]
        public static void SeededAggregate_Ok(Version ok, int seed, Func<int, Version, int> func) =>
            Assert.Equal(func(seed, ok), Try<string, Version>.From(ok).Aggregate(seed, func));

        [Property(DisplayName = "Folding over a Try with a null seed throws.")]
        public static void ResultAggregate_Null_Throws(
            Try<string, Version> @try,
            Func<string, Version, string> func,
            Func<string, int> resultSelector)
        {
            var actual = Record.Exception(() => @try
                .Aggregate(seed: null, func: func, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("seed", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a Try with a null aggregator throws.")]
        public static void ResultAggregate_NullFunc_Throws(
            Try<string, Version> @try,
            int seed,
            Func<int, string> resultSelector)
        {
            var actual = Record.Exception(() => @try
                .Aggregate(seed, func: null, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null result selector throws.")]
        public static void ResultAggregate_NullResultSelector_Throws(
            Try<string, Version> @try,
            int seed,
            Func<int, Version, int> func)
        {
            var actual = Record.Exception(() => @try
                .Aggregate<string, Version, int, int>(seed, func: func, resultSelector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("resultSelector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Try returns the transformed seed value.")]
        public static void ResultAggregate_None(int seed, Func<int, Version, int> func, Func<int, int> resultSelector) =>
            Assert.Equal(resultSelector(seed), Try<string, Version>.None.Aggregate(seed, func, resultSelector));

        [Property(DisplayName = "Folding over an Ok Try returns the transformed result of invoking the accumulator over the seed value and the Ok value.")]
        public static void ResultAggregate_Ok(
            Version ok,
            int seed,
            Func<int, Version, int> func,
            Func<int, int> resultSelector) =>
            Assert.Equal(resultSelector(func(seed, ok)), Try<string, Version>.From(ok).Aggregate(seed, func, resultSelector));
    }
}
