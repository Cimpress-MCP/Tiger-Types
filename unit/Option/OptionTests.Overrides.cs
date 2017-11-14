using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to overrides on <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "A None Option stringifies to None.")]
        static void ToString_None() => Assert.Equal("None", Option<string>.None.ToString());

        [Property(DisplayName = "A Some Option stringifies to a wrapped value.")]
        static void ToString_Some(int some) => Assert.Equal($"Some({some})", Option.From(some).ToString());

        [Fact(DisplayName = "A None Option is not equal to null.")]
        static void ObjectEquals_NoneNull()
        {
            var left = Option<string>.None;
            var right = default(object);

            Assert.False(left.Equals(right));
        }

        [Property(DisplayName = "A Some Option is not equal to null.")]
        static void ObjectEquals_SomeNull(NonNull<string> some)
        {
            var left = Option.From(some.Get);
            var right = default(object);

            Assert.False(left.Equals(right));
        }

        [Fact(DisplayName = "Two None Options of the same type are equal.")]
        static void ObjectEquals_NoneNone_SameType()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.True(left.Equals(right));
        }

        [Fact(DisplayName = "Two None Options of different types are not equal.")]
        static void ObjectEquals_NoneNone_DifferentType()
        {
            var left = Option<string>.None;
            var right = Option<int>.None;

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "A None Option and a Some Option of the same type are not equal.")]
        static void ObjectEquals_NoneSome_SameType(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "A None Option and a Some Option of different types are not equal.")]
        static void ObjectEquals_NoneSome_DifferentType(NonNull<string> some)
        {
            var left = Option<int>.None;
            var right = Option.From(some.Get);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of the same type with different values are not equal.")]
        static void ObjectEquals_SomeSome_SameType_DifferentValue(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of the same type with the same values are equal.")]
        static void ObjectEquals_SomeSome_SameType_SameValue(NonNull<string> some)
        {
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            Assert.True(left.Equals(right));
            Assert.True(right.Equals(left));
        }

        [Property(DisplayName = "Two Some Options of different types are not equal.")]
        static void ObjectEquals_SomeSome_DifferentType(NonNull<string> someLeft, int someRight)
        {
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));
        }

        [Fact(DisplayName = "A None Option has a hashcode of 0.")]
        static void GetHashCode_None() => Assert.Equal(0, Option<string>.None.GetHashCode());

        [Property(DisplayName = "A Some Option has the hashcode of its Some value.")]
        static void GetHashCode_Some(NonNull<string> some) =>
            Assert.Equal(some.Get.GetHashCode(), Option.From(some.Get).GetHashCode());
    }
}
