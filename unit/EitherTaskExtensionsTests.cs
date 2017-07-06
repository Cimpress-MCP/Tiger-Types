using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="EitherTaskExtensions"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) })]
    public static class EitherTaskExtensionsTests
    {
        #region MatchT

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, " +
            "not the Right func branch.")]
        public static void FuncFuncMatchTReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MatchT(
                left: l => l.Length,
                right: r => r).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left func branch.")]
        public static void FuncFuncMatchTReturn_Right(Version right)
        {
            // arrange
            var value = FromResult<Either<int, Version>>(right);

            // act
            var actual = value.MatchT(
                left: l => l,
                right: r => r.Major).Result;

            // assert
            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, " +
            "not the Right task branch.")]
        public static void FuncTaskMatchTReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MatchT(
                left: l => l.Length,
                right: FromResult).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, " +
            "not the Left func branch.")]
        public static void FuncTaskMatchTReturn_Right(Version right)
        {
            // arrange
            var value = FromResult<Either<int, Version>>(right);

            // act
            var actual = value.MatchT(
                left: l => l,
                right: r => r.Major.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static void TaskFuncMatchTReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: r => r).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static void TaskFuncMatchTReturn_Right(Version right)
        {
            // arrange
            var value = FromResult<Either<int, Version>>(right);

            // act
            var actual = value.MatchT(
                left: FromResult,
                right: r => r.Major).Result;

            // assert
            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static void TaskTaskMatchTReturn_Left(NonNull<string> left)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult).Result;

            // assert
            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static void TaskTaskMatchTReturn_Right(Version right)
        {
            // arrange
            var value = FromResult<Either<int, Version>>(right);

            // act
            var actual = value.MatchT(
                left: FromResult,
                right: r => r.Major.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(right.Major, actual);
        }

        #endregion

        #region MapT

        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MapT(left: _ => sentinel.Get).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.MapT(left: _ => sentinel.Get).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MapT(left: _ => FromResult(sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public static void TaskMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.MapT(left: _ => FromResult(sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapTRight_Left(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MapT(right: _ => sentinel).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapTRight_Right(int right, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.MapT(right: _ => sentinel).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public static void TaskMapTRight_Left(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.MapT(right: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public static void TaskMapTRight_Right(int right, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.MapT(right: _ => FromResult(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region BindT

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Right Either " +
            "returns a Left Either.")]
        public static void FuncBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(right: _ => Either.Right<string, Version>(sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(right: _ => Either.Left<string, Version>(sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(right: _ => Either.Left<string, Version>(sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncBindTRight_Right_Right(int right, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(right: _ => Either.Right<string, Guid>(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncBindTLeft_Right_Right(int right, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(left: _ => Either.Right<Guid, int>(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Left Either " +
            "returns a Right Either.")]
        public static void FuncBindTLeft_Right_Left(int right, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(left: _ => Either.Left<Guid, int>(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static void FuncBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(left: _ => Either.Right<Guid, int>(sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static void FuncBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(left: _ => Either.Left<Guid, int>(sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Right Either" +
            "returns a Left Either.")]
        public static void TaskBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Left Either" +
            "returns a Left Either.")]
        public static void TaskBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskBindTRight_Right_Right(int right, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskBindTLeft_Right(int right, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Left Either " +
            "returns a Right Either.")]
        public static void TaskBindTLeft_Right_Left(int right, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = value.BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static void TaskBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static void TaskBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = value.BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel))).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region TapT

        [Property(DisplayName = "Left-tapping a Left Either over a func returns a Left Either " +
            "and performs an action.")]
        public static void FuncTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = value.TapT(left: _ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a func returns a Right Either " +
            "and performs no action.")]
        public static void FuncTapTLeft_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = value.TapT(left: _ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a func returns a Left Either " +
            "and performs no action.")]
        public static void FuncTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = value.TapT(right: _ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a func returns a Right Either " +
            "and performs an action.")]
        public static void FuncTapTRight_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = value.TapT(right: _ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Left Either over a task returns a Left Either " +
            "and performs an action")]
        public static void TaskTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = value.TapT(left: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a task returns a Right Either " +
            "and performs no action.")]
        public static void TaskTapTLeft_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = value.TapT(left: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a task returns a Left Either " +
            "and performs no action.")]
        public static void TaskTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = value.TapT(right: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a task returns a Right Either " +
            "and performs an action.")]
        public static void TaskTapTRight_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = value.TapT(right: _ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        #endregion
    }
}
