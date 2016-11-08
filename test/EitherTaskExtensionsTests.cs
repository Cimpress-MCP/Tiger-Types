// ReSharper disable All

using System;
using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="EitherTaskExtensions"/>.</summary>
    public sealed class EitherTaskExtensionsTests
    {
        const string sentinel = "sentinel";

        #region MatchT

        [Fact(DisplayName = "Matching a Left Either returns the Left func branch, " +
                            "not the Right func branch.")]
        public async Task FuncFuncMatchTReturn_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
                            "not the Left func branch.")]
        public async Task FuncFuncMatchTReturn_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l,
                right: r => r.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left func branch, " +
                            "not the Right task branch.")]
        public async Task FuncTaskMatchTReturn_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l.Length,
                right: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right task branch, " +
                            "not the Left func branch.")]
        public async Task FuncTaskMatchTReturn_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l,
                right: r => r.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left task branch, " +
                    "not the Right func branch.")]
        public async Task TaskFuncMatchTReturn_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: r => r);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskFuncMatchTReturn_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: FromResult,
                right: r => r.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Left Either returns the Left task branch, " +
                            "not the Right func branch.")]
        public async Task TaskTaskMatchTReturn_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a Right Either returns the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchTReturn_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MatchT(
                left: FromResult,
                right: r => r.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        #endregion

        #region MapT

        [Fact(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public async Task FuncMapTLeft_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>("megatron");

            // act
            var actual = await value.MapT(left: _ => sentinel);

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public async Task FuncMapTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MapT(left: _ => false);

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskMapTLeft_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>("megatron");

            // act
            var actual = await value.MapT(left: _ => FromResult(sentinel));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public async Task TaskMapTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.MapT(left: _ => FromResult(false));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public async Task FuncMapTRight_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MapT(right: _ => 42);

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public async Task FuncMapTRight_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>("megatron");

            // act
            var actual = await value.MapT(right: _ => sentinel);

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public async Task TaskMapTRight_Left()
        {
            // arrange
            var value = FromResult<Either<string, int>>(sentinel);

            // act
            var actual = await value.MapT(right: _ => FromResult(42));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public async Task TaskMapTRight_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>("megatron");

            // act
            var actual = await value.MapT(right: _ => FromResult(sentinel));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region BindT

        [Fact(DisplayName = "Right-Binding a Left Either over a func returns a Left Either.")]
        public async Task FuncBindTRight_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task FuncBindTRight_Right_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.BindT(v => v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(33, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task FuncBindTRight_Right_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.BindT(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Right Either over a func returns a Right Either.")]
        public async Task FuncBindTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var actual = await value.BindT(v => v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task FuncBindTLeft_Left_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => v == 42 ? Either.Right<bool, string>(sentinel) : Either.Left<bool, string>(true));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task FuncBindTLeft_Left_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => v == 42 ? Either.Left<bool, string>(true) : Either.Right<bool, string>(sentinel));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Left Either over a task returns a Left Either.")]
        public async Task TaskBindTRight_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskBindTRight_Right_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.BindT(v => FromResult(v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true)));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(33, innerValue);
        }

        [Fact(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskBindTRight_Right_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(sentinel);

            // act
            var actual = await value.BindT(v => FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(true, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Right Either over a task returns a Right Either.")]
        public async Task TaskBindTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var actual = await value.BindT(v => FromResult(v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33)));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either " +
                            "returns a Right Either.")]
        public async Task TaskBindTLeft_Left_Right()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => FromResult(v == 42
                ? Either.Right<bool, string>(sentinel)
                : Either.Left<bool, string>(true)));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either " +
                            "returns a Left Either.")]
        public async Task TaskBindTLeft_Left_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var actual = await value.BindT(v => FromResult(v == 42
                ? Either.Left<bool, string>(true)
                : Either.Right<bool, string>(sentinel)));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(true, innerValue);
        }

        #endregion

        #region TapT

        [Fact(DisplayName = "Left-tapping a Left Either over a func returns a Left Either " +
                            "and perform an action")]
        public async Task FuncTapTLeft_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var output = 33;
            var actual = await value.TapT(v => output = v);

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Right Either over a func returns a Right Either " +
                            "and perform no action.")]
        public async Task FuncTapTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => output = v);

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Left Either over a func returns a Left Either " +
                            "and perform no action.")]
        public async Task FuncTapTRight_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => output = v);

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Right Either over a func returns a Right Either " +
                            "and perform an action.")]
        public async Task FuncTapTRight_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var output = 33;
            var actual = await value.TapT(v => output = v);

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Left Either over a task returns a Left Either " +
                            "and perform an action")]
        public async Task TaskTapTLeft_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var output = 33;
            var actual = await value.TapT(v => Run(() => output = v));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Left-tapping a Right Either over a task returns a Right Either " +
                            "and perform no action.")]
        public async Task TaskTapTLeft_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => Run(() => output = v));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Left Either over a task returns a Left Either " +
                            "and perform no action.")]
        public async Task TaskTapTRight_Left()
        {
            // arrange
            var value = FromResult<Either<int, string>>(42);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => Run(() => output = v));

            // assert
            var innerValue = Assert.Left(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Right-tapping a Right Either over a task returns a Right Either " +
                            "and perform an action.")]
        public async Task TaskTapTRight_Right()
        {
            // arrange
            var value = FromResult<Either<string, int>>(42);

            // act
            var output = 33;
            var actual = await value.TapT(v => Run(() => output = v));

            // assert
            var innerValue = Assert.Right(actual);
            Assert.Equal(42, innerValue);
        }

        #endregion
    }
}
