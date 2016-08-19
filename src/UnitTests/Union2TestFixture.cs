// ReSharper disable All
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using LINQPad;
using Tiger.Types.Properties;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="Union{T1,T2}"/>.</summary>
    [TestFixture(TestOf = typeof(Union<,>))]
    public sealed class Union2TestFixture
    {
        const string sentinel = "sentinel";

        #region IsState1, IsState2

        [Test(Description = "A first Union should be in the first state.")]
        public void FromFirstBoth_IsFirst_First()
        {
            // arrange, act
            var actual = Union<string, int>.From(sentinel);

            // assert
            Assert.That(actual.IsState1, Is.True);
        }

        [Test(Description = "A first Union should be in the first state.")]
        public void FromFirstOne_IsFirst_First()
        {
            // arrange, act
            var actual = Union.From<string, int>(sentinel);

            // assert
            Assert.That(actual.IsState1, Is.True);
        }

        [Test(Description = "A second Union should not be in the first state.")]
        public void FromSecondBoth_IsFirst_Second()
        {
            // arrange, act
            var actual = Union<string, int>.From(42);

            // assert
            Assert.That(actual.IsState1, Is.False);
        }

        [Test(Description = "A second Union should not be in the first state.")]
        public void FromSecondOne_IsFirst_Second()
        {
            // arrange, act
            var actual = Union.From<string, int>(42);

            // assert
            Assert.That(actual.IsState1, Is.False);
        }

        [Test(Description = "A first Union should not be in the second state.")]
        public void FromFirstBoth_IsSecond_First()
        {
            // arrange, act
            var actual = Union<string, int>.From(sentinel);

            // assert
            Assert.That(actual.IsState2, Is.False);
        }

        [Test(Description = "A first Union should not be in the second state.")]
        public void FromFirstOne_IsSecond_First()
        {
            // arrange, act
            var actual = Union.From<string, int>(sentinel);

            // assert
            Assert.That(actual.IsState2, Is.False);
        }

        [Test(Description = "A second Union should be in the second state.")]
        public void FromSecondBoth_IsSecond_Second()
        {
            // arrange, act
            var actual = Union.From<string, int>(42);

            // assert
            Assert.That(actual.IsState2, Is.True);
        }

        [Test(Description = "A second Union should be in the second state.")]
        public void FromSecondOne_IsSecond_Second()
        {
            // arrange, act
            var actual = Union<string, int>.From(42);

            // assert
            Assert.That(actual.IsState2, Is.True);
        }

        #endregion

        #region Match

        [Test(Description = "Matching a first Union should return the first func branch, " +
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
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a second Union should return the second func branch, " +
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
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a first Union should return the first func branch, " +
                            "not the second task branch.")]
        public async Task FuncTaskMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length,
                two: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a second Union should return the second task branch, " +
                            "not the first func branch.")]
        public async Task FuncTaskMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o,
                two: t => t.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a first Union should return the first task branch, " +
                            "not the second func branch.")]
        public async Task TaskFuncMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length.Pipe(Task.FromResult),
                two: t => t);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a second Union should return the second func branch, " +
                            "not the first task branch.")]
        public async Task TaskFuncMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: Task.FromResult,
                two: t => t.Length);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a first Union should return the first task branch, " +
                            "not the second func branch.")]
        public async Task TaskTaskMatchReturn_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = await value.Match(
                one: o => o.Length.Pipe(Task.FromResult),
                two: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a second Union should return the second func branch, " +
                            "not the first task branch.")]
        public async Task TaskTaskMatchReturn_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = await value.Match(
                one: Task.FromResult,
                two: t => t.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a first Union should execute the first action branch, " +
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
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a second Union should execute the second action branch, " +
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
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a first Union should execute the first action branch, " +
                            "not the second task branch.")]
        public async Task ActionTaskMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => actual = sentinel,
                two: t => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a second Union should execute the second task branch, " +
                            "not the first action branch.")]
        public async Task ActionTaskMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => { },
                two: t => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a first Union should execute the first task branch, " +
                            "not the second action branch.")]
        public async Task TaskActionMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Task.Run(() => actual = sentinel),
                two: t => { });

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a second Union should execute the second action branch, " +
                            "not the first task branch.")]
        public async Task TaskActionMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Task.WhenAll(),
                two: t => actual = sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a first Union should execute the first task branch, " +
                            "not the second task branch.")]
        public async Task TaskTaskMatchVoid_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Task.Run(() => actual = sentinel),
                two: t => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a second Union should execute the second task branch, " +
                            "not the first task branch.")]
        public async Task TaskTaskMatchVoid_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                one: o => Task.WhenAll(),
                two: t => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion

        #region Value

        [Test(Description = "Forcibly first-unwrapping a first Union should return the first value.")]
        public void SecondValue_Second()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.Value1;

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Forcibly second-unwrapping a first Union should throw.")]
        public void SecondValue_First_Throws()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = value.Value2);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.UnionDoesNotMatch));
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Forcibly second-unwrapping a second Union should return the second value.")]
        public void Value_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.Value2;

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Forcibly first-unwrapping a second Union should throw.")]
        public void Value_Left_Throws()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = value.Value1);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.UnionDoesNotMatch));
            Assert.That(actual, Is.EqualTo(42));
        }

        #endregion

        #region Overrides

        [Test(Description = "A first Union should stringify to One.")]
        [Category("Override")]
        public void ToString_Left()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo($"One({sentinel})"));
        }

        [Test(Description = "A second Union should stringify to Two.")]
        [Category("Override")]
        public void ToString_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo($"Two({sentinel})"));
        }

        [Test(Description = "A first Union should not be equal to null.")]
        [Category("Override")]
        public void ObjectEquals_LeftNull()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A second Either should not be equal to null.")]
        [Category("Override")]
        public void ObjectEquals_RightNull()
        {
            // arrange
            var left = Union.From<int, string>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of different type, in different state, with " +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<bool, string>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of different type, in different state, with " +
                            "same value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_DifferentState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<bool, string>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of different type, in same state, with " +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, bool>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of different type, in same state, with " +
                            "same value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, bool>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of same type, in different state, with " +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A first Union should have a hashcode of its first value.")]
        [Category("Override")]
        public void GetHashCode_Left()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.GetHashCode()));
        }

        [Test(Description = "A second Union should have a hashcode of its second value.")]
        [Category("Override")]
        public void GetHashCode_Right()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.GetHashCode()));
        }

        #endregion

        #region Implementations

        [Test(Description = "A first Union should interact with LINQPad correctly.")]
        public void CustomMemberProvider_First()
        {
            // arrange
            var value = Union.From<string, int>(sentinel) as ICustomMemberProvider;

            // act, assert
            Assert.That(value.GetNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(value.GetTypes(), Is.EquivalentTo(new[] { typeof(string) }));
            Assert.That(value.GetValues(), Is.EquivalentTo(new[] { Union.From<string, int>(sentinel).ToString() }));
        }

        [Test(Description = "A second Union should interact with LINQPad correctly.")]
        public void CustomMemberProvider_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel) as ICustomMemberProvider;

            // act, assert
            Assert.That(value.GetNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(value.GetTypes(), Is.EquivalentTo(new[] { typeof(string) }));
            Assert.That(value.GetValues(), Is.EquivalentTo(new[] { Union.From<int, string>(sentinel).ToString() }));
        }

        #endregion

        #region Operators and Named Alternatives

        [Test(Description = "Two Unions of same type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Unions of same type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(42);

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>("megatron");

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Unions of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Union.From<string, int>(sentinel);
            var right = Union.From<string, int>(sentinel);

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A value of the first type should convert to a first Union.")]
        public void First_IsFirst()
        {
            // arrange, act
            Union<string, int> actual = sentinel;

            // assert
            Assert.That(actual.IsState1, Is.True);
            Assert.That(actual.IsState2, Is.False);
        }

        [Test(Description = "A value of the second type should convert to a second Union.")]
        public void Second_IsSecond()
        {
            // arrange, act
            Union<int, string> actual = sentinel;

            // assert
            Assert.That(actual.IsState1, Is.False);
            Assert.That(actual.IsState2, Is.True);
        }

        [Test(Description = "Second Unwrapping a first Union should throw.")]
        public void Cast_First_Throws()
        {
            // arrange
            var value = Union.From<string, int>(sentinel);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = (int)value);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.UnionDoesNotMatch));
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Second Unwrapping a second Union should return its second value.")]
        public void Cast_Second()
        {
            // arrange
            var value = Union.From<int, string>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion
    }
}
