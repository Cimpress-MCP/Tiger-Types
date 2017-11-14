using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Threading.Tasks.Task;
using static Tiger.Types.Resources;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Union{T1,T2}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) })]
    public static class Union2Tests
    {
        #region IsState1, IsState2

        [Property(DisplayName = "A first Union is in the first state.")]
        static void FromFirstBoth_IsFirst_First(NonNull<string> one) =>
            Assert.True(Union<string, int>.From(one.Get).IsState1);

        [Property(DisplayName = "A first Union is in the first state.")]
        static void FromFirstOne_IsFirst_First(NonNull<string> one) =>
            Assert.True(Union.From<string, int>(one.Get).IsState1);

        [Property(DisplayName = "A second Union is not in the first state.")]
        static void FromSecondBoth_IsFirst_Second(int two) => Assert.False(Union<string, int>.From(two).IsState1);

        [Property(DisplayName = "A second Union is not in the first state.")]
        static void FromSecondOne_IsFirst_Second(int two) => Assert.False(Union.From<string, int>(two).IsState1);

        [Property(DisplayName = "A first Union is not in the second state.")]
        static void FromFirstBoth_IsSecond_First(NonNull<string> one) =>
            Assert.False(Union<string, int>.From(one.Get).IsState2);

        [Property(DisplayName = "A first Union is not in the second state.")]
        static void FromFirstOne_IsSecond_First(NonNull<string> one) =>
            Assert.False(Union.From<string, int>(one.Get).IsState2);

        [Property(DisplayName = "A second Union is in the second state.")]
        static void FromSecondBoth_IsSecond_Second(int two) => Assert.True(Union.From<string, int>(two).IsState2);

        [Property(DisplayName = "A second Union is in the second state.")]
        static void FromSecondOne_IsSecond_Second(int two) => Assert.True(Union<string, int>.From(two).IsState2);

        #endregion

        #region Match

        [Property(DisplayName = "Matching a first Union returns the first func branch, not the second func branch.")]
        static void FuncFuncMatchReturn_First(NonNull<string> one)
        {
            var actual = Union.From<string, int>(one.Get).Match(
                one: o => o.Length,
                two: t => t);

            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, not the first func branch.")]
        static void FuncFuncMatchReturn_Second(int two)
        {
            var actual = Union.From<string, int>(two).Match(
                one: o => o.Length,
                two: s => s);

            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first func branch, not the second task branch.")]
        static async Task FuncTaskMatchReturn_First(NonNull<string> one)
        {
            var actual = await Union.From<string, int>(one.Get).Match(
                one: o => o.Length,
                two: FromResult);

            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second task branch, not the first func branch.")]
        static async Task FuncTaskMatchReturn_Second(int two)
        {
            var actual = await Union.From<string, int>(two).Match(
                one: o => o.Length,
                two: FromResult);

            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first task branch, not the second func branch.")]
        static async Task TaskFuncMatchReturn_First(NonNull<string> one)
        {
            var actual = await Union.From<string, int>(one.Get).Match(
                one: o => o.Length.Pipe(FromResult),
                two: t => t);

            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, not the first task branch.")]
        static async Task TaskFuncMatchReturn_Second(int two)
        {
            var actual = await Union.From<string, int>(two).Match(
                one: o => o.Length.Pipe(FromResult),
                two: t => t);

            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first task branch, not the second func branch.")]
        static async Task TaskTaskMatchReturn_First(NonNull<string> one)
        {
            var actual = await Union.From<string, int>(one.Get).Match(
                one: o => o.Length.Pipe(FromResult),
                two: FromResult);

            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, not the first task branch.")]
        static async Task TaskTaskMatchReturn_Second(int two)
        {
            var actual = await Union.From<string, int>(two).Match(
                one: o => o.Length.Pipe(FromResult),
                two: FromResult);

            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first action branch, not the second action branch.")]
        static void ActionActionMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            var actual = before;
            var unit = Union.From<string, int>(one.Get).Match(
                one: o => actual = sentinel,
                two: t => { });

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second action branch, not the first action branch.")]
        static void ActionActionMatchVoid_Second(int two, Version before, Version sentinel)
        {
            var actual = before;
            var unit = Union.From<string, int>(two).Match(
                one: o => { },
                two: t => actual = sentinel);

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first action branch, not the second task branch.")]
        static async Task ActionTaskMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            var actual = before;
            await Union.From<string, int>(one.Get).Match(
                one: o => actual = sentinel,
                two: t => CompletedTask);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second task branch, not the first action branch.")]
        static async Task ActionTaskMatchVoid_Second(int two, Version before, Version sentinel)
        {
            var actual = before;
            await Union.From<string, int>(two).Match(
                one: o => { },
                two: t => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first task branch, not the second action branch.")]
        static async Task TaskActionMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            var actual = before;
            await Union.From<string, int>(one.Get).Match(
                one: o => Run(() => actual = sentinel),
                two: t => { });

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second action branch, not the first task branch.")]
        static async Task TaskActionMatchVoid_Second(int two, Version before, Version sentinel)
        {
            var actual = before;
            await Union.From<string, int>(two).Match(
                one: o => CompletedTask,
                two: t => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first task branch, not the second task branch.")]
        static async Task TaskTaskMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            var actual = before;
            await Union.From<string, int>(one.Get).Match(
                one: o => Run(() => actual = sentinel),
                two: t => CompletedTask);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second task branch, not the first task branch.")]
        static async Task TaskTaskMatchVoid_Second(int two, Version before, Version sentinel)
        {
            var actual = before;
            await Union.From<string, int>(two).Match(
                one: o => CompletedTask,
                two: t => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Value

        [Property(DisplayName = "Forcibly first-unwrapping a first Union returns the first value.")]
        static void SecondValue_Second(NonNull<string> one) =>
            Assert.Equal(one.Get, Union.From<string, int>(one.Get).Value1);

        [Property(DisplayName = "Forcibly second-unwrapping a first Union throws.")]
        static void SecondValue_First_Throws(NonNull<string> one)
        {
            var actual = Record.Exception(() => Union.From<string, int>(one.Get).Value2);

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(UnionDoesNotMatch, ex.Message);
        }

        [Property(DisplayName = "Forcibly second-unwrapping a second Union returns the second value.")]
        static void Value_Right(int two) => Assert.Equal(two, Union.From<string, int>(two).Value2);

        [Property(DisplayName = "Forcibly first-unwrapping a second Union throws.")]
        static void Value_Left_Throws(int two)
        {
            var actual = Record.Exception(() => Union.From<string, int>(two).Value1);

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(UnionDoesNotMatch, ex.Message);
        }

        #endregion

        #region Overrides

        [Property(DisplayName = "A first Union stringifies to One.")]
        static void ToString_Left(NonNull<string> one) =>
            Assert.Equal($"One({one.Get})", Union.From<string, int>(one.Get).ToString());

        [Property(DisplayName = "A second Union stringifies to Two.")]
        static void ToString_Right(int two) => Assert.Equal($"Two({two})", Union.From<string, int>(two).ToString());

        [Property(DisplayName = "A first Union is not equal to null.")]
        static void ObjectEquals_LeftNull(NonNull<string> one) =>
            Assert.False(Union.From<string, int>(one.Get).Equals(null));

        [Property(DisplayName = "A second Either is not equal to null.")]
        static void ObjectEquals_RightNull(int two) => Assert.False(Union.From<string, int>(two).Equals(null));

        [Property(DisplayName = "Two Unions of different type, in different state, with different value are not equal.")]
        static void ObjectEquals_DifferentType_DifferentState_DifferentValue(
            NonNull<string> leftOne,
            NonNull<string> rightTwo) =>
            Assert.False(Union.From<string, int>(leftOne.Get).Equals(Union.From<Guid, string>(rightTwo.Get)));

        [Property(DisplayName = "Two Unions of different type, in different state, with same value are not equal.")]
        static void ObjectEquals_DifferentType_DifferentState_SameValue(NonNull<string> value) =>
            Assert.False(Union.From<string, int>(value.Get).Equals(Union.From<bool, string>(value.Get)));

        [Property(DisplayName = "Two Unions of different type, in same state, with different value are not equal.")]
        static void ObjectEquals_DifferentType_SameState_DifferentValue(UnequalNonNullPair<string> values) =>
            Assert.False(Union.From<string, int>(values.Left).Equals(Union.From<string, bool>(values.Right)));

        [Property(DisplayName = "Two Unions of different type, in same state, with same value are not equal.")]
        static void ObjectEquals_DifferentType_SameState_SameValue(NonNull<string> one) =>
            Assert.False(Union.From<string, int>(one.Get).Equals(Union.From<string, bool>(one.Get)));

        [Property(DisplayName = "Two Unions of same type, in different state, with different value are not equal.")]
        static void ObjectEquals_SameType_DifferentState_DifferentValue(NonNull<string> leftOne, int rightTwo) =>
            Assert.False(Union.From<string, int>(leftOne.Get).Equals(Union.From<string, int>(rightTwo)));

        [Property(DisplayName = "Two Unions of same type, in same state, with different value are not equal.")]
        static void ObjectEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values) =>
            Assert.False(Union.From<string, int>(values.Left).Equals(Union.From<string, int>(values.Right)));

        [Property(DisplayName = "Two Unions of same type, in same state, with same value are equal.")]
        static void ObjectEquals_SameType_SameState_SameValue(NonNull<string> one) =>
            Assert.True(Union.From<string, int>(one.Get).Equals(Union.From<string, int>(one.Get)));

        [Property(DisplayName = "A first Union should have a hashcode of its first value.")]
        static void GetHashCode_Left(NonNull<string> one) =>
            Assert.Equal(one.Get.GetHashCode(), Union.From<string, int>(one.Get).GetHashCode());

        [Property(DisplayName = "A second Union should have a hashcode of its second value.")]
        static void GetHashCode_Right(int two) => Assert.Equal(two.GetHashCode(), Union.From<string, int>(two).GetHashCode());

        #endregion

        #region Operators and Named Alternatives

        [Property(DisplayName = "Two Unions of same type, in different state, with different value are not equal.")]
        static void OperatorEquals_SameType_DifferentState_DifferentValue(NonNull<string> leftOne, int rightTwo)
        {
            // arrange
            var left = Union.From<string, int>(leftOne.Get);
            var right = Union.From<string, int>(rightTwo);

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with different value are not equal.")]
        static void OperatorEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values)
        {
            // arrange
            var left = Union.From<string, int>(values.Left);
            var right = Union.From<string, int>(values.Right);

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with same value are equal.")]
        static void OperatorEquals_SameType_SameState_SameValue(NonNull<string> one)
        {
            // arrange
            var left = Union.From<string, int>(one.Get);
            var right = Union.From<string, int>(one.Get);

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in different state, with different value are unequal.")]
        static void OperatorNotEquals_SameType_DifferentState_DifferentValue(
            NonNull<string> leftOne,
            int rightTwo)
        {
            // arrange
            var left = Union.From<string, int>(leftOne.Get);
            var right = Union.From<string, int>(rightTwo);

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with different value are unequal.")]
        static void OperatorNotEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values)
        {
            // arrange
            var left = Union.From<string, int>(values.Left);
            var right = Union.From<string, int>(values.Right);

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with same value are not unequal.")]
        static void OperatorNotEquals_SameType_SameState_SameValue(NonNull<string> value)
        {
            // arrange
            var left = Union.From<string, int>(value.Get);
            var right = Union.From<string, int>(value.Get);

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "A value of the first type converts to a first Union.")]
        static void First_IsFirst(Guid one)
        {
            // arrange, act
            Union<Guid, Version> actual = one;

            // assert
            Assert.True(actual.IsState1);
            Assert.False(actual.IsState2);
        }

        [Property(DisplayName = "A value of the second type converts to a second Union.")]
        static void Second_IsSecond(Version two)
        {
            // arrange, act
            Union<Guid, Version> actual = two;

            // assert
            Assert.False(actual.IsState1);
            Assert.True(actual.IsState2);
        }

        [Property(DisplayName = "Second Unwrapping a first Union throws.")]
        static void Cast_First_Throws(Guid one)
        {
            // arrange
            var value = Union.From<Guid, Version>(one);

            // act
            var actual = Record.Exception(() => (Version)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(UnionDoesNotMatch, ex.Message);
        }

        [Property(DisplayName = "Second Unwrapping a second Union returns its second value.")]
        static void Cast_Second(Version two)
        {
            // arrange
            var value = Union.From<Guid, Version>(two);

            // act
            var actual = (Version)value;

            // assert
            Assert.Equal(two, actual);
        }

        #endregion
    }
}
