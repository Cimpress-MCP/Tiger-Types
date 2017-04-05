// ReSharper disable All

using System;
using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    public sealed class EitherTests
    {
        const string sentinel = "sentinel";

        #region IsLeft, IsRight

        [Fact(DisplayName = "A Left Either is in the Left state.")]
        public void FromLeftBoth_IsLeft_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.True(actual.IsLeft);
        }

        [Fact(DisplayName = "A Left Either is in the Left state.")]
        public void FromLeftOne_IsLeft_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

            // assert
            Assert.True(actual.IsLeft);
        }

        [Fact(DisplayName = "A Right Either is not in the Left state.")]
        public void FromRightBoth_IsLeft_Right()
        {
            // arrange, act
            var actual = Either<string, int>.FromRight(42);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Fact(DisplayName = "A Right Either is not in the Left state.")]
        public void FromRightOne_IsLeft_Right()
        {
            // arrange, act
            var actual = Either.Right<int, string>(sentinel);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Fact(DisplayName = "A Bottom Either is not in the Left state.")]
        public void Default_IsLeft_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Fact(DisplayName = "A Left Either is not in the Right state.")]
        public void FromLeftBoth_IsRight_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "A Left Either is not in the Right state.")]
        public void FromLeftOne_IsRight_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

            // assert
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "A Right Either is in the Right state.")]
        public void FromRightBoth_IsRight_Right()
        {
            // arrange, act
            var actual = Either<int, string>.FromRight(sentinel);

            // assert
            Assert.True(actual.IsRight);
        }

        [Fact(DisplayName = "A Right Either is in the Right state.")]
        public void FromRightOne_IsRight_Right()
        {
            // arrange, act
            var actual = Either.Right<int, string>(sentinel);

            // assert
            Assert.True(actual.IsRight);
        }

        [Fact(DisplayName = "A Bottom Either is not in the Right state.")]
        public void Default_IsRight_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.False(actual.IsRight);
        }

        #endregion

        #region Match

        [Fact(DisplayName = "Matching a Left Either returns the Left func branch, " +
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
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
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
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left func branch, " +
                            "not the Right task branch.")]
        public async Task FuncTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length,
                right: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right task branch, " +
                            "not the Left func branch.")]
        public async Task FuncTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l,
                right: r => r.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left task branch, " +
                    "not the Right func branch.")]
        public async Task TaskFuncMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: r => r);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskFuncMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: FromResult,
                right: r => r.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left task branch, " +
                            "not the Right func branch.")]
        public async Task TaskTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: FromResult,
                right: r => r.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either executes the Left action branch, " +
                            "not the Right action branch.")]
        public void ActionActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            var unit = value.Match(
                left: l => actual = sentinel,
                right: r => { });

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Right Either executes the Right v branch, " +
                            "not the Left v branch.")]
        public void ActionActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            var unit = value.Match(
                left: l => { },
                right: r => actual = sentinel);

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Left Either executes the Left action branch, " +
                            "not the Right task branch.")]
        public async Task ActionTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => actual = sentinel,
                right: r => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Right Either executes the Right task branch, " +
                            "not the Left action branch.")]
        public async Task ActionTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => { },
                right: r => Run(() => actual = sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Left Either executes the Left task branch, " +
                            "not the Right action branch.")]
        public async Task TaskActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Run(() => actual = sentinel),
                right: r => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Right Either executes the Right action branch, " +
                            "not the Left task branch.")]
        public async Task TaskActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => CompletedTask,
                right: r => actual = sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Left Either executes the Left task branch, " +
                            "not the Right task branch.")]
        public async Task TaskTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Run(() => actual = sentinel),
                right: r => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Right Either executes the Right task branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => CompletedTask,
                right: r => Run(() => actual = sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Map

        [Fact(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public void FuncMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(left: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public void FuncMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(left: _ => false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(left: _ => FromResult(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public async Task TaskMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(left: _ => FromResult(false));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public void FuncMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Map(right: _ => 42);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public void FuncMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");
            Assert.True(value.IsRight);

            // act
            var actual = value.Map(right: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Map(right: _ => FromResult(42));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public async Task TaskMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");

            // act
            var actual = await value.Map(right: _ => FromResult(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public void FuncFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(
                left: _ => sentinel,
                right: r => r);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public void FuncFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(
                left: _ => false,
                right: r => r);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public async Task FuncTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => FromResult(sentinel),
                right: r => r);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public async Task FuncTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => FromResult(false),
                right: r => r);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => sentinel,
                right: FromResult);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public async Task TaskFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => false,
                right: FromResult);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => FromResult(sentinel),
                right: FromResult);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public async Task TaskTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => FromResult(false),
                right: FromResult);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region Bind

        [Fact(DisplayName = "Right-Binding a Left Either over a func returns a Left Either.")]
        public void FuncBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public void FuncBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(33, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public void FuncBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Right Either over a func returns a Right Either.")]
        public void FuncBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public void FuncBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42
                ? Either.Right<bool, string>(sentinel)
                : Either.Left<bool, string>(true));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public void FuncBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42
                ? Either.Left<bool, string>(true)
                : Either.Right<bool, string>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Left Either over a task returns a Left Either.")]
        public async Task TaskBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => FromResult(v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(33, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Right Either over a task returns a Right Either.")]
        public async Task TaskBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(v => FromResult(v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => FromResult(v == 42
                ? Either.Right<bool, string>(sentinel)
                : Either.Left<bool, string>(true)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => FromResult(v == 42
                ? Either.Left<bool, string>(true)
                : Either.Right<bool, string>(sentinel)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
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
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(false, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
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
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(99L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
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
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
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
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(33L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskFuncBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(false, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskFuncBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(99L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskFuncBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskFuncBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(33L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task FuncTaskBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(false, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task FuncTaskBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(99L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task FuncTaskBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task FuncTaskBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(33L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskTaskBindBoth_Right_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(false, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskTaskBindBoth_Right_Left()
        {
            // arrange
            var value = Either.Right<string, int>(43);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(99L, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskTaskBindBoth_Left_Right()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskTaskBindBoth_Left_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Bind(
                left: s => FromResult(s == sentinel
                    ? Either.Right<long, bool>(true)
                    : Either.Left<long, bool>(33L)),
                right: i => FromResult(i == 42
                    ? Either.Right<long, bool>(false)
                    : Either.Left<long, bool>(99L)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (long)actual;
            Assert.Equal(33L, innerValue);
        }

        #endregion

        #region Fold

        [Fact(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        public void FuncFoldRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator" +
                            "over the seed value and the Right value.")]
        public void FuncFoldRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        public void FuncFoldLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
                            "over the seed value and the Left value.")]
        public void FuncFoldLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        public async Task TaskFoldRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator" +
                            "over the seed value and the Right value.")]
        public async Task TaskFoldRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        public async Task TaskFoldLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
                            "over the seed value and the Left value.")]
        public async Task TaskFoldLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(42, actual);
        }

        #endregion

        #region Tap

        [Fact(DisplayName = "Left-tapping a Left Either over a func returns a Left Either " +
                            "and perform an action")]
        public void FuncTapLeft_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = 33;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Right Either over a func returns a Right Either " +
                            "and perform no action.")]
        public void FuncTapLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = sentinel;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Left Either over a func returns a Left Either " +
                            "and perform no action.")]
        public void FuncTapRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = sentinel;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Right Either over a func returns a Right Either " +
                            "and perform an action.")]
        public void FuncTapRight_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = 33;
            var actual = value.Tap(v => output = v);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Left Either over a task returns a Left Either " +
                            "and perform an action")]
        public async Task TaskTapLeft_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = 33;
            var actual = await value.Tap(v => Run(() => output = v));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Right Either over a task returns a Right Either " +
                            "and perform no action.")]
        public async Task TaskTapLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = sentinel;
            var actual = await value.Tap(v => Run(() => output = v));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Left Either over a task returns a Left Either " +
                            "and perform no action.")]
        public async Task TaskTapRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var output = sentinel;
            var actual = await value.Tap(v => Run(() => output = v));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Right Either over a task returns a Right Either " +
                            "and perform an action.")]
        public async Task TaskTapRight_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var output = 33;
            var actual = await value.Tap(v => Run(() => output = v));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        #endregion

        #region Let

        [Fact(DisplayName = "Left-conditionally executing an action based on a Left Either executes.")]
        public void ActionLetLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = "megatron";
            var unit = value.Let(v => actual = v);

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Left-conditionally executing an action based on a Right Either " +
                            "does not execute.")]
        public void ActionLetLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = 42;
            var unit = value.Let(v => actual = v);

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Right-conditionally executing an action based on a Left Either " +
                            "does not execute.")]
        public void ActionLetRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            value.Let(v => actual = v);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Right-conditionally executing an action based on a Right Either executes.")]
        public void ActionLetRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            value.Let(v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Left-conditionally executing a task based on a Left Either executes.")]
        public async Task TaskLetLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = "megatron";
            await value.Let(v => Run(() => actual = v));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Left-conditionally executing a Task based on a Right Either " +
                            "does not execute.")]
        public async Task TaskLetLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = 42;
            await value.Let(v => Run(() => actual = v));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Right-conditionally executing a Task based on a Left Either " +
                            "does not execute.")]
        public async Task TaskLetRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            await value.Let(v => Run(() => actual = v));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Right-conditionally executing a Task based on a Right Either executes.")]
        public async Task TaskLetRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            await value.Let(v => Run(() => actual = v));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Recover

        [Fact(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public void ValueRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Recover(42);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Recovering a Right Option returns the original value.")]
        public void ValueRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover("megatron");

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public void FuncRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Recover(() => 42);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Recovering a Right Option returns the original value.")]
        public void FuncRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover(() => "megatron");

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Recovering a Left Option returns the recovery value.")]
        public async Task TaskRecover_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Recover(() => FromResult(42));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Recovering a Right Option returns the original value.")]
        public async Task TaskRecover_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Recover(() => FromResult("megatron"));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region Value

        [Fact(DisplayName = "Forcibly unwrapping a Left Either throws.")]
        public void Value_Left_Throws()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = Record.Exception(() => value.Value);
            
            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }

        [Fact(DisplayName = "Forcibly unwrapping a Right Either returns the Right value.")]
        public void Value_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Left Either with an alternative value " +
                            "returns the alternative value.")]
        public void GetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(default(string), actual);
        }

        [Fact(DisplayName = "Coalescing a Right Either with an alternative value " +
                            "returns the Right value.")]
        public void GetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Left Either with an alternative value " +
                            "returns the alternative value.")]
        public void ValueGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault(sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Right Either with an alternative value " +
                            "returns the Right value.")]
        public void ValueGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault("megatron");

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Left Either with an alternative value " +
            "returns the alternative value.")]
        public void FuncGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.GetValueOrDefault(() => sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Right Either with an alternative value " +
                            "returns the Right value.")]
        public void FuncGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetValueOrDefault(() => "megatron");

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Left Either with an alternative value " +
                            "returns the alternative value.")]
        public async Task TaskGetValueOrDefault_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.GetValueOrDefault(() => FromResult(sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Right Either with an alternative value " +
                            "returns the Right value.")]
        public async Task TaskGetValueOrDefault_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.GetValueOrDefault(() => FromResult("megatron"));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Overrides

        [Fact(DisplayName = "A Left Either stringifies to Left.")]
        public void ToString_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Left({sentinel})", actual);
        }

        [Fact(DisplayName = "A Right Either stringifies to Right.")]
        public void ToString_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Right({sentinel})", actual);
        }

        [Fact(DisplayName = "A Bottom Either stringifies to Bottom.")]
        public void ToString_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("Bottom", actual);
        }

        [Fact(DisplayName = "A Left Either is not equal to null.")]
        public void ObjectEquals_LeftNull()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A Right Either is not equal to null.")]
        public void ObjectEquals_RightNull()
        {
            // arrange
            var left = Either.Right<int, string>(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A Bottom Either is not equal to null.")]
        public void ObjectEquals_BottomNull()
        {
            // arrange
            var left = default(Either<string, int>);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of different type, in different state, with" +
                            "different value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<bool, string>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of different type, in different state, with" +
                            "same value are not equal.")]
        public void ObjectEquals_DifferentType_DifferentState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<bool, string>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of different type, in same state, with" +
                            "different value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, bool>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of different type, in same state, with" +
                            "same value are not equal.")]
        public void ObjectEquals_DifferentType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, bool>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of same type, in different state, with" +
                            "different value are not equal.")]
        public void ObjectEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "different value are not equal.")]
        public void ObjectEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "same value are equal.")]
        public void ObjectEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public void ObjectEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A Left Either has a hashcode of its Left value.")]
        public void GetHashCode_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        [Fact(DisplayName = "A Right Either has a hashcode of its Right value.")]
        public void GetHashCode_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        [Fact(DisplayName = "A Bottom Either has a hashcode of 0.")]
        public void GetHashCode_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(0, actual);
        }

        #endregion

        #region Implementations

        [Fact(DisplayName = "A Left Either does not enumerate.")]
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
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "A Right Either enumerates.")]
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
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "A Bottom Either does not enumerate.")]
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
            Assert.Equal(42, actual);
        }

        #endregion

        #region Operators and Named Alternatives

        [Fact(DisplayName = "Two Eithers of same type, in different state, with" +
                            "different value are not equal.")]
        public void OperatorEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "different value are not equal.")]
        public void OperatorEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left == right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "same value are equal.")]
        public void OperatorEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public void OperatorEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Eithers of same type, in different state, with" +
                            "different value are not equal.")]
        public void OperatorNotEquals_SameType_DifferentState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Right<string, int>(42);

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        // note(cosborn) Same Type, Different State, Same Value is impossible.

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "different value are not equal.")]
        public void OperatorNotEquals_SameType_SameState_DifferentValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>("megatron");

            // act
            var actual = left != right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two Eithers of same type, in same state, with" +
                            "same value are equal.")]
        public void OperatorNotEquals_SameType_SameState_SameValue()
        {
            // arrange
            var left = Either.Left<string, int>(sentinel);
            var right = Either.Left<string, int>(sentinel);

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two Bottom Eithers are equal.")]
        public void OperatorNotEquals_BottomBottom()
        {
            // arrange
            var left = default(Either<int, string>);
            var right = default(Either<int, string>);

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "An EitherLeft converts implicitly to a Left Either.")]
        public void EitherLeft_ToEither()
        {
            // arrange, act
            Either<string, int> actual = Either.Left(sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "An EitherRight converts implicitly to a Right Either.")]
        public void EitherRight_ToEither()
        {
            // arrange, act
            Either<int, string> actual = Either.Right(sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "An EitherLeft and EitherRight behave together with type inference.")]
        public void EitherSided_Combine()
        {
            // arrange
            Func<int, Either<string, bool>> func = i =>
            {
                if (i <= 0)
                {
                    return Either.Right(true);
                }
                return Either.Left(sentinel);
            };

            // act
            var actual = func(42);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "A value of the Left type converts to a Left Either.")]
        public void Left_IsLeft()
        {
            // arrange, act
            Either<string, int> actual = sentinel;

            // assert
            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "A value of the Right type converts to a Right Either.")]
        public void Right_IsRight()
        {
            // arrange, act
            Either<int, string> actual = sentinel;

            // assert
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Null converts to a Bottom Either.")]
        public void Bottom_IsBottom()
        {
            // arrange, act
            Either<int, string> actual = null;

            // assert
            Assert.False(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "Unwrapping a Left Either throws.")]
        public void Cast_Left_Throws()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = Record.Exception(() => (int)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }

        [Fact(DisplayName = "Unwrapping a Left Either returns the Left value.")]
        public void Cast_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Unwrapping a Right Either returns its Right value.")]
        public void Cast_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Unwrapping a Bottom Either throws.")]
        public void Cast_Bottom_Throwws()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = Record.Exception(() => (int)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsNotRight, ex.Message);
        }

        #endregion

        #region Split

        [Fact(DisplayName = "Splitting a value over a func, failing the condition, " +
                            "returns a Left Either.")]
        public void FuncSplit_Left()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = Either.Split(value, v => v.Length == 0);

            // assert
            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "Splitting a value over a func, passing the condition, " +
                            "returns a Left Either.")]
        public void FuncSplit_Right()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = Either.Split(value, v => v.Length == 8);

            // assert
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }

        [Fact(DisplayName = "Splitting a value over a task, failing the condition, " +
                            "returns a Left Either.")]
        public async Task TaskSplit_Left()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = await Either.Split(value, v => FromResult(v.Length == 0));

            // assert
            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "Splitting a value over a task, passing the condition, " +
                            "returns a Left Either.")]
        public async Task TaskSplit_Right()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = await Either.Split(value, v => FromResult(v.Length == 8));

            // assert
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }

        #endregion

        #region Join

        [Fact(DisplayName = "Joining a Left Either Either returns a Left Either.")]
        public void Join_Left()
        {
            // arrange
            var value = Either<int, Either<int, string>>.FromLeft(42);

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsLeft);
            Assert.Equal(42, (int)actual);
        }

        [Fact(DisplayName = "Joining a Right Either containing a Left Either returns a Left Either.")]
        public void Join_RightLeft()
        {
            // arrange
            var value = Either<int, Either<int, string>>.FromRight(Either<int, string>.FromLeft(42));

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsLeft);
            Assert.Equal(42, (int)actual);
        }

        [Fact(DisplayName = "Joining a Right Either containing a Right Either returns a Right Either.")]
        public void Join_RightRight()
        {
            // arrange
            var value = Either<int, Either<int, string>>.FromRight(Either<int, string>.FromRight(sentinel));

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsRight);
            Assert.Equal(sentinel, actual.Value);
        }

        #endregion

        #region Extensions

        #region LINQ

        [Fact(DisplayName = "Asking a Left Either for any returns false.")]
        public void Any_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any returns true.")]
        public void Any_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for any with a false predicate returns false.")]
        public void PredicateAny_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for any with a true predicate returns false.")]
        public void PredicateAny_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any with a false predicate returns false.")]
        public void PredicateAny_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any with a true predicate returns true.")]
        public void PredicateAny_RightTrue()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for all with a false predicate returns true.")]
        public void PredicateAll_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for all with a true predicate returns true.")]
        public void PredicateAll_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for all with a false predicate returns false.")]
        public void PredicateAll_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for all with a true predicate returns true.")]
        public void PredicateAll_RightTrue()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.All(v => true);

            // assert
            Assert.True(actual);
        }

        [Theory(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        public void Contains_Left(int testValue)
        {
            // arrange
            var value = Either.Left<int, string>(testValue);

            // act
            var actual = value.Contains(sentinel);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either whether it contains a value that it doesn't " +
                            "returns false.")]
        public void Contains_Right_False()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains("megatron");

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either whether it contains a value that it does " +
                            "returns true.")]
        public void Contains_Right_True()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains(sentinel);

            // assert
            Assert.True(actual);
        }

        [Theory(DisplayName = "Asking a Left Either whether it contains a value returns false.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        public void ComparerContains_Left(int testValue)
        {
            // arrange
            var value = Either.Left<int, string>(testValue);

            // act
            var actual = value.Contains(sentinel, StringComparer.Ordinal);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either whether it contains a value that it doesn't " +
                            "returns false.")]
        public void ComparerContains_Right_False()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains("megatron", StringComparer.Ordinal);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either whether it contains a value that it does " +
                            "returns true.")]
        public void ComparerContains_Right_True()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains(sentinel, StringComparer.Ordinal);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Recovering a Left Either returns the recovery value.")]
        public void DefaultIfEmpty_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Fact(DisplayName = "Recovering a Right Either returns the Right value.")]
        public void DefaultIfEmpty_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Recover("megatron");

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Recovering a Bottom Either returns the recovery value.")]
        public void DefaultIfEmpty_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(0, innerValue);
        }

        [Fact(DisplayName = "Tapping a Left Either over a func returns a Left Either " +
                            "and perform no action.")]
        public void Do_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var output = 42;
            var actual = value.Do(v => output = v);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(42, output);
        }

        [Fact(DisplayName = "Tapping a Right Either over a func returns a Right Either " +
                            "and perform an action.")]
        public void Do_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var output = "megatron";
            var actual = value.Do(v => output = v);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a Left Either " +
                            "does not execute.")]
        public void ForEach_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = 42;
            value.ForEach(v => actual = v);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a Right Either executes.")]
        public void ForEach_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = "megatron";
            value.ForEach(v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Selecting a Left Either produces a Left Either.")]
        public void Select_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Select(_ => 42);

            // assert
            Assert.Equal(Either.Left<string, int>(sentinel), actual);
        }

        [Fact(DisplayName = "Selecting a Right Either produces a Right Either.")]
        public void Select_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);
            Assert.True(value.IsRight);

            // act
            var actual = value.Select(_ => true);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (bool)actual;
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Selecting a Bottom Either throws.")]
        public void Select_Bottom_Throws()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = Record.Exception(() => value.Select(_ => true));
            
            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.EitherIsBottom, ex.Message);
        }

        [Fact(DisplayName = "Selecting from two Left eithers produces a Left either.")]
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
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Selecting from a Left Either and a Right Either produces a Left Either.")]
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
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Selecting from two Right eithers produces a Right either.")]
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
            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(75, innerValue);
        }

        [Fact(DisplayName = "Folding over a Left Either returns the seed value.")]
        public void Aggregate_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Folding over a Right Either returns the result of invoking the " +
                            "accumulator over the seed value and the Right value.")]
        public void Aggregate_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v.Length);

            // assert
            Assert.Equal(50, actual);
        }

        [Fact(DisplayName = "Folding over a Left Either returns the seed value.")]
        public void ResultAggregate_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v, v => v * 2);

            // assert
            Assert.Equal(84, actual);
        }

        [Fact(DisplayName = "Folding over a Right Either returns the result of invoking the " +
                            "accumulator over the seed value and the Right value.")]
        public void ResultAggregate_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v.Length, v => v * 2);

            // assert
            Assert.Equal(100, actual);
        }

        #endregion

        #endregion
    }
}
