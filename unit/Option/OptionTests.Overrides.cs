using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to overrides on <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "A None Option stringifies to None.")]
        public static void ToString_None() => Assert.Equal("None", Option<string>.None.ToString());

        [Property(DisplayName = "A Some Option stringifies to a wrapped value.")]
        public static void ToString_Some(int some) => Assert.Equal($"Some({some})", Option.From(some).ToString());

        [Fact(DisplayName = "A None Option is not equal to null.")]
        public static void ObjectEquals_NoneNull()
        {
            var left = Option<string>.None;
            var right = default(object);

            Assert.False(left.Equals(right));
        }

        [Property(DisplayName = "A Some Option is not equal to null.")]
        public static void ObjectEquals_SomeNull(NonEmptyString some)
        {
            var left = Option.From(some.Get);
            var right = default(object);

            Assert.False(left.Equals(right));
        }

        [Fact(DisplayName = "Two None Options of the same type are equal.")]
        public static void ObjectEquals_NoneNone_SameType()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.True(left.Equals(right));
        }

        [Fact(DisplayName = "Two None Options of different types are not equal.")]
        public static void ObjectEquals_NoneNone_DifferentType()
        {
            var left = Option<string>.None;
            var right = Option<int>.None;

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "A None Option and a Some Option of the same type are not equal.")]
        public static void ObjectEquals_NoneSome_SameType(NonEmptyString some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "A None Option and a Some Option of different types are not equal.")]
        public static void ObjectEquals_NoneSome_DifferentType(NonEmptyString some)
        {
            var left = Option<int>.None;
            var right = Option.From(some.Get);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of the same type with different values are not equal.")]
        public static void ObjectEquals_SomeSome_SameType_DifferentValue(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of the same type with the same values are equal.")]
        public static void ObjectEquals_SomeSome_SameType_SameValue(NonEmptyString some)
        {
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            Assert.True(left.Equals(right));
            Assert.True(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of different types are not equal.")]
        public static void ObjectEquals_SomeSome_DifferentType(NonEmptyString someLeft, int someRight)
        {
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Fact(DisplayName = "A None Option has a hashcode of 0.")]
        public static void GetHashCode_None() => Assert.Equal(0, Option<string>.None.GetHashCode());

        [Property(DisplayName = "A Some Option has the hashcode of its Some value.")]
        public static void GetHashCode_Some(NonEmptyString some) =>
            Assert.Equal(EqualityComparer<string>.Default.GetHashCode(some.Get), Option.From(some.Get).GetHashCode());
    }
}
