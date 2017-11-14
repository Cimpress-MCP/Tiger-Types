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

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right func branch.")]
        static async Task FuncFuncMatchTReturn_Left(NonNull<string> left)
        {
            var actual = await FromResult<Either<string, int>>(left.Get).MatchT(
                left: l => l.Length,
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left func branch.")]
        static async Task FuncFuncMatchTReturn_Right(Version right)
        {
            var actual = await FromResult<Either<int, Version>>(right).MatchT(
                left: l => l,
                right: r => r.Major)
                .ConfigureAwait(false);

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right task branch.")]
        static async Task FuncTaskMatchTReturn_Left(NonNull<string> left)
        {
            var actual = await FromResult<Either<string, int>>(left.Get).MatchT(
                left: l => l.Length,
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, not the Left func branch.")]
        static async Task FuncTaskMatchTReturn_Right(Version right)
        {
            var actual = await FromResult<Either<int, Version>>(right).MatchT(
                left: l => l,
                right: r => r.Major.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        static async Task TaskFuncMatchTReturn_Left(NonNull<string> left)
        {
            var actual = await FromResult<Either<string, int>>(left.Get).MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        static async Task TaskFuncMatchTReturn_Right(Version right)
        {
            var actual = await FromResult<Either<int, Version>>(right).MatchT(
                left: FromResult,
                right: r => r.Major)
                .ConfigureAwait(false);

            Assert.Equal(right.Major, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        static async Task TaskTaskMatchTReturn_Left(NonNull<string> left)
        {
            var actual = await FromResult<Either<string, int>>(left.Get).MatchT(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        static async Task TaskTaskMatchTReturn_Right(Version right)
        {
            var actual = await FromResult<Either<int, Version>>(right).MatchT(
                left: FromResult,
                right: r => r.Major.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.Equal(right.Major, actual);
        }

        #endregion

        #region MapT

        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        static async Task FuncMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .MapT(left: _ => sentinel.Get)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        static async Task FuncMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .MapT(left: _ => sentinel.Get)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskMapTLeft_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .MapT(left: _ => FromResult(sentinel.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        static async Task TaskMapTLeft_Right(int right, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .MapT(left: _ => FromResult(sentinel.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        static async Task FuncMapTRight_Left(NonNull<string> left, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .MapT(right: _ => sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        static async Task FuncMapTRight_Right(int right, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .MapT(right: _ => sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskMapTRight_Left(NonNull<string> left, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .MapT(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        static async Task TaskMapTRight_Right(int right, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .MapT(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region BindT

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Right Either returns a Left Either.")]
        static async Task FuncBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(right: _ => Either.Right<string, Version>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        static async Task FuncBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(right: _ => Either.Left<string, Version>(sentinel.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        static async Task FuncBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(right: _ => Either.Left<string, Version>(sentinel.Get))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        static async Task FuncBindTRight_Right_Right(int right, Guid sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(right: _ => Either.Right<string, Guid>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        static async Task FuncBindTLeft_Right_Right(int right, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(left: _ => Either.Right<Guid, int>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func that returns a Left Either returns a Right Either.")]
        static async Task FuncBindTLeft_Right_Left(int right, Guid sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(left: _ => Either.Left<Guid, int>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        static async Task FuncBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(left: _ => Either.Right<Guid, int>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        static async Task FuncBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(left: _ => Either.Left<Guid, int>(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Right Either returns a Left Either.")]
        static async Task TaskBindTRight_Left_Right(NonNull<string> left, Version sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskBindTRight_Left_Left(NonNull<string> left, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskBindTRight_Right_Left(int right, NonNull<string> sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskBindTRight_Right_Right(int right, Version sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(right: _ => FromResult(Either.Right<string, Version>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskBindTLeft_Right(int right, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task that returns a Left Either returns a Right Either.")]
        static async Task TaskBindTLeft_Right_Left(int right, Guid sentinel)
        {
            var actual = await FromResult<Either<string, int>>(right)
                .BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskBindTLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(left: _ => FromResult(Either.Right<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskBindTLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = await FromResult<Either<string, int>>(left.Get)
                .BindT(left: _ => FromResult(Either.Left<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region TapT

        [Property(DisplayName = "Left-tapping a Left Either over a func returns a Left Either and performs an action.")]
        static async Task FuncTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(left.Get)
                .TapT(left: _ => output = sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a func returns a Right Either and performs no action.")]
        static async Task FuncTapTLeft_Right(int right, Version before, Version sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(right)
                .TapT(left: _ => output = sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a func returns a Left Either and performs no action.")]
        static async Task FuncTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(left.Get)
                .TapT(right: _ => output = sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a func returns a Right Either and performs an action.")]
        static async Task FuncTapTRight_Right(int right, Version before, Version sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(right)
                .TapT(right: _ => output = sentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Left Either over a task returns a Left Either and performs an action")]
        static async Task TaskTapTLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(left.Get)
                .TapT(left: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, sentinel);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a task returns a Right Either and performs no action.")]
        static async Task TaskTapTLeft_Right(int right, Version before, Version sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(right)
                .TapT(left: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a task returns a Left Either and performs no action.")]
        static async Task TaskTapTRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(left.Get)
                .TapT(right: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(output, before);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a task returns a Right Either and performs an action.")]
        static async Task TaskTapTRight_Right(int right, Version before, Version sentinel)
        {
            var output = before;
            var actual = await FromResult<Either<string, int>>(right)
                .TapT(right: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(output, sentinel);
        }

        #endregion
    }
}
