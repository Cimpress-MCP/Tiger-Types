using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
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
        public static async Task FuncFuncMatchTReturn_Left(NonNull<string> left)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MatchT(
                left: l => l.Length,
                right: r => r);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left func branch.")]
        public static async Task FuncFuncMatchTReturn_Right(Version right)
        {
            var value = FromResult<Either<int, Version>>(right);

            var actual = await value.MatchT(
                left: l => l,
                right: r => r.Major);

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, " +
            "not the Right task branch.")]
        public static async Task FuncTaskMatchTReturn_Left(NonNull<string> left)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MatchT(
                left: l => l.Length,
                right: FromResult);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, " +
            "not the Left func branch.")]
        public static async Task FuncTaskMatchTReturn_Right(Version right)
        {
            var value = FromResult<Either<int, Version>>(right);

            var actual = await value.MatchT(
                left: l => l,
                right: r => r.Major.Pipe(FromResult));

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static async Task TaskFuncMatchTReturn_Left(NonNull<string> left)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: r => r);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static async Task TaskFuncMatchTReturn_Right(Version right)
        {
            var value = FromResult<Either<int, Version>>(right);

            var actual = await value.MatchT(
                left: FromResult,
                right: r => r.Major);

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, " +
            "not the Right func branch.")]
        public static async Task TaskTaskMatchTReturn_Left(NonNull<string> left)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, " +
            "not the Left task branch.")]
        public static async Task TaskTaskMatchTReturn_Right(Version right)
        {
            var value = FromResult<Either<int, Version>>(right);

            var actual = await value.MatchT(
                left: FromResult,
                right: r => r.Major.Pipe(FromResult));

            Assert.Equal(right.Major, actual);
        }

        #endregion

        #region MapT

        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public static async Task FuncMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MapT(left: _ => sentinel.Get);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public static async Task FuncMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.MapT(left: _ => sentinel.Get);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MapT(left: _ => FromResult(sentinel.Get));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public static async Task TaskMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.MapT(left: _ => FromResult(sentinel.Get));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public static async Task FuncMapTRight_Left(NonNull<string> left, int sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MapT(right: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public static async Task FuncMapTRight_Right(int right, int sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.MapT(right: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskMapTRight_Left(NonNull<string> left, int sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.MapT(right: _ => FromResult(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public static async Task TaskMapTRight_Right(int right, int sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.MapT(right: _ => FromResult(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region BindT

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Right Either " +
            "returns a Left Either.")]
        public static async Task FuncBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.BindT(right: _ => Either.Right<string, Version>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static async Task FuncBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(left.Get);

            var actual = await value.BindT(right: _ => Either.Left<string, Version>(sentinel.Get));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static async Task FuncBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.BindT(right: _ => Either.Left<string, Version>(sentinel.Get));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task FuncBindTRight_Right_Right(int right, Guid sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.BindT(right: _ => Either.Right<string, Guid>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task FuncBindTLeft_Right_Right(int right, int sentinel)
        {
            var value = FromResult<Either<string, int>>(right);

            var actual = await value.BindT(left: _ => Either.Right<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Left Either " +
            "returns a Right Either.")]
        public static async Task FuncBindTLeft_Right_Left(int right, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = await value.BindT(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task FuncBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(left: _ => Either.Right<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either " +
            "returns a Left Either.")]
        public static async Task FuncBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Right Either" +
            "returns a Left Either.")]
        public static async Task TaskBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Left Either" +
            "returns a Left Either.")]
        public static async Task TaskBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static async Task TaskBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = await value.BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task TaskBindTRight_Right_Right(int right, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = await value.BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task TaskBindTLeft_Right(int right, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = await value.BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Left Either " +
            "returns a Right Either.")]
        public static async Task TaskBindTLeft_Right_Left(int right, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var actual = await value.BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either " +
            "returns a Right Either.")]
        public static async Task TaskBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel)));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either " +
            "returns a Left Either.")]
        public static async Task TaskBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var actual = await value.BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel)));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region TapT

        [Property(DisplayName = "Left-tapping a Left Either over a func returns a Left Either " +
            "and performs an action.")]
        public static async Task FuncTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = await value.TapT(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a func returns a Right Either " +
            "and performs no action.")]
        public static async Task FuncTapTLeft_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = await value.TapT(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a func returns a Left Either " +
            "and performs no action.")]
        public static async Task FuncTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = await value.TapT(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a func returns a Right Either " +
            "and performs an action.")]
        public static async Task FuncTapTRight_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = await value.TapT(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Left Either over a task returns a Left Either " +
            "and performs an action")]
        public static async Task TaskTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = await value.TapT(left: _ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a task returns a Right Either " +
            "and performs no action.")]
        public static async Task TaskTapTLeft_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = await value.TapT(left: _ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a task returns a Left Either " +
            "and performs no action.")]
        public static async Task TaskTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(left.Get);

            // act
            var output = before;
            var actual = await value.TapT(right: _ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a task returns a Right Either " +
            "and performs an action.")]
        public static async Task TaskTapTRight_Right(int right, Version before, Version sentinel)
        {
            // arrange
            var value = FromResult<Either<string, int>>(right);

            // act
            var output = before;
            var actual = await value.TapT(right: _ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        #endregion
    }
}
