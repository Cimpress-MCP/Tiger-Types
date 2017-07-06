using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
using static Tiger.Types.Resources;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Union{T1,T2}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) })]
    public sealed class Union2Tests
    {
        #region IsState1, IsState2

        [Property(DisplayName = "A first Union is in the first state.")]
        public void FromFirstBoth_IsFirst_First(NonNull<string> one)
        {
            // arrange, act
            var actual = Union<string, int>.From(one.Get);

            // assert
            Assert.True(actual.IsState1);
        }

        [Property(DisplayName = "A first Union is in the first state.")]
        public void FromFirstOne_IsFirst_First(NonNull<string> one)
        {
            // arrange, act
            var actual = Union.From<string, int>(one.Get);

            // assert
            Assert.True(actual.IsState1);
        }

        [Property(DisplayName = "A second Union is not in the first state.")]
        public void FromSecondBoth_IsFirst_Second(int two)
        {
            // arrange, act
            var actual = Union<string, int>.From(two);

            // assert
            Assert.False(actual.IsState1);
        }

        [Property(DisplayName = "A second Union is not in the first state.")]
        public void FromSecondOne_IsFirst_Second(int two)
        {
            // arrange, act
            var actual = Union.From<string, int>(two);

            // assert
            Assert.False(actual.IsState1);
        }

        [Property(DisplayName = "A first Union is not in the second state.")]
        public void FromFirstBoth_IsSecond_First(NonNull<string> one)
        {
            // arrange, act
            var actual = Union<string, int>.From(one.Get);

            // assert
            Assert.False(actual.IsState2);
        }

        [Property(DisplayName = "A first Union is not in the second state.")]
        public void FromFirstOne_IsSecond_First(NonNull<string> one)
        {
            // arrange, act
            var actual = Union.From<string, int>(one.Get);

            // assert
            Assert.False(actual.IsState2);
        }

        [Property(DisplayName = "A second Union is in the second state.")]
        public void FromSecondBoth_IsSecond_Second(int two)
        {
            // arrange, act
            var actual = Union.From<string, int>(two);

            // assert
            Assert.True(actual.IsState2);
        }

        [Property(DisplayName = "A second Union is in the second state.")]
        public void FromSecondOne_IsSecond_Second(int two)
        {
            // arrange, act
            var actual = Union<string, int>.From(two);

            // assert
            Assert.True(actual.IsState2);
        }

        #endregion

        #region Match

        [Property(DisplayName = "Matching a first Union returns the first func branch, " +
            "not the second func branch.")]
        public void FuncFuncMatchReturn_First(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.Match(
                one: o => o.Length,
                two: t => t);

            // assert
            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, " +
            "not the first func branch.")]
        public void FuncFuncMatchReturn_Second(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.Match(
                one: o => o.Length,
                two: s => s);

            // assert
            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first func branch, " +
            "not the second task branch.")]
        public void FuncTaskMatchReturn_First(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.Match(
                one: o => o.Length,
                two: FromResult).Result;

            // assert
            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second task branch, " +
            "not the first func branch.")]
        public void FuncTaskMatchReturn_Second(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.Match(
                one: o => o.Length,
                two: FromResult).Result;

            // assert
            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first task branch, " +
            "not the second func branch.")]
        public void TaskFuncMatchReturn_First(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: t => t).Result;

            // assert
            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, " +
            "not the first task branch.")]
        public void TaskFuncMatchReturn_Second(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: t => t).Result;

            // assert
            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union returns the first task branch, " +
            "not the second func branch.")]
        public void TaskTaskMatchReturn_First(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: FromResult).Result;

            // assert
            Assert.Equal(one.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a second Union returns the second func branch, " +
            "not the first task branch.")]
        public void TaskTaskMatchReturn_Second(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: FromResult).Result;

            // assert
            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first action branch, " +
            "not the second action branch.")]
        public void ActionActionMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = before;
            value.Match(
                one: o => actual = sentinel,
                two: t => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second action branch, " +
            "not the first action branch.")]
        public void ActionActionMatchVoid_Second(int two, Version before, Version sentinel)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = before;
            value.Match(
                one: o => { },
                two: t => actual = sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first action branch, " +
            "not the second task branch.")]
        public void ActionTaskMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = before;
            value.Match(
                one: o => actual = sentinel,
                two: t => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second task branch, " +
            "not the first action branch.")]
        public void ActionTaskMatchVoid_Second(int two, Version before, Version sentinel)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = before;
            value.Match(
                one: o => { },
                two: t => Run(() => actual = sentinel)).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first task branch, " +
            "not the second action branch.")]
        public void TaskActionMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = before;
            value.Match(
                one: o => Run(() => actual = sentinel),
                two: t => { }).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second action branch, " +
            "not the first task branch.")]
        public void TaskActionMatchVoid_Second(int two, Version before, Version sentinel)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = before;
            value.Match(
                one: o => CompletedTask,
                two: t => actual = sentinel).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a first Union should execute the first task branch, " +
            "not the second task branch.")]
        public void TaskTaskMatchVoid_First(NonNull<string> one, Guid before, Guid sentinel)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = before;
            value.Match(
                one: o => Run(() => actual = sentinel),
                two: t => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a second Union should execute the second task branch, " +
            "not the first task branch.")]
        public void TaskTaskMatchVoid_Second(int two, Version before, Version sentinel)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = before;
            value.Match(
                one: o => CompletedTask,
                two: t => Run(() => actual = sentinel)).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Value

        [Property(DisplayName = "Forcibly first-unwrapping a first Union returns the first value.")]
        public void SecondValue_Second(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.Value1;

            // assert
            Assert.Equal(one.Get, actual);
        }

        [Property(DisplayName = "Forcibly second-unwrapping a first Union throws.")]
        public void SecondValue_First_Throws(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = Record.Exception(() => value.Value2);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(UnionDoesNotMatch, ex.Message);
        }

        [Property(DisplayName = "Forcibly second-unwrapping a second Union returns the second value.")]
        public void Value_Right(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.Value2;

            // assert
            Assert.Equal(two, actual);
        }

        [Property(DisplayName = "Forcibly first-unwrapping a second Union throws.")]
        public void Value_Left_Throws(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = Record.Exception(() => value.Value1);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(UnionDoesNotMatch, ex.Message);
        }

        #endregion

        #region Overrides

        [Property(DisplayName = "A first Union stringifies to One.")]
        public void ToString_Left(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"One({one.Get})", actual);
        }

        [Property(DisplayName = "A second Union stringifies to Two.")]
        public void ToString_Right(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Two({two})", actual);
        }

        [Property(DisplayName = "A first Union is not equal to null.")]
        public void ObjectEquals_LeftNull(NonNull<string> one)
        {
            // arrange
            var left = Union.From<string, int>(one.Get);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "A second Either is not equal to null.")]
        public void ObjectEquals_RightNull(int two)
        {
            // arrange
            var left = Union.From<string, int>(two);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of different type, in different state, with " +
            "different value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_DifferentValue(
            NonNull<string> leftOne,
            NonNull<string> rightTwo)
        {
            // arrange
            var left = Union.From<string, int>(leftOne.Get);
            var right = Union.From<Guid, string>(rightTwo.Get);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of different type, in different state, with same value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_SameValue(NonNull<string> value)
        {
            // arrange
            var left = Union.From<string, int>(value.Get);
            var right = Union.From<bool, string>(value.Get);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of different type, in same state, with different value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_DifferentValue(UnequalNonNullPair<string> values)
        {
            // arrange
            var left = Union.From<string, int>(values.Left);
            var right = Union.From<string, bool>(values.Right);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of different type, in same state, with same value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_SameValue(NonNull<string> one)
        {
            // arrange
            var left = Union.From<string, int>(one.Get);
            var right = Union.From<string, bool>(one.Get);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in different state, with different value are not equal.")]
        public void ObjectEquals_SameType_DifferentState_DifferentValue(NonNull<string> leftOne, int rightTwo)
        {
            // arrange
            var left = Union.From<string, int>(leftOne.Get);
            var right = Union.From<string, int>(rightTwo);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with different value are not equal.")]
        public void ObjectEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values)
        {
            // arrange
            var left = Union.From<string, int>(values.Left);
            var right = Union.From<string, int>(values.Right);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Two Unions of same type, in same state, with same value are equal.")]
        public void ObjectEquals_SameType_SameState_SameValue(NonNull<string> one)
        {
            // arrange
            var left = Union.From<string, int>(one.Get);
            var right = Union.From<string, int>(one.Get);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A first Union should have a hashcode of its first value.")]
        public void GetHashCode_Left(NonNull<string> one)
        {
            // arrange
            var value = Union.From<string, int>(one.Get);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(one.Get.GetHashCode(), actual);
        }

        [Property(DisplayName = "A second Union should have a hashcode of its second value.")]
        public void GetHashCode_Right(int two)
        {
            // arrange
            var value = Union.From<string, int>(two);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(two.GetHashCode(), actual);
        }

        #endregion

        #region Operators and Named Alternatives

        [Property(DisplayName = "Two Unions of same type, in different state, with different value are not equal.")]
        public void OperatorEquals_SameType_DifferentState_DifferentValue(NonNull<string> leftOne, int rightTwo)
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
        public void OperatorEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values)
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
        public void OperatorEquals_SameType_SameState_SameValue(NonNull<string> one)
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
        public void OperatorNotEquals_SameType_DifferentState_DifferentValue(
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
        public void OperatorNotEquals_SameType_SameState_DifferentValue(UnequalNonNullPair<string> values)
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
        public void OperatorNotEquals_SameType_SameState_SameValue(NonNull<string> value)
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
        public void First_IsFirst(Guid one)
        {
            // arrange, act
            Union<Guid, Version> actual = one;

            // assert
            Assert.True(actual.IsState1);
            Assert.False(actual.IsState2);
        }

        [Property(DisplayName = "A value of the second type converts to a second Union.")]
        public void Second_IsSecond(Version two)
        {
            // arrange, act
            Union<Guid, Version> actual = two;

            // assert
            Assert.False(actual.IsState1);
            Assert.True(actual.IsState2);
        }

        [Property(DisplayName = "Second Unwrapping a first Union throws.")]
        public void Cast_First_Throws(Guid one)
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
        public void Cast_Second(Version two)
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
