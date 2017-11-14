using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to matching <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right func branch.")]
        static void FuncFuncMatchReturn_Left(NonNull<string> left)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left func branch.")]
        static void FuncFuncMatchReturn_Right(int right)
        {
            var value = Either.Right<string, int>(right);

            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right task branch.")]
        static async Task FuncTaskMatchReturn_Left(NonNull<string> left)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = await value.Match(
                left: l => l.Length,
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, not the Left func branch.")]
        static async Task FuncTaskMatchReturn_Right(int right)
        {
            var value = Either.Right<string, int>(right);

            var actual = await value.Match(
                left: l => l.Length,
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        static async Task TaskFuncMatchReturn_Left(NonNull<string> left)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        static async Task TaskFuncMatchReturn_Right(int right)
        {
            var value = Either.Right<string, int>(right);

            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        static async Task TaskTaskMatchReturn_Left(NonNull<string> left)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        static async Task TaskTaskMatchReturn_Right(int right)
        {
            var value = Either.Right<string, int>(right);

            var actual = await value.Match(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, not the Right action branch.")]
        static void ActionActionMatchVoid_Left(
            NonNull<string> left,
            NonNull<string> before,
            NonNull<string> sentinel)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = before.Get;
            var unit = value.Match(
                left: l => actual = sentinel.Get,
                right: r => { });

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, not the Left action branch.")]
        static void ActionActionMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.Right<string, int>(right);

            var actual = before;
            var unit = value.Match(
                left: l => { },
                right: r => actual = sentinel);

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, not the Right task branch.")]
        static async Task ActionTaskMatchVoid_Left(
            NonNull<string> left,
            NonNull<string> before,
            NonNull<string> sentinel)
        {
            var value = Either.Left<string, int>(left.Get);

            var actual = before.Get;
            await value.Match(
                left: l => actual = sentinel.Get,
                right: r => CompletedTask);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, not the Left action branch.")]
        static async Task ActionTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.Right<string, int>(right);

            var actual = before;
            await value.Match(
                left: l => { },
                right: r => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, not the Right action branch.")]
        static async Task TaskActionMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            await value.Match(
                left: l => Run(() => actual = sentinel.Get),
                right: r => { });

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, not the Left task branch.")]
        static async Task TaskActionMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            await value.Match(
                left: l => CompletedTask,
                right: r => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, not the Right task branch.")]
        static async Task TaskTaskMatchVoid_Left(NonNull<string> left, NonNull<string> before, NonNull<string> sentinel)
        {
            var value = Either.Left<string, int>(left.Get);

            // act
            var actual = before.Get;
            await value.Match(
                left: l => Run(() => actual = sentinel.Get),
                right: r => CompletedTask);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, not the Left task branch.")]
        static async Task TaskTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.Right<string, int>(right);

            // act
            var actual = before;
            await value.Match(
                left: l => CompletedTask,
                right: r => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }
    }
}
