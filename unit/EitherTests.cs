using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [Properties(Arbitrary = new [] { typeof(Generators) })]
    public static class EitherTests
    {
        const string sentinel = "sentinel";

        #region IsLeft, IsRight

        [Property(DisplayName = "A Left Either is in the Left state.")]
        public static void FromLeftBoth_IsLeft_Left(NonNull<string> left)
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(left.Get);

            // assert
            Assert.True(actual.IsLeft);
        }

        [Property(DisplayName = "A Left Either is in the Left state.")]
        public static void FromLeftOne_IsLeft_Left(NonNull<string> left)
        {
            // arrange, act
            var actual = Either.Left<string, int>(left.Get);

            // assert
            Assert.True(actual.IsLeft);
        }

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        public static void FromRightBoth_IsLeft_Right(int right)
        {
            // arrange, act
            var actual = Either<string, int>.FromRight(right);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        public static void FromRightOne_IsLeft_Right(int right)
        {
            // arrange, act
            var actual = Either.Right<string, int>(right);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Fact(DisplayName = "A Bottom Either is not in the Left state.")]
        public static void Default_IsLeft_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.False(actual.IsLeft);
        }

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        public static void FromLeftBoth_IsRight_Left(NonNull<string> left)
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(left.Get);

            // assert
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        public static void FromLeftOne_IsRight_Left(NonNull<string> left)
        {
            // arrange, act
            var actual = Either.Left<string, int>(left.Get);

            // assert
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "A Right Either is in the Right state.")]
        public static void FromRightBoth_IsRight_Right(int right)
        {
            // arrange, act
            var actual = Either<string, int>.FromRight(right);

            // assert
            Assert.True(actual.IsRight);
        }

        [Property(DisplayName = "A Right Either is in the Right state.")]
        public static void FromRightOne_IsRight_Right(int right)
        {
            // arrange, act
            var actual = Either.Right<string, int>(right);

            // assert
            Assert.True(actual.IsRight);
        }

        [Fact(DisplayName = "A Bottom Either is not in the Right state.")]
        public static void Default_IsRight_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.False(actual.IsRight);
        }

        #endregion

        #region Match

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, " +
            "not the Right func branch.")]
        public static void FuncFuncMatchReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left func branch.")]
        public static void FuncFuncMatchReturn_Right(int right)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, " +
            "not the Right task branch.")]
        public static void FuncTaskMatchReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: FromResult).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, " +
            "not the Left func branch.")]
        public static void FuncTaskMatchReturn_Right(int right)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: FromResult).Result;

            // assert
            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static void TaskFuncMatchReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: r => r).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static void TaskFuncMatchReturn_Right(int right)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: r => r).Result;

            // assert
            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static void TaskTaskMatchReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static void TaskTaskMatchReturn_Right(int right)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult).Result;

            // assert
            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, " +
            "not the Right action branch.")]
        public static void ActionActionMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            var unit = value.Match(
                left: l => actual = sentinel.Get,
                right: r => { });

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, " +
            "not the Left action branch.")]
        public static void ActionActionMatchVoid_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            var unit = value.Match(
                left: l => { },
                right: r => actual = sentinel);

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, " +
            "not the Right task branch.")]
        public static void ActionTaskMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            value.Match(
                left: l => actual = sentinel.Get,
                right: r => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, " +
            "not the Left action branch.")]
        public static void ActionTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            value.Match(
                left: l => { },
                right: r => Run(() => actual = sentinel)).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, " +
            "not the Right action branch.")]
        public static void TaskActionMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            value.Match(
                left: l => Run(() => actual = sentinel.Get),
                right: r => { }).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, " +
            "not the Left task branch.")]
        public static void TaskActionMatchVoid_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            value.Match(
                left: l => CompletedTask,
                right: r => actual = sentinel).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, " +
            "not the Right task branch.")]
        public static void TaskTaskMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            value.Match(
                left: l => Run(() => actual = sentinel.Get),
                right: r => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, " +
            "not the Left task branch.")]
        public static void TaskTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            value.Match(
                left: l => CompletedTask,
                right: r => Run(() => actual = sentinel)).Wait();

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Map

        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapLeft_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(left: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapLeft_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(left: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskMapLeft_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(left: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public static void TaskMapLeft_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(left: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapRight_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(right: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapRight_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(right: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskMapRight_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(right: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public static void TaskMapRight_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(right: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncFuncMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncTaskMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public static void FuncTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskFuncMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public static void TaskFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskTaskMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Map(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public static void TaskTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Map(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        #endregion

        #region Bind

        [Property(DisplayName = "Right-Binding a Left Either over a func returns a Left Either.")]
        public static void FuncBindRight_Left(NonNull<string> left, Version sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(right: _ => Either.Right<string, Version>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncBindRight_Right_Left(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(right: _ => Either.Left<string, Version>(sentinel.Get));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncBindRight_Right_Right(int right, Version sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(right: _ => Either.Right<string, Version>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func returns a Right Either.")]
        public static void FuncBindLeft_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncBindLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(left: _ => Either.Right<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncBindLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task returns a Left Either.")]
        public static void TaskBindRight_Left(NonNull<string> left, Version sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(right: _ => FromResult(Either.Right<string, Version>(sentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskBindRight_Right_Left(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskBindRight_Right_Right(int right, Version sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(right: _ => FromResult(Either.Right<string, Version>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task returns a Right Either.")]
        public static void TaskBindLeft_Right(int right, Guid sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(left: _ => FromResult(Either.Left<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskBindLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(left: _ => FromResult(Either.Right<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskBindLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Bind(left: _ => FromResult(Either.Left<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => Either.Right<Guid, Version>(rightSentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => Either.Left<Guid, Version>(leftSentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncFuncBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => Either.Left<Guid, Version>(leftSentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncFuncBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => Either.Right<Guid, Version>(rightSentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => Either.Right<Guid, Version>(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => Either.Left<Guid, Version>(leftSentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskFuncBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => Either.Left<Guid, Version>(leftSentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskFuncBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => Either.Right<Guid, Version>(rightSentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncTaskBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel))).Result;
            
            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncTaskBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskTaskBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskTaskBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        #endregion

        #region Fold

        [Property(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        public static void FuncFoldRight_Left(NonNull<string> left, int seed)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Fold(seed, right: (s, v) => s + v);

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator" +
            "over the seed value and the Right value.")]
        public static void FuncFoldRight_Right(int right, int seed)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Fold(seed, right: (s, v) => s + v);

            // assert
            Assert.Equal(seed + right, actual);
        }

        [Property(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        public static void FuncFoldLeft_Right(int right, int seed)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Fold(seed, left: (s, v) => s + v.Length);

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
            "over the seed value and the Left value.")]
        public static void FuncFoldLeft_Left(NonNull<string> left, int seed)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Fold(seed, left: (s, v) => s + v.Length);

            // assert
            Assert.Equal(seed + left.Get.Length, actual);
        }

        [Property(DisplayName = "Right-Folding over a Left Either returns the seed value.")]
        public static void TaskFoldRight_Left(NonNull<string> left, int seed)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Fold(seed, right: (s, v) => FromResult(s + v)).Result;

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Right-Folding over a Right Either returns result of invoking the accumulator" +
            "over the seed value and the Right value.")]
        public static void TaskFoldRight_Right(int right, int seed)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Fold(seed, right: (s, v) => FromResult(s + v)).Result;

            // assert
            Assert.Equal(seed + right, actual);
        }

        [Property(DisplayName = "Left-Folding over a Right Either returns the seed value.")]
        public static void TaskFoldLeft_Right(int right, int seed)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var actual = value.Fold(seed, left: (s, v) => FromResult(s + v.Length)).Result;

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Left-Folding over a Left Either returns result of invoking the accumulator" +
            "over the seed value and the Left value.")]
        public static void TaskFoldLeft_Left(NonNull<string> left, int seed)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = value.Fold(seed, left: (s, v) => FromResult(s + v.Length)).Result;

            // assert
            Assert.Equal(seed + left.Get.Length, actual);
        }

        #endregion

        #region Tap

        [Property(DisplayName = "Left-tapping a Left Either over a func returns a Left Either " +
            "and performs an action")]
        public static void FuncTapLeft_Left(NonNull<string> left, int before, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var output = before;
            var actual = value.Tap(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a func returns a Right Either " +
            "and performs no action.")]
        public static void FuncTapLeft_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var output = before;
            var actual = value.Tap(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a func returns a Left Either " +
            "and performs no action.")]
        public static void FuncTapRight_Left(NonNull<string> left, int before, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var output = before;
            var actual = value.Tap(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a func returns a Right Either " +
            "and performs an action.")]
        public static void FuncTapRight_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var output = before;
            var actual = value.Tap(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Left Either over a task returns a Left Either " +
            "and perform an action")]
        public static void TaskTapLeft_Left(NonNull<string> left, int before, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var output = before;
            var actual = value.Tap(left: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a task returns a Right Either " +
            "and perform no action.")]
        public static void TaskTapLeft_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var output = before;
            var actual = value.Tap(left: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a task returns a Left Either " +
            "and perform no action.")]
        public static void TaskTapRight_Left(NonNull<string> left, int before, int sentinel)
        {
            // arrange
            var value = Either.Left<string, int>(left.Get);

            // act
            var output = before;
            var actual = value.Tap(right: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a task returns a Right Either " +
            "and perform an action.")]
        public static void TaskTapRight_Right(int right, int before, int sentinel)
        {
            // arrange
            var value = Either.Right<string, int>(right);

            // act
            var output = before;
            var actual = value.Tap(right: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }

        #endregion

        #region Let

        [Fact(DisplayName = "Left-conditionally executing an action based on a Left Either executes.")]
        public static void ActionLetLeft_Left()
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
        public static void ActionLetLeft_Right()
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
        public static void ActionLetRight_Left()
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
        public static void ActionLetRight_Right()
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
        public static async Task TaskLetLeft_Left()
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
        public static async Task TaskLetLeft_Right()
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
        public static async Task TaskLetRight_Left()
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
        public static async Task TaskLetRight_Right()
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
        public static void ValueRecover_Left()
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
        public static void ValueRecover_Right()
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
        public static void FuncRecover_Left()
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
        public static void FuncRecover_Right()
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
        public static async Task TaskRecover_Left()
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
        public static async Task TaskRecover_Right()
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
        public static void Value_Left_Throws()
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
        public static void Value_Right()
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
        public static void GetValueOrDefault_Left()
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
        public static void GetValueOrDefault_Right()
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
        public static void ValueGetValueOrDefault_Left()
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
        public static void ValueGetValueOrDefault_Right()
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
        public static void FuncGetValueOrDefault_Left()
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
        public static void FuncGetValueOrDefault_Right()
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
        public static async Task TaskGetValueOrDefault_Left()
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
        public static async Task TaskGetValueOrDefault_Right()
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
        public static void ToString_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Left({sentinel})", actual);
        }

        [Fact(DisplayName = "A Right Either stringifies to Right.")]
        public static void ToString_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Right({sentinel})", actual);
        }

        [Fact(DisplayName = "A Bottom Either stringifies to Bottom.")]
        public static void ToString_Bottom()
        {
            // arrange
            var value = default(Either<string, int>);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("Bottom", actual);
        }

        [Fact(DisplayName = "A Left Either is not equal to null.")]
        public static void ObjectEquals_LeftNull()
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
        public static void ObjectEquals_RightNull()
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
        public static void ObjectEquals_BottomNull()
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
        public static void ObjectEquals_DifferentType_DifferentState_DifferentValue()
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
        public static void ObjectEquals_DifferentType_DifferentState_SameValue()
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
        public static void ObjectEquals_DifferentType_SameState_DifferentValue()
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
        public static void ObjectEquals_DifferentType_SameState_SameValue()
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
        public static void ObjectEquals_SameType_DifferentState_DifferentValue()
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
        public static void ObjectEquals_SameType_SameState_DifferentValue()
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
        public static void ObjectEquals_SameType_SameState_SameValue()
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
        public static void ObjectEquals_BottomBottom()
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
        public static void GetHashCode_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        [Fact(DisplayName = "A Right Either has a hashcode of its Right value.")]
        public static void GetHashCode_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        [Fact(DisplayName = "A Bottom Either has a hashcode of 0.")]
        public static void GetHashCode_Bottom()
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
        public static void GetEnumerator_Left()
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
        public static void GetEnumerator_Right()
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
        public static void GetEnumerator_Bottom()
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
        public static void OperatorEquals_SameType_DifferentState_DifferentValue()
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
        public static void OperatorEquals_SameType_SameState_DifferentValue()
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
        public static void OperatorEquals_SameType_SameState_SameValue()
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
        public static void OperatorEquals_BottomBottom()
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
        public static void OperatorNotEquals_SameType_DifferentState_DifferentValue()
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
        public static void OperatorNotEquals_SameType_SameState_DifferentValue()
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
        public static void OperatorNotEquals_SameType_SameState_SameValue()
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
        public static void OperatorNotEquals_BottomBottom()
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
        public static void EitherLeft_ToEither()
        {
            // arrange, act
            Either<string, int> actual = Either.Left(sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "An EitherRight converts implicitly to a Right Either.")]
        public static void EitherRight_ToEither()
        {
            // arrange, act
            Either<int, string> actual = Either.Right(sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "An EitherLeft and EitherRight behave together with type inference.")]
        public static void EitherSided_Combine()
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
        public static void Left_IsLeft()
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
        public static void Right_IsRight()
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
        public static void Bottom_IsBottom()
        {
            // arrange, act
            Either<int, string> actual = null;

            // assert
            Assert.False(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Fact(DisplayName = "Unwrapping a Left Either throws.")]
        public static void Cast_Left_Throws()
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
        public static void Cast_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Unwrapping a Right Either returns its Right value.")]
        public static void Cast_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Unwrapping a Bottom Either throws.")]
        public static void Cast_Bottom_Throwws()
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
        public static void FuncSplit_Left()
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
        public static void FuncSplit_Right()
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
        public static async Task TaskSplit_Left()
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
        public static async Task TaskSplit_Right()
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
        public static void Join_Left()
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
        public static void Join_RightLeft()
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
        public static void Join_RightRight()
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
        public static void Any_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any returns true.")]
        public static void Any_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for any with a false predicate returns false.")]
        public static void PredicateAny_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for any with a true predicate returns false.")]
        public static void PredicateAny_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any with a false predicate returns false.")]
        public static void PredicateAny_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for any with a true predicate returns true.")]
        public static void PredicateAny_RightTrue()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for all with a false predicate returns true.")]
        public static void PredicateAll_LeftFalse()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Left Either for all with a true predicate returns true.")]
        public static void PredicateAll_LeftTrue()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.All(v => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for all with a false predicate returns false.")]
        public static void PredicateAll_RightFalse()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.All(v => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Right Either for all with a true predicate returns true.")]
        public static void PredicateAll_RightTrue()
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
        public static void Contains_Left(int testValue)
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
        public static void Contains_Right_False()
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
        public static void Contains_Right_True()
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
        public static void ComparerContains_Left(int testValue)
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
        public static void ComparerContains_Right_False()
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
        public static void ComparerContains_Right_True()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Contains(sentinel, StringComparer.Ordinal);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Recovering a Left Either returns the recovery value.")]
        public static void DefaultIfEmpty_Left()
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
        public static void DefaultIfEmpty_Right()
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
        public static void DefaultIfEmpty_Bottom()
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
        public static void Do_Left()
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
        public static void Do_Right()
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
        public static void ForEach_Left()
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
        public static void ForEach_Right()
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
        public static void Select_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Select(_ => 42);

            // assert
            Assert.Equal(Either.Left<string, int>(sentinel), actual);
        }

        [Fact(DisplayName = "Selecting a Right Either produces a Right Either.")]
        public static void Select_Right()
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
        public static void Select_Bottom_Throws()
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
        public static void SelectManyResult_LeftLeft()
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
        public static void SelectManyResult_LeftRight()
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
        public static void SelectManyResult_RightRight()
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
        public static void Aggregate_Left()
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
        public static void Aggregate_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Aggregate(42, (s, v) => s + v.Length);

            // assert
            Assert.Equal(50, actual);
        }

        [Fact(DisplayName = "Folding over a Left Either returns the seed value.")]
        public static void ResultAggregate_Left()
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
        public static void ResultAggregate_Right()
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
