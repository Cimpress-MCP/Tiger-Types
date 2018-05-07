using System;
using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "A None Option converted to a Nullable is null.")]
        public static void ToNullable_None() => Assert.Null(Option<int>.None.ToNullable());

        [Property(DisplayName = "A Some Option converted to a Nullable is equal to the Some value.")]
        public static void ToNullable_Some(int some)
        {
            var actual = Option.From(some).ToNullable();

            Assert.NotNull(actual);
            Assert.Equal(some, actual);
        }

        [Property(DisplayName = "A None Option asserts itself with an exception.")]
        public static void Assert_None(NonEmptyString message)
        {
            var actual = Record.Exception(() => Option<int>.None.Assert(() => new InvalidOperationException(message.Get)));

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(message.Get, ioe.Message, StringComparer.Ordinal);
        }

        [Property(DisplayName = "A Some Option asserts itself to be its Some value.")]
        public static void Assert_Some(int some, Func<Exception> none) =>
            Assert.Equal(some, Option.From(some).Assert(none));

        [Fact(DisplayName = "Asking a None Option for any returns false.")]
        public static void Any_None() => Assert.False(Option<string>.None.Any());

        [Property(DisplayName = "Asking a Some Option for any returns true.")]
        public static void Any_Some(NonEmptyString some) => Assert.True(Option.From(some.Get).Any());

        [Property(DisplayName = "Asking an Option for any with a null predicate throws.")]
        public static void PredicateAny_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).Any(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Option for any returns false.")]
        public static void PredicateAny_None(Func<string, bool> predicate) => Assert.False(Option<string>.None.Any(predicate));

        [Property(DisplayName = "Asking a Some Option for any returns the result of the predicate.")]
        public static void PredicateAny_Some(NonEmptyString some, bool result) =>
            Assert.Equal(Option.From(some.Get).Any(_ => result), result);

        [Property(DisplayName = "Asking an Option for all with a null predicate throws.")]
        public static void All_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).All(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Option for all returns true.")]
        public static void All_None(Func<string, bool> predicate) => Assert.True(Option<string>.None.All(predicate));

        [Property(DisplayName = "Asking a Some Option for all returns the result of the predicate.")]
        public static void All_Some(NonEmptyString some, bool result) =>
            Assert.Equal(result, Option.From(some.Get).All(_ => result));

        [Property(DisplayName = "Asking an option whether it contains null throws.")]
        public static void Contains_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).Contains(value: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("value", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Option whether it contains a value returns false.")]
        public static void Contains_None(int contains) => Assert.False(Option<int>.None.Contains(contains));

        [Property(DisplayName = "Asking a Some Option whether it contains a value that it doesn't returns false.")]
        public static void Contains_Some_False(UnequalNonNullPair<string> pair) =>
            Assert.False(Option.From(pair.Left).Contains(pair.Right));

        [Property(DisplayName = "Asking a Some Option whether it contains a value that it does returns true.")]
        public static void Contains_Some_True(NonEmptyString some) => Assert.True(Option.From(some.Get).Contains(some.Get));

        [Property(DisplayName = "Asking an option whether it contains null throws.")]
        public static void ComparerContains_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from)
                .Contains(value: null, equalityComparer: StringComparer.Ordinal));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("value", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Asking a None Option whether it contains a value returns false.")]
        public static void ComparerContains_None(int contains) =>
            Assert.False(Option<int>.None.Contains(contains, EqualityComparer<int>.Default));

        [Property(DisplayName = "Asking a Some Option whether it contains a value that it doesn't returns false.")]
        public static void ComparerContains_Some_False(UnequalNonNullPair<string> pair) =>
            Assert.False(Option.From(pair.Left).Contains(pair.Right, StringComparer.OrdinalIgnoreCase));

        [Property(DisplayName = "Asking a Some Option whether it contains a value that it does returns true.")]
        public static void ComparerContains_Some_True(NonEmptyString some) =>
            Assert.True(Option.From(some.Get).Contains(some.Get.ToUpperInvariant(), StringComparer.OrdinalIgnoreCase));

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void DefaultIfEmpty_None()
        {
            var actual = Option<int>.None.DefaultIfEmpty();

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        public static void DefaultIfEmpty_Some(int some)
        {
            var actual = Option.From(some).DefaultIfEmpty();

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Recovering an Option with null throws.")]
        public static void ValueDefaultIfEmpty_None_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.DefaultIfEmpty(defaultValue: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("defaultValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void ValueDefaultIfEmpty_None(NonEmptyString sentinel)
        {
            var actual = Option<string>.None.DefaultIfEmpty(sentinel.Get);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        public static void ValueDefaultIfEmpty_Some(NonEmptyString some, NonEmptyString sentinel)
        {
            var actual = Option.From(some.Get).DefaultIfEmpty(sentinel.Get);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Tapping an Option over a null func throws.")]
        public static void Do_Some_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Do(onNext: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("onNext", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option and performs no action.")]
        public static void Do_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Do(_ => output = sentinel.Get);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option and performs an action.")]
        public static void Do_Some(NonEmptyString some, NonEmptyString before, NonEmptyString sentinel)
        {
            var output = before.Get;
            var actual = Option.From(some.Get).Do(_ => output = sentinel.Get);

            Assert.True(actual.IsSome);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Conditionally executing a null action based on an Option throws.")]
        public static void ForEach_Some_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).ForEach(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("onNext", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Conditionally executing an action based on a None Option does not execute.")]
        public static void ForEach_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            Option<string>.None.ForEach(_ => actual = sentinel.Get);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        public static void ForEach_Some(NonEmptyString some, NonEmptyString before)
        {
            var actual = before.Get;
            Option.From(some.Get).ForEach(v => actual = v);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Selecting an Option with null throws.")]
        public static void Select_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).Select<string, string>(selector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("selector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting a None Option produces a None Option.")]
        public static void Select_None(Func<int, int> selector) => Assert.True(Option<int>.None.Select(selector).IsNone);

        [Property(DisplayName = "Selecting a Some Option produces a Some Option.")]
        public static void Select_Some(short some)
        {
            var value = Option.From((int)some);

            var actual = from v in value
                         select v + 1;

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some + 1, innerValue);
        }

        [Property(DisplayName = "Filtering an Option with null throws.")]
        public static void Where_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).Where(predicate: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static void Where_None(Func<int, bool> predicate) => Assert.True(Option<int>.None.Where(predicate).IsNone);

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static void Where_SomeFalse(int some)
        {
            var actual = from v in Option.From(some)
                         where false
                         select v;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        public static void Where_SomeTrue(int some)
        {
            var actual = from v in Option.From(some)
                         where true
                         select v;

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Selecting from an Option with null throws.")]
        public static void SelectManyResult_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).SelectMany<string, Option<string>>(selector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("selector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from an Option with a null optional selector throws.")]
        public static void SelectManyResult_NullOptionalSelector_Throws(
            string from,
            Func<string, string, string> resultSelector)
        {
            var actual = Record.Exception(() => Option.From(from)
                .SelectMany(optionalSelector: null, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("optionalSelector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Selecting from an Option with a null result selector throws.")]
        public static void SelectManyResult_NullResultSelector_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from)
                .SelectMany<string, string, string>(optionalSelector: Option.From, resultSelector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("resultSelector", ane.Message, Ordinal);
        }

        [Fact(DisplayName = "Selecting from two None Options produces a None Option.")]
        public static void SelectManyResult_NoneNone()
        {
            var actual = from l in Option<int>.None
                         from r in Option<int>.None
                         select l + r;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a Some Option and a None Option produces a None Option.")]
        public static void SelectManyResult_SomeNone(int someLeft, Func<int, int, int> selector)
        {
            var actual = from l in Option.From(someLeft)
                         from r in Option<int>.None
                         select selector(l, r);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a None Option and a Some Option produces a None Option.")]
        public static void SelectManyResult_NoneSome(int someRight, Func<int, int, int> selector)
        {
            var actual = from l in Option<int>.None
                         from r in Option.From(someRight)
                         select selector(l, r);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from two Some Options produces a Some Option.")]
        public static void SelectManyResult_SomeSome(UnequalPair<int> values, Func<int, int, int> selector)
        {
            var (left, right) = values;
            var actual = from l in Option.From(left)
                         from r in Option.From(right)
                         select selector(l, r);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(selector(left, right), innerValue);
        }

        [Property(DisplayName = "Binding a None Option over a func returns a None Option.")]
        public static void SelectMany_None_Some(string from) =>
            Assert.True(Option<string>.None.SelectMany(_ => Option.From(from)).IsNone);

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option returns a None Option.")]
        public static void SelectMany_ReturnNone_Some(NonEmptyString some) =>
            Assert.True(Option.From(some.Get).SelectMany(_ => Option<int>.None).IsNone);

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option returns a Some Option.")]
        public static void SelectManyReturnSome_Some(NonEmptyString someString, int someInt)
        {
            var actual = Option.From(someString.Get).SelectMany(_ => Option.From(someInt));

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(someInt, innerValue);
        }

        [Property(DisplayName = "Folding over an option with null throws.")]
        public static void Aggregate_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).Aggregate<string, int>(func: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Option returns the default.")]
        public static void Aggregate_None(Func<int, string, int> func) =>
            Assert.Equal(default, Option<string>.None.Aggregate(func));

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator over the default.")]
        public static void Aggregate_Some(NonEmptyString some, Func<int, string, int> func) =>
            Assert.Equal(func(default, some.Get), Option.From(some.Get).Aggregate(func));

        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static void SeededAggregate_Null_Throws(string from, Func<string, string, string> func)
        {
            var actual = Record.Exception(() => Option.From(from).Aggregate(seed: null, func: func));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("seed", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null aggregator throws.")]
        public static void SeededAggregate_NullFunc_Throws(string from, int seed)
        {
            var actual = Record.Exception(() => Option.From(from).Aggregate(seed, func: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void SeededAggregate_None(int seed, Func<int, string, int> func) =>
            Assert.Equal(seed, Option<string>.None.Aggregate(seed, func));

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator over the seed value and the Some value.")]
        public static void SeededAggregate_Some(NonEmptyString some, int seed, Func<int, string, int> func) =>
            Assert.Equal(func(seed, some.Get), Option.From(some.Get).Aggregate(seed, func));

        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static void ResultAggregate_Null_Throws(
            string from,
            Func<string, string, string> func,
            Func<string, string> resultSelector)
        {
            var actual = Record.Exception(() => Option.From(from)
                .Aggregate(seed: null, func: func, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("seed", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null aggregator throws.")]
        public static void ResultAggregate_NullFunc_Throws(string from, int seed, Func<int, int> resultSelector)
        {
            var actual = Record.Exception(() => Option.From(from)
                .Aggregate(seed, func: null, resultSelector: resultSelector));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("func", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null result selector throws.")]
        public static void ResultAggregate_NullResultSelector_Throws(string from, int seed, Func<int, string, int> func)
        {
            var actual = Record.Exception(() => Option.From(from)
                .Aggregate<string, int, int>(seed, func: func, resultSelector: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("resultSelector", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Option returns the transformed seed value.")]
        public static void ResultAggregate_None(int seed, Func<int, string, int> func, Func<int, int> resultSelector) =>
            Assert.Equal(resultSelector(seed), Option<string>.None.Aggregate(seed, func, resultSelector));

        [Property(DisplayName = "Folding over a Some Option returns the transformed result of invoking the accumulator over the seed value and the Some value.")]
        public static void ResultAggregate_Some(
            NonEmptyString some,
            int seed,
            Func<int, string, int> func,
            Func<int, int> resultSelector) =>
            Assert.Equal(resultSelector(func(seed, some.Get)), Option.From(some.Get).Aggregate(seed, func, resultSelector));
    }
}
