using System;
using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparer;

// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to extensions to <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "A None Option converted to a Nullable is null.")]
        static void ToNullable_None() => Assert.Null(Option<int>.None.ToNullable());

        [Property(DisplayName = "A Some Option converted to a Nullable is equal to the Some value.")]
        static void ToNullable_Some(int some)
        {
            var actual = Option.From(some).ToNullable();

            Assert.NotNull(actual);
            Assert.Equal(some, actual);
        }

        [Property(DisplayName = "A None Option asserts itself with an exception.")]
        static void Assert_None(NonNull<string> message)
        {
            var actual = Record.Exception(() => Option<int>.None.Assert(() => new InvalidOperationException(message.Get)));

            Assert.NotNull(actual);
            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(message.Get, ioe.Message, Ordinal);
        }

        [Property(DisplayName = "A Some Option asserts itself to be its Some value.")]
        static void Assert_Some(int some, NonNull<string> message) =>
            Assert.Equal(some, Option.From(some).Assert(() => new InvalidOperationException(message.Get)));

        [Fact(DisplayName = "Asking a None Option for any returns false.")]
        static void Any_None() => Assert.False(Option<string>.None.Any());

        [Property(DisplayName = "Asking a Some Option for any returns true.")]
        static void Any_Some(NonNull<string> some) => Assert.True(Option.From(some.Get).Any());

        [Fact(DisplayName = "Asking a None Option for any with a false predicate returns false.")]
        static void PredicateAny_NoneFalse() => Assert.False(Option<string>.None.Any(_ => false));

        [Fact(DisplayName = "Asking a None Option for any with a true predicate returns false.")]
        static void PredicateAny_NoneTrue() => Assert.False(Option<string>.None.Any(_ => true));

        [Property(DisplayName = "Asking a Some Option for any with a false predicate returns false.")]
        static void PredicateAny_SomeFalse(NonNull<string> some) => Assert.False(Option.From(some.Get).Any(_ => false));

        [Property(DisplayName = "Asking a Some Option for any with a true predicate returns true.")]
        static void PredicateAny_SomeTrue(NonNull<string> some) => Assert.True(Option.From(some.Get).Any(_ => true));

        [Fact(DisplayName = "Asking a None Option for all with a false predicate returns true.")]
        static void PredicateAll_NoneFalse() => Assert.True(Option<string>.None.All(_ => false));

        [Fact(DisplayName = "Asking a None Option for all with a true predicate returns true.")]
        static void PredicateAll_NoneTrue() => Assert.True(Option<string>.None.All(_ => true));

        [Property(DisplayName = "Asking a Some Option for all with a false predicate returns false.")]
        static void PredicateAll_SomeFalse(NonNull<string> some) => Assert.False(Option.From(some.Get).All(_ => false));

        [Property(DisplayName = "Asking a Some Option for all with a true predicate returns true.")]
        static void PredicateAll_SomeTrue(NonNull<string> some) => Assert.True(Option.From(some.Get).All(_ => true));

        [Property(DisplayName = "Asking a None option whether it contains a value returns false.")]
        static void Contains_None(int contains) => Assert.False(Option<int>.None.Contains(contains));

        [Property(DisplayName = "Asking a Some option whether it contains a value that it doesn't returns false.")]
        static void Contains_Some_False(UnequalNonNullPair<string> pair) =>
            Assert.False(Option.From(pair.Left).Contains(pair.Right));

        [Property(DisplayName = "Asking a Some option whether it contains a value that it does returns true.")]
        static void Contains_Some_True(NonNull<string> some) => Assert.True(Option.From(some.Get).Contains(some.Get));

        [Property(DisplayName = "Asking a None option whether it contains a value returns false.")]
        static void ComparerContains_None(int contains) =>
            Assert.False(Option<int>.None.Contains(contains, EqualityComparer<int>.Default));

        [Property(DisplayName = "Asking a Some option whether it contains a value that it doesn't returns false.")]
        static void ComparerContains_Some_False(UnequalNonNullPair<string> pair) =>
            Assert.False(Option.From(pair.Left).Contains(pair.Right, OrdinalIgnoreCase));

        [Property(DisplayName = "Asking a Some option whether it contains a value that it does returns true.")]
        static void ComparerContains_Some_True(NonNull<string> some) =>
            Assert.True(Option.From(some.Get).Contains(some.Get.ToUpperInvariant(), OrdinalIgnoreCase));

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        static void DefaultIfEmpty_None()
        {
            var actual = Option<int>.None.DefaultIfEmpty();

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        static void DefaultIfEmpty_Some(int some)
        {
            var actual = Option.From(some).DefaultIfEmpty();

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        static void ValueDefaultIfEmpty_None(NonNull<string> sentinel)
        {
            var actual = Option<string>.None.DefaultIfEmpty(sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        static void ValueDefaultIfEmpty_Some(NonNull<string> some, NonNull<string> sentinel)
        {
            var actual = Option.From(some.Get).DefaultIfEmpty(sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option " +
            "and performs no action.")]
        static void Do_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var output = before.Get;
            var actual = Option<string>.None.Do(v => output = sentinel.Get);

            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
            "and performs an action.")]
        static void Do_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            var value = Option.From(some.Get);

            var output = before.Get;
            var actual = value.Do(v => output = sentinel.Get);

            Assert.Equal(value, actual);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Conditionally executing an action based on a None Option " +
            "does not execute.")]
        static void ForEach_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string>.None.ForEach(v => actual = sentinel.Get);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        static void ForEach_Some(NonNull<string> some, NonNull<string> before)
        {
            var actual = before.Get;
            Option.From(some.Get).ForEach(v => actual = v);

            Assert.Equal(some.Get, actual);
        }

        [Fact(DisplayName = "Selecting a None Option produces a None Option.")]
        static void Select_None()
        {
            var actual = from v in Option<int>.None
                         select v + 1;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting a Some Option produces a Some Option.")]
        static void Select_Some(short some)
        {
            var value = Option.From((int)some);

            var actual = from v in value
                         select v + 1;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some + 1, innerValue);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        static void Where_None(bool filter)
        {
            var actual = from v in Option<int>.None
                         where filter
                         select v;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        static void Where_SomeFalse(int some)
        {
            var actual = from v in Option.From(some)
                         where false
                         select v;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        static void Where_SomeTrue(int some)
        {
            var actual = from v in Option.From(some)
                         where true
                         select v;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Fact(DisplayName = "Selecting from two None Options produces a None Option.")]
        static void SelectManyResult_NoneNone()
        {
            var actual = from l in Option<int>.None
                         from r in Option<int>.None
                         select l + r;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a Some Option and a None Option produces a None Option.")]
        static void SelectManyResult_SomeNone(int someLeft)
        {
            var actual = from l in Option.From(someLeft)
                         from r in Option<int>.None
                         select l + r;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a None Option and a Some Option produces a None Option.")]
        static void SelectManyResult_NoneSome(int someRight)
        {
            var actual = from l in Option<int>.None
                         from r in Option.From(someRight)
                         select l + r;

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from two Some Options produces a Some Option.")]
        static void SelectManyResult_SomeSome(UnequalPair<int> values)
        {
            var actual = from l in Option.From(values.Left)
                         from r in Option.From(values.Right)
                         select (long)l + r;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(values.Left + values.Right, innerValue);
        }

        [Fact(DisplayName = "Binding a None Option over a func returning a None Option returns a None Option.")]
        static void SelectMany_None_None()
        {
            var actual = Option<string>.None.SelectMany(_ => Option<int>.None);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a None Option over a func returning a Some Option returns a None Option.")]
        static void SelectMany_None_Some(int some)
        {
            var actual = Option<string>.None.SelectMany(_ => Option.From(some));

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option returns a None Option.")]
        static void SelectMany_ReturnNone_Some(NonNull<string> some)
        {
            var actual = Option.From(some.Get).SelectMany(_ => Option<int>.None);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option " +
            "returns a Some Option.")]
        static void SelectManyReturnSome_Some(NonNull<string> someString, int someInt)
        {
            var actual = Option.From(someString.Get).SelectMany(_ => Option.From(someInt));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(someInt, innerValue);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        static void Aggregate_None(short seed) =>
            Assert.Equal(seed, Option<string>.None.Aggregate((int)seed, (s, v) => s + v.Length));

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator over the seed value and the Some value.")]
        static void Aggregate_Some(NonNull<string> some, short seed)
        {
            var actual = Option.From(some.Get).Aggregate((int)seed, (s, v) => s + v.Length);

            var expected = some.Get.Length + seed;
            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        static void ResultAggregate_None(short seed)
        {
            var actual = Option<string>.None.Aggregate((int)seed, (s, v) => s + v.Length, v => v * 2);

            var expected = seed * 2;
            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the " +
            "accumulator over the seed value and the Some value.")]
        static void ResultAggregate_Some(NonNull<string> some, short seed)
        {
            var actual = Option.From(some.Get).Aggregate((int)seed, (s, v) => s + v.Length, v => v * 2);

            var expected = (seed + some.Get.Length) * 2;
            Assert.Equal(expected, actual);
        }
    }
}
