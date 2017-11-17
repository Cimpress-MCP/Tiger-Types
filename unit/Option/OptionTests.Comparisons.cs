using System;
using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to comparing <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticEquals_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.True(Option.Equals(left, right));
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticEquals_NoneSome(NonEmptyString some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.False(Option.Equals(left, right));
            Assert.False(Option.Equals(right, left));
        }

        [Property(DisplayName = "Two Some Options are as equal as their Some values.")]
        public static void StaticEquals_SomeSome(NonEmptyString leftSome, NonEmptyString rightSome)
        {
            var left = Option.From(leftSome.Get);
            var right = Option.From(rightSome.Get);

            Assert.Equal(EqualityComparer<string>.Default.Equals(leftSome.Get, rightSome.Get), Option.Equals(left, right));
            Assert.Equal(EqualityComparer<string>.Default.Equals(rightSome.Get, leftSome.Get), Option.Equals(right, left));
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticEqualsComparer_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.True(Option.Equals(left, right, OrdinalIgnoreCase));
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticEqualsComparer_NoneSome(NonEmptyString some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.False(Option.Equals(left, right, OrdinalIgnoreCase));
            Assert.False(Option.Equals(right, left, OrdinalIgnoreCase));
        }

        [Property(DisplayName = "Two Some Options are as equal as their Some values.")]
        public static void StaticEqualsComparer_SomeSome(NonEmptyString leftSome, NonEmptyString rightSome)
        {
            var left = Option.From(leftSome.Get);
            var right = Option.From(rightSome.Get);

            Assert.Equal(Ordinal.Equals(leftSome.Get, rightSome.Get), Option.Equals(left, right, Ordinal));
            Assert.Equal(Ordinal.Equals(rightSome.Get, leftSome.Get), Option.Equals(right, left, Ordinal));
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticCompare_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.Equal(0, Option.Compare(left, right));
            Assert.Equal(0, Option.Compare(right, left));
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticCompare_NoneSome(NonEmptyString some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(-1, Option.Compare(left, right));
            Assert.Equal(1, Option.Compare(right, left));
        }

        [Property(DisplayName = "Two Some Options compare the same as their Some values.")]
        public static void StaticCompare_SomeSome(NonEmptyString leftSome, NonEmptyString rightSome)
        {
            var left = Option.From(leftSome.Get);
            var right = Option.From(rightSome.Get);
            // note(cosborn) Normalize the return value of `Default.Compare`.
            int DefaultCompare(string x, string y) => Comparer<string>.Default.Compare(x, y).Pipe(Math.Sign);

            Assert.Equal(DefaultCompare(leftSome.Get, rightSome.Get), Option.Compare(left, right));
            Assert.Equal(DefaultCompare(rightSome.Get, leftSome.Get), Option.Compare(right, left));
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticCompareComparer_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.Equal(0, Option.Compare(left, right, OrdinalIgnoreCase));
            Assert.Equal(0, Option.Compare(right, left, OrdinalIgnoreCase));
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticCompareComparer_NoneSome(NonEmptyString some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(-1, Option.Compare(left, right, OrdinalIgnoreCase));
            Assert.Equal(1, Option.Compare(right, left, OrdinalIgnoreCase));
        }

        [Property(DisplayName = "Two Some Options compare the same as their Some values.")]
        public static void StaticCompareComparer_SomeSome(NonEmptyString leftSome, NonEmptyString rightSome)
        {
            var left = Option.From(leftSome.Get);
            var right = Option.From(rightSome.Get);
            // note(cosborn) Normalize the return value of `Ordinal.Compare`.
            int OrdinalCompare(string x, string y) => Ordinal.Compare(x, y).Pipe(Math.Sign);

            Assert.Equal(OrdinalCompare(leftSome.Get, rightSome.Get), Option.Compare(left, right, Ordinal));
            Assert.Equal(OrdinalCompare(rightSome.Get, leftSome.Get), Option.Compare(right, left, Ordinal));
        }
    }
}
