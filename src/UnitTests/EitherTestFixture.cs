// ReSharper disable All

using System;
using NUnit.Framework;
using System.Threading.Tasks;
using LINQPad;
using Tiger.Types.Properties;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [TestFixture(TestOf = typeof(Either<,>))]
    public sealed class EitherTestFixture
    {
        const string sentinel = "sentinel";

        #region IsLeft, IsRight

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftBoth_IsLeft_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftOne_IsLeft_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Right Either should not be in the Left state.")]
        public void FromRightBoth_IsLeft_Right()
        {
            // arrange, act
            var actual = Either<string, int>.FromRight(42);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Right Either should not be in the Left state.")]
        public void FromRightOne_IsLeft_Right()
        {
            // arrange, act
            var actual = Either.Right<int, string>(sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Bottom Either should not be in the Left state.")]
        public void Default_IsLeft_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Left Either should not be in the Right state.")]
        public void FromLeftBoth_IsRight_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Left Either should not be in the Right state.")]
        public void FromLeftOne_IsRight_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromRightBoth_IsRight_Right()
        {
            // arrange, act
            var actual = Either<int, string>.FromRight(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.True);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromRightOne_IsRight_Right()
        {
            // arrange, act
            var actual = Either.Right<int, string>(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.True);
        }

        [Test(Description = "A Bottom Either should not be in the Right state.")]
        public void Default_IsRight_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        #endregion

        #region Match

        [Test(Description = "Matching a Left Either should return the Left func branch, " +
                            "not the Right func branch.")]
        public void FuncFuncMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left func branch.")]
        public void FuncFuncMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Match(
                left: l => l,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left func branch, " +
                            "not the Right task branch.")]
        public async Task FuncTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right task branch, " +
                            "not the Left func branch.")]
        public async Task FuncTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                    "not the Right func branch.")]
        public async Task TaskFuncMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskFuncMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                            "not the Right func branch.")]
        public async Task TaskTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right action branch.")]
        public void ActionActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => actual = sentinel,
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right v branch, " +
                            "not the Left v branch.")]
        public void ActionActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => { },
                right: r => actual = sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right task branch.")]
        public async Task ActionTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => actual = sentinel,
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left action branch.")]
        public async Task ActionTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => { },
                right: r => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right action branch.")]
        public async Task TaskActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = sentinel),
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right action branch, " +
                            "not the Left task branch.")]
        public async Task TaskActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => actual = sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right task branch.")]
        public async Task TaskTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = sentinel),
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion

        #region Map

        [Test(Description = "Left-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(left: _ => sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(left: _ => false);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(left: _ => Task.FromResult(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(left: _ => Task.FromResult(false));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Map(right: _ => 42);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");
            Assert.True(value.IsRight);

            // act
            var actual = value.Map(right: _ => sentinel);
            Assert.IsTrue(actual.IsRight);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Map(right: _ => Task.FromResult(42));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");

            // act
            var actual = await value.Map(right: _ => Task.FromResult(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public void FuncFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(
                left: _ => sentinel,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public void FuncFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(
                left: _ => false,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public async Task FuncTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(sentinel),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task FuncTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => sentinel,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public async Task TaskFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => false,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(sentinel),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        #endregion

        #region Bind

        [Test(Description = "Right-Binding a Left Either over a func should return a Left Either.")]
        public void FuncBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(42)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public void FuncBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(33)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, bool>(true)));
        }

        [Test(Description = "Left-Binding a Right Either over a func should return a Right Either.")]
        public void FuncBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, int>(42)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42 ? Either.Right<bool, string>(sentinel) : Either.Left<bool, string>(true));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public void FuncBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42 ? Either.Left<bool, string>(true) : Either.Right<bool, string>(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<bool, string>(true)));
        }

        [Test(Description = "Right-Binding a Left Either over a task should return a Left Either.")]
        public async Task TaskBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(42)));
        }

        [Test(Description = "Right-Binding a Right Either over a task that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(33)));
        }

        [Test(Description = "Right-Binding a Right Either over a task that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, bool>(true)));
        }

        [Test(Description = "Left-Binding a Right Either over a task should return a Right Either.")]
        public async Task TaskBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, int>(42)));
        }

        [Test(Description = "Left-Binding a Left Either over a task that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == 42
                ? Either.Right<bool, string>(sentinel)
                : Either.Left<bool, string>(true)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Binding a Left Either over a task that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == 42
                ? Either.Left<bool, string>(true)
                : Either.Right<bool, string>(sentinel)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<bool, string>(true)));
        }

        [Test(Description = "Bi-Binding a Right Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncFuncBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(false)));
        }

        [Test(Description = "Bi-Binding a Right Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public void FuncFuncBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(99L)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncFuncBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(true)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public void FuncFuncBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(33L)));
        }

        [Test(Description = "Bi-Binding a Right Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskFuncBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(false)));
        }

        [Test(Description = "Bi-Binding a Right Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskFuncBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(99L)));
        }

        [Test(Description = "Bi-Binding a Left Either over a task that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskFuncBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(true)));
        }

        [Test(Description = "Bi-Binding a Left Either over a task that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskFuncBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(33L)));
        }

        [Test(Description = "Bi-Binding a Right Either over a task that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task FuncTaskBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(false)));
        }

        [Test(Description = "Bi-Binding a Right Either over a task that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task FuncTaskBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(99L)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task FuncTaskBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(true)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task FuncTaskBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(33L)));
        }

        [Test(Description = "Bi-Binding a Right Either over a task that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskTaskBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(false)));
        }

        [Test(Description = "Bi-Binding a Right Either over a task that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskTaskBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(99L)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskTaskBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<long, bool>(true)));
        }

        [Test(Description = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskTaskBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => Task.FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => Task.FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<long, bool>(33L)));
        }

        #endregion

        #region Fold

        [Test(Description = "Right-Folding over a Left Either should return the seed value.")]
        public void FuncFoldRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(34));
        }

        [Test(Description = "Right-Folding over a Right Either should return result of invoking the accumulator" +
                            "over the seed value and the Right value.")]
        public void FuncFoldRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Left-Folding over a Right Either should return the seed value.")]
        public void FuncFoldLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(34));
        }

        [Test(Description = "Left-Folding over a Left Either should return result of invoking the accumulator" +
                            "over the seed value and the Left value.")]
        public void FuncFoldLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Right-Folding over a Left Either should return the seed value.")]
        public async Task TaskFoldRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Fold(34, (s, v) => Task.FromResult(s + v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(34));
        }

        [Test(Description = "Right-Folding over a Right Either should return result of invoking the accumulator" +
                            "over the seed value and the Right value.")]
        public async Task TaskFoldRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Fold(34, (s, v) => Task.FromResult(s + v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Left-Folding over a Right Either should return the seed value.")]
        public async Task TaskFoldLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Fold(34, (s, v) => Task.FromResult(s + v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(34));
        }

        [Test(Description = "Left-Folding over a Left Either should return result of invoking the accumulator" +
                            "over the seed value and the Left value.")]
        public async Task TaskFoldLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Fold(34, (s, v) => Task.FromResult(s + v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        #endregion

        #region Tap

        [Test(Description = "Left-tapping a Left Either over a func should return a Left Either " +
                            "and perform an action")]
        public void FuncTapLeft_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = 33;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, string>(42)));
            Assert.That(output, Is.EqualTo(42));
        }

        [Test(Description = "Left-tapping a Right Either over a func should return a Right Either " +
                            "and perform no action.")]
        public void FuncTapLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = sentinel;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
            Assert.That(output, Is.EqualTo(sentinel));
        }

        [Test(Description = "Right-tapping a Left Either over a func should return a Left Either " +
                            "and perform no action.")]
        public void FuncTapRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = sentinel;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, string>(42)));
            Assert.That(output, Is.EqualTo(sentinel));
        }

        [Test(Description = "Right-tapping a Right Either over a func should return a Right Either " +
                            "and perform an action.")]
        public void FuncTapRight_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = 33;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
            Assert.That(output, Is.EqualTo(42));
        }

        [Test(Description = "Left-tapping a Left Either over a task should return a Left Either " +
                            "and perform an action")]
        public async Task TaskTapLeft_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = 33;
            var actual = await value.Tap(v => Task.Run(() => output = v));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, string>(42)));
            Assert.That(output, Is.EqualTo(42));
        }

        [Test(Description = "Left-tapping a Right Either over a task should return a Right Either " +
                            "and perform no action.")]
        public async Task TaskTapLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = sentinel;
            var actual = await value.Tap(v => Task.Run(() => output = v));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
            Assert.That(output, Is.EqualTo(sentinel));
        }

        [Test(Description = "Right-tapping a Left Either over a task should return a Left Either " +
                            "and perform no action.")]
        public async Task TaskTapRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = sentinel;
            var actual = await value.Tap(v => Task.Run(() => output = v));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, string>(42)));
            Assert.That(output, Is.EqualTo(sentinel));
        }

        [Test(Description = "Right-tapping a Right Either over a task should return a Right Either " +
                            "and perform an action.")]
        public async Task TaskTapRight_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = 33;
            var actual = await value.Tap(v => Task.Run(() => output = v));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
            Assert.That(output, Is.EqualTo(42));
        }

        #endregion

        #region Let

        [Test(Description = "Left-conditionally executing an action based on a Left Either " +
                            "should execute.")]
        public void ActionLetLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = "megatron";
            value.Let(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Left-conditionally executing an action based on a Right Either " +
                            "should not execute.")]
        public void ActionLetLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = 42;
            value.Let(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Right-conditionally executing an action based on a Left Either " +
                            "should not execute.")]
        public void ActionLetRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            value.Let(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Right-conditionally executing an action based on a Right Either " +
                            "should execute.")]
        public void ActionLetRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            value.Let(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Left-conditionally executing a task based on a Left Either " +
                            "should execute.")]
        public async Task TaskLetLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = "megatron";
            await value.Let(v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Left-conditionally executing a Task based on a Right Either " +
                            "should not execute.")]
        public async Task TaskLetLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = 42;
            await value.Let(v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Right-conditionally executing a Task based on a Left Either " +
                            "should not execute.")]
        public async Task TaskLetRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            await value.Let(v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Right-conditionally executing a Task based on a Right Either " +
                            "should execute.")]
        public async Task TaskLetRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            await value.Let(v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion

        #region Recover

        [Test(Description = "Recovering a Left Option should return the recovery value.")]
        public void ValueRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Recover(42);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
        }

        [Test(Description = "Recovering a Right Option should return the original value.")]
        public void ValueRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover("megatron");

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Recovering a Left Option should return the recovery value.")]
        public void FuncRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Recover(() => 42);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
        }

        [Test(Description = "Recovering a Right Option should return the original value.")]
        public void FuncRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover(() => "megatron");

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Recovering a Left Option should return the recovery value.")]
        public async Task TaskRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Recover(() => Task.FromResult(42));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(42)));
        }

        [Test(Description = "Recovering a Right Option should return the original value.")]
        public async Task TaskRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Recover(() => Task.FromResult("megatron"));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        #endregion

        #region Value

        [Test(Description = "Forcibly unwrapping a Left Either should throw.")]
        public void Value_Left_Throws()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = value.Value);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.EitherIsNotRight));
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Forcibly unwrapping a Right Either should return the Right value.")]
        public void Value_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Value;

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Left Either with an alternative value " +
                            "should return the alternative value.")]
        public void GetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.That(actual, Is.EqualTo(default(string)));
        }

        [Test(Description = "Coalescing a Right Either with an alternative value " +
                            "should return the Right value.")]
        public void GetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Left Either with an alternative value " +
                    "should return the alternative value.")]
        public void ValueGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault(sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Right Either with an alternative value " +
                            "should return the Right value.")]
        public void ValueGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault("megatron");

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Left Either with an alternative value " +
            "should return the alternative value.")]
        public void FuncGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault(() => sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Right Either with an alternative value " +
                            "should return the Right value.")]
        public void FuncGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault(() => "megatron");

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Left Either with an alternative value " +
                            "should return the alternative value.")]
        public async Task TaskGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.GetValueOrDefault(() => Task.FromResult(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Coalescing a Right Either with an alternative value " +
                            "should return the Right value.")]
        public async Task TaskGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.GetValueOrDefault(() => Task.FromResult("megatron"));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion

        #region Overrides

        [Test(Description = "A Left Either should stringify to Left.")]
        [Category("Override")]
        public void ToString_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo($"Left({sentinel})"));
        }

        [Test(Description = "A Right Either should stringify to Right.")]
        [Category("Override")]
        public void ToString_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo($"Right({sentinel})"));
        }

        [Test(Description = "A Bottom Either should stringify to Bottom.")]
        [Category("Override")]
        public void ToString_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo("Bottom"));
        }

        [Test(Description = "A Left Either should not be equal to null.")]
        [Category("Override")]
        public void ObjectEquals_LeftNull()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A Right Either should not be equal to null.")]
        [Category("Override")]
        public void ObjectEquals_RightNull()
        {
            // arrange
            var left = Either.Right<int, string>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A Bottom Either should not be equal to null.")]
        [Category("Override")]
        public void ObjectEquals_BottomNull()
        {
            // arrange
            var left = default(Either<string, int>);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of different type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<bool, string>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of different type, in different state, with" +
                            "same value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_DifferentState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<bool, string>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of different type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, bool>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of different type, in same state, with" +
                            "same value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_DifferentType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, bool>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of same type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Override")]
        public void ObjectEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Bottom Eithers should be equal.")]
        [Category("Override")]
        public void ObjectEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A Left Either should have a hashcode of its Left value.")]
        [Category("Override")]
        public void GetHashCode_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.GetHashCode()));
        }

        [Test(Description = "A Right Either should have a hashcode of its Right value.")]
        [Category("Override")]
        public void GetHashCode_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.GetHashCode()));
        }

        [Test(Description = "A Bottom Either should have a hashcode of 0.")]
        [Category("Override")]
        public void GetHashCode_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        #endregion

        #region Implementations

        [Test(Description = "A Left Either should not enumerate.")]
        [Category("Implementation")]
        public void GetEnumerator_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "A Right Either should enumerate.")]
        [Category("Implementation")]
        public void GetEnumerator_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = 0;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "A Bottom Either should not enumerate.")]
        [Category("Implementation")]
        public void GetEnumerator_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = 42;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "A Left Either should interact with LINQPad correctly.")]
        public void CustomMemberProvider_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel) as ICustomMemberProvider;

            // act, assert
            Assert.That(value.GetNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(value.GetTypes(), Is.EquivalentTo(new[] { typeof(string) }));
            Assert.That(value.GetValues(), Is.EquivalentTo(new[] { Either.Left<string, int>(sentinel).ToString() }));
        }

        [Test(Description = "A Right Either should interact with LINQPad correctly.")]
        public void CustomMemberProvider_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel) as ICustomMemberProvider;

            // act, assert
            Assert.That(value.GetNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(value.GetTypes(), Is.EquivalentTo(new[] { typeof(string) }));
            Assert.That(value.GetValues(), Is.EquivalentTo(new[] { Either.Right<int, string>(sentinel).ToString() }));
        }

        [Test(Description = "A Bottom Either should interact with LINQPad correctly.")]
        public void CustomMemberProvider_Bottom()
        {
            // arrange
            var value = default(Either<string, int>) as ICustomMemberProvider;

            // act, assert
            Assert.That(value.GetNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(value.GetTypes(), Is.EquivalentTo(new[] { typeof(string) }));
            Assert.That(value.GetValues(), Is.EquivalentTo(new[] { default(Either<string, int>).ToString() }));
        }

        #endregion

        #region Operators and Named Alternatives

        [Test(Description = "Two Eithers of same type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.False);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Bottom Eithers should be equal.")]
        [Category("Operator")]
        public void OperatorEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Eithers of same type, in different state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.True);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "different value should not be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two Eithers of same type, in same state, with" +
                            "same value should be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two Bottom Eithers should be equal.")]
        [Category("Operator")]
        public void OperatorNotEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A value of the Left type should convert to a Left Either.")]
        public void Left_IsLeft()
        {
            // arrange, act
            Either<string, int> actual = sentinel;

            // assert
            Assert.That(actual.IsLeft, Is.True);
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A value of the Right type should convert to a Right Either.")]
        public void Right_IsRight()
        {
            // arrange, act
            Either<int, string> actual = sentinel;

            // assert
            Assert.That(actual.IsLeft, Is.False);
            Assert.That(actual.IsRight, Is.True);
        }

        [Test(Description = "Null should convert to a Bottom Either.")]
        public void Bottom_IsBottom()
        {
            // arrange, act
            Either<int, string> actual = null;

            // assert
            Assert.That(actual.IsLeft, Is.False);
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "Unwrapping a Left Either should throw.")]
        public void Cast_Left_Throws()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = (int)value);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.EitherIsNotRight));
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Unwrapping a Right Either should return its Right value.")]
        public void Cast_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Unwrapping a Bottom Either should throw.")]
        public void Cast_Bottom_Throwws()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = 42;
            var ex = Assert.Throws<InvalidOperationException>(() => actual = (int)value);

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.EitherIsNotRight));
            Assert.That(actual, Is.EqualTo(42));
        }

        #endregion

        #region Extensions

        #region LINQ

        [Test(Description = "Asking a Left Either for any should return false.")]
        [Category("Extension")]
        public void Any_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either for any should return true.")]
        [Category("Extension")]
        public void Any_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Left Either for any with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Left Either for any with a true predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either for any with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either for any with a true predicate should return true.")]
        [Category("Extension")]
        public void PredicateAny_RightTrue()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Left Either for all with a false predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Left Either for all with a true predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Right Either for all with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAll_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either for all with a true predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_RightTrue()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.All(v => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Left Either whether it contains a value should return false.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        [Category("Extension")]
        public void Contains_Left(int testValue)
        {
            // arrange
            var value = Either.Left<int, string>(testValue);

            // act
            var actual = value.Contains(sentinel);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either whether it contains a value that it doesn't " +
                            "should return false.")]
        [Category("Extension")]
        public void Contains_Right_False()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains("megatron");

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either whether it contains a value that it does " +
                            "should return true.")]
        [Category("Extension")]
        public void Contains_Right_True()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains(sentinel);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Left Either whether it contains a value should return false.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        [Category("Extension")]
        public void ComparerContains_Left(int testValue)
        {
            // arrange
            var value = Either.Left<int, string>(testValue);

            // act
            var actual = value.Contains(sentinel, StringComparer.Ordinal);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either whether it contains a value that it doesn't " +
                            "should return false.")]
        [Category("Extension")]
        public void ComparerContains_Right_False()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains("megatron", StringComparer.Ordinal);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Right Either whether it contains a value that it does " +
                            "should return true.")]
        [Category("Extension")]
        public void ComparerContains_Right_True()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains(sentinel, StringComparer.Ordinal);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Recovering a Left Either should return the recovery value.")]
        [Category("Extension")]
        public void DefaultIfEmpty_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(0)));
        }

        [Test(Description = "Recovering a Right Either should return the Right value.")]
        [Category("Extension")]
        public void DefaultIfEmpty_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover("megatron");

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Recovering a Bottom Either should return the recovery value.")]
        [Category("Extension")]
        public void DefaultIfEmpty_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(0)));
        }

        [Test(Description = "Tapping a Left Either over a func should return a Left Either " +
                            "and perform no action.")]
        [Category("Extension")]
        public void Do_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var output = 42;
            var actual = value.Do(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
            Assert.That(output, Is.EqualTo(42));
        }

        [Test(Description = "Tapping a Right Either over a func should return a Right Either " +
                            "and perform an action.")]
        [Category("Extension")]
        public void Do_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var output = "megatron";
            var actual = value.Do(v => output = v);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
            Assert.That(output, Is.EqualTo(sentinel));
        }

        [Test(Description = "Conditionally executing an action based on a Left Either " +
                            "should not execute.")]
        [Category("Extension")]
        public void ForEach_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            value.ForEach(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Conditionally executing an action based on a Right Either " +
                            "should execute.")]
        [Category("Extension")]
        public void ForEach_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            value.ForEach(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Selecting a Left Either should produce a Left Either.")]
        [Category("Extension")]
        public void Select_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Select(_ => 42);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Selecting a Right Either should produce a Right Either.")]
        [Category("Extension")]
        public void Select_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);
            Assert.True(value.IsRight);

            // act
            var actual = value.Select(_ => true);
            Assert.IsTrue(actual.IsRight);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, bool>(true)));
        }

        [Test(Description = "Selecting a Bottom Either should throw.")]
        [Category("Extension")]
        public void Select_Bottom_Throws()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var ex = Assert.Throws<InvalidOperationException>(() => value.Select(_ => true));

            // assert
            Assert.That(ex, Has.Message.Contains(Resources.EitherIsBottom));
        }

        [Test(Description = "Selecting from two Left eithers should produce a Left either.")]
        [Category("Extension")]
        public void SelectManyResult_LeftLeft()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual.IsLeft, Is.True);
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "Selecting from a Left Either and a Right Either should produce a Left Either.")]
        [Category("Extension")]
        public void SelectManyResult_LeftRight()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(33);

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual.IsLeft, Is.True);
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "Selecting from two Right eithers should produce a Right either.")]
        [Category("Extension")]
        public void SelectManyResult_RightRight()
        {
            // arrange
            var left = Either.Right<string, int>(42);
            var right = Either.Right<string, int>(33);

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual.IsLeft, Is.False);
            Assert.That(actual.IsRight, Is.True);
            Assert.That(actual, Is.EqualTo(Either.Right<string, int>(75)));
        }

        [Test(Description = "Folding over a Left Either should return the seed value.")]
        [Category("Extension")]
        public void Aggregate_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test(Description = "Folding over a Right Either should return the result of invoking the " +
                            "accumulator over the seed value and the Right value.")]
        [Category("Extension")]
        public void Aggregate_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(50));
        }

        [Test(Description = "Folding over a Left Either should return the seed value.")]
        [Category("Extension")]
        public void ResultAggregate_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v, v => v * 2);

            // assert
            Assert.That(actual, Is.EqualTo(84));
        }

        [Test(Description = "Folding over a Right Either should return the result of invoking the " +
                            "accumulator over the seed value and the Right value.")]
        [Category("Extension")]
        public void ResultAggregate_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v.Length, v => v * 2);

            // assert
            Assert.That(actual, Is.EqualTo(100));
        }

        #endregion

        #endregion
    }
}
