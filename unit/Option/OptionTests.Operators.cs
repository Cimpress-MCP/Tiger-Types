using System;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to operators of <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Two None Options are equal.")]
        static void OperatorEquals_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        static void OperatorEquals_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        static void OperatorEquals_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            Assert.False(left == right);
            Assert.False(right == left);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        static void OperatorEquals_SomeSome_SameValue(NonNull<string> some)
        {
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            Assert.True(left == right);
            Assert.True(right == left);
        }

        [Fact(DisplayName = "Two None Options are not unequal.")]
        static void OperatorNotEquals_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Property(DisplayName = "A None Option and a Some Option are unequal.")]
        static void OperatorNotEquals_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Some Options with different values are unequal.")]
        static void OperatorNotEquals_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "Two Some Options with the same values are not unequal.")]
        static void OperatorNotEquals_SomeSome_SameValue(NonNull<string> some)
        {
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);
            
            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        static void OperatorBitwiseOr_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        static void NamedBitwiseOr_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        static void OperatorLogicalOr_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        static void OperatorBitwiseOr_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(right, left | right);
            Assert.Equal(right, right | left);
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        static void NamedBitwiseOr_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(right, left.BitwiseOr(right));
            Assert.Equal(right, right.BitwiseOr(left));
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        static void OperatorLogicalOr_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(right, left || right);
            Assert.Equal(right, right || left);
        }

        [Property(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        static void OperatorBitwiseOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);
            
            Assert.Equal(left, left | right);
            Assert.Equal(right, right | left);
        }

        [Property(DisplayName = "The disjunction of two Some Options is the latter Some Option.")]
        static void NamedBitwiseOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);
            
            Assert.Equal(left, left.BitwiseOr(right));
            Assert.Equal(right, right.BitwiseOr(left));
        }

        [Property(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        static void OperatorLogicalOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            Assert.Equal(left, left || right);
            Assert.Equal(right, right || left);
        }

        [Fact(DisplayName = "A None Option does not evaluate as true.")]
        static void OperatorIsTrue_None()
        {
            bool actual;
            if (Option<string>.None)
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            Assert.False(actual);
        }

        [Fact(DisplayName = "A None Option does not evaluate as true.")]
        static void NamedIsTrue_None() => Assert.False(Option<string>.None.IsTrue);

        [Property(DisplayName = "A Some Option evaluates as true.")]
        static void NamedIsTrue_Some(NonNull<string> some) => Assert.True(Option.From(some.Get).IsTrue);

        [Property(DisplayName = "A None Option evaluates as false.")]
        static void OperatorIsTrue_Some(NonNull<string> some)
        {
            bool actual;
            if (Option.From(some.Get))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option evaluates as false.")]
        static void NamedIsFalse_None() => Assert.True(Option<string>.None.IsFalse);

        [Property(DisplayName = "A Some Option does not evaluate as true.")]
        static void NamedIsFalse_Some(NonNull<string> some) => Assert.False(Option.From(some.Get).IsFalse);

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        static void NamedLogicalNot_None() => Assert.True(Option<string>.None.LogicalNot());

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        static void OperatorLogicalNot_None() => Assert.True(!Option<string>.None);

        [Property(DisplayName = "The logical inverse of a Some Option is false.")]
        static void NamedLogicalNot_Some(NonNull<string> some) => Assert.False(Option.From(some.Get).LogicalNot());

        [Property(DisplayName = "The logical inverse of a Some Option is false.")]
        static void OperatorNot_None(NonNull<string> some) => Assert.False(!Option.From(some.Get));

        [Property(DisplayName = "The disjunction of a Some Option and a None Option short-circuits.")]
        static void OperatorLogicalOr_SomeNone_ShortCircuits(
            NonNull<string> some,
            NonNull<string> before,
            NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option<string>.None;
            }

            var dummy = Option.From(some.Get) || Right();

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "The disjunction of two Some Options short-circuits.")]
        static void OperatorLogicalOr_SomeSome_ShortCircuits(
            NonNull<string> some,
            NonNull<string> before,
            NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option.From(some.Get);
            }

            var dummy = Option.From(some.Get) || Right();

            Assert.Equal(before.Get, actual);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        static void OperatorBitwiseAnd_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        static void NamedBitwiseAnd_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        static void OperatorLogicalAnd_NoneNone()
        {
            var left = Option<string>.None;
            var right = Option<string>.None;

            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        static void OperatorBitwiseAnd_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(left, left & right);
            Assert.Equal(left, right & left);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        static void NamedBitwiseAnd_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(left, left.BitwiseAnd(right));
            Assert.Equal(left, right.BitwiseAnd(left));
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        static void OperatorLogicalAnd_NoneSome(NonNull<string> some)
        {
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            Assert.Equal(left, left && right);
            Assert.Equal(left, right && left);
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        static void OperatorBitwiseAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            Assert.Equal(right, left & right);
            Assert.Equal(left, right & left);
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        static void NamedBitwiseAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            Assert.Equal(right, left.BitwiseAnd(right));
            Assert.Equal(left, right.BitwiseAnd(left));
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        static void OperatorLogicalAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            Assert.Equal(right, left && right);
            Assert.Equal(left, right && left);
        }

        [Property(DisplayName = "The conjunction of two None Options short-circuits.")]
        static void OperatorLogicalAnd_NoneNone_ShortCircuits(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option<string>.None;
            }

            var dummy = Option<string>.None && Right();

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option short-circuits.")]
        static void OperatorLogicalAnd_NoneSome_ShortCircuits(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option.From(sentinel.Get);
            }

            var dummy = Option<string>.None && Right();

            Assert.Equal(before.Get, actual);
        }

        [Fact(DisplayName = "The untyped None converts to a None Option.")]
        static void LiteralNone_IsNone()
        {
            Option<string> actual = Option.None;

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Fact(DisplayName = "Null converts to a None Option.")]
        static void Null_IsNone()
        {
            Option<string> actual = null;

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Property(DisplayName = "Values convert to Some Options.")]
        static void Value_IsSome(int some)
        {
            Option<int> actual = some;

            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Unwrapping a None Option throws.")]
        static void Cast_None_Throws()
        {
            var value = Option<string>.None;

            var actual = Record.Exception(() => (string)value);

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);
        }

        [Property(DisplayName = "Unwrapping a Some Option returns its Some value.")]
        static void Cast_Some(NonNull<string> some)
        {
            var actual = (string)Option.From(some.Get);

            Assert.Equal(some.Get, actual);
        }
    }
}
