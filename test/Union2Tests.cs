// ReSharper disable All

using System;
using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="Union{T1,T2}"/>.</summary>
    public sealed class Union2Tests
    {
        const string sentinel = "sentinel";

        #region IsState1, IsState2

        [Fact(DisplayName = "A first Union is in the first state.")]
        public void FromFirstBoth_IsFirst_First()
        {
            // arrange, act
            var actual = Union<string, int>.From(sentinel);

            // assert
            Assert.True(actual.IsState1);
        }

        [Fact(DisplayName = "A first Union is in the first state.")]
        public void FromFirstOne_IsFirst_First()
        {
            // arrange, act
            var actual = Union.From<string, int>(sentinel);

            // assert
            Assert.True(actual.IsState1);
        }

        [Fact(DisplayName = "A second Union is not in the first state.")]
        public void FromSecondBoth_IsFirst_Second()
        {
            // arrange, act
            var actual = Union<string, int>.From(42);

            // assert
            Assert.False(actual.IsState1);
        }

        [Fact(DisplayName = "A second Union is not in the first state.")]
        public void FromSecondOne_IsFirst_Second()
        {
            // arrange, act
            var actual = Union.From<string, int>(42);

            // assert
            Assert.False(actual.IsState1);
        }

        [Fact(DisplayName = "A first Union is not in the second state.")]
        public void FromFirstBoth_IsSecond_First()
        {
            // arrange, act
            var actual = Union<string, int>.From(sentinel);

            // assert
            Assert.False(actual.IsState2);
        }

        [Fact(DisplayName = "A first Union is not in the second state.")]
        public void FromFirstOne_IsSecond_First()
        {
            // arrange, act
            var actual = Union.From<string, int>(sentinel);

            // assert
            Assert.False(actual.IsState2);
        }

        [Fact(DisplayName = "A second Union is in the second state.")]
        public void FromSecondBoth_IsSecond_Second()
        {
            // arrange, act
            var actual = Union.From<string, int>(42);

            // assert
            Assert.True(actual.IsState2);
        }

        [Fact(DisplayName = "A second Union is in the second state.")]
        public void FromSecondOne_IsSecond_Second()
        {
            // arrange, act
            var actual = Union<string, int>.From(42);

            // assert
            Assert.True(actual.IsState2);
        }

        #endregion

        #region Match

        [Fact(DisplayName = "Matching a first Union returns the first func branch, " +
                            "not the second func branch.")]
        public void FuncFuncMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.Match(
                one: o => o.Length,
                two: t => t);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a second Union returns the second func branch, " +
                            "not the first func branch.")]
        public void FuncFuncMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.Match(
                one: o => o,
                two: s => s.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a first Union returns the first func branch, " +
                            "not the second task branch.")]
        public async Task FuncTaskMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length,
                two: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a second Union returns the second task branch, " +
                            "not the first func branch.")]
        public async Task FuncTaskMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o,
                two: t => t.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a first Union returns the first task branch, " +
                            "not the second func branch.")]
        public async Task TaskFuncMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: t => t);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a second Union returns the second func branch, " +
                            "not the first task branch.")]
        public async Task TaskFuncMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: FromResult,
                two: t => t.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a first Union returns the first task branch, " +
                            "not the second func branch.")]
        public async Task TaskTaskMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length.Pipe(FromResult),
                two: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a second Union returns the second func branch, " +
                            "not the first task branch.")]
        public async Task TaskTaskMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: FromResult,
                two: t => t.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a first Union should execute the first action branch, " +
                            "not the second action branch.")]
        public void ActionActionMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                one: o => actual = sentinel,
                two: t => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a second Union should execute the second action branch, " +
                            "not the first action branch.")]
        public void ActionActionMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                one: o => { },
                two: t => actual = sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a first Union should execute the first action branch, " +
                            "not the second task branch.")]
        public async Task ActionTaskMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => actual = sentinel,
                two: t => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a second Union should execute the second task branch, " +
                            "not the first action branch.")]
        public async Task ActionTaskMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => { },
                two: t => Run(() => actual = sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a first Union should execute the first task branch, " +
                            "not the second action branch.")]
        public async Task TaskActionMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Run(() => actual = sentinel),
                two: t => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a second Union should execute the second action branch, " +
                            "not the first task branch.")]
        public async Task TaskActionMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => CompletedTask,
                two: t => actual = sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a first Union should execute the first task branch, " +
                            "not the second task branch.")]
        public async Task TaskTaskMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Run(() => actual = sentinel),
                two: t => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a second Union should execute the second task branch, " +
                            "not the first task branch.")]
        public async Task TaskTaskMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => CompletedTask,
                two: t => Run(() => actual = sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Value

        [Fact(DisplayName = "Forcibly first-unwrapping a first Union returns the first value.")]
        public void SecondValue_Second()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.Value1;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Forcibly second-unwrapping a first Union throws.")]
        public void SecondValue_First_Throws()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = Record.Exception(() => value.Value2);
            
            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.UnionDoesNotMatch, ex.Message);
        }

        [Fact(DisplayName = "Forcibly second-unwrapping a second Union returns the second value.")]
        public void Value_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.Value2;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Forcibly first-unwrapping a second Union throws.")]
        public void Value_Left_Throws()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = Record.Exception(() => value.Value1);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.UnionDoesNotMatch, ex.Message);
        }

        #endregion

        #region Overrides

        [Fact(DisplayName = "A first Union stringifies to One.")]
        public void ToString_Left()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"One({sentinel})", actual);
        }

        [Fact(DisplayName = "A second Union stringifies to Two.")]
        public void ToString_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Two({sentinel})", actual);
        }

        [Fact(DisplayName = "A first Union is not equal to null.")]
        public void ObjectEquals_LeftNull()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A second Either is not equal to null.")]
        public void ObjectEquals_RightNull()
        {
            // arrange
            var left = Union.From<int, string>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of different type, in different state, with " +
                            "different value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<bool, string>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of different type, in different state, with " +
                            "same value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<bool, string>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of different type, in same state, with " +
                            "different value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, bool>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of different type, in same state, with " +
                            "same value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, bool>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in different state, with " +
                            "different value are not equal.")]
        public void ObjectEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "different value are not equal.")]
        public void ObjectEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "same value are equal.")]
        public void ObjectEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A first Union should have a hashcode of its first value.")]
        public void GetHashCode_Left()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        [Fact(DisplayName = "A second Union should have a hashcode of its second value.")]
        public void GetHashCode_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        #endregion

        #region Operators and Named Alternatives

        [Fact(DisplayName = "Two Unions of same type, in different state, with" +
                            "different value are not equal.")]
        public void OperatorEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "different value are not equal.")]
        public void OperatorEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "same value are equal.")]
        public void OperatorEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in different state, with" +
                            "different value are not equal.")]
        public void OperatorNotEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "different value are not equal.")]
        public void OperatorNotEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Unions of same type, in same state, with" +
                            "same value are equal.")]
        public void OperatorNotEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A value of the first type converts to a first Union.")]
        public void First_IsFirst()
        {
            // arrange, act
            Union<string, int> actual = sentinel;

            // assert
            Assert.True(actual.IsState1);
            Assert.False(actual.IsState2);
        }

        [Fact(DisplayName = "A value of the second type converts to a second Union.")]
        public void Second_IsSecond()
        {
            // arrange, act
            Union<int, string> actual = sentinel;

            // assert
            Assert.False(actual.IsState1);
            Assert.True(actual.IsState2);
        }

        [Fact(DisplayName = "Second Unwrapping a first Union throws.")]
        public void Cast_First_Throws()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = Record.Exception(() => (int)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.UnionDoesNotMatch, ex.Message);
        }

        [Fact(DisplayName = "Second Unwrapping a second Union returns its second value.")]
        public void Cast_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion
    }
}
