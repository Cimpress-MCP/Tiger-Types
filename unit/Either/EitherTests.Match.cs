using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to matching <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right func branch.")]
        public static void FuncFuncMatchReturn_Left(NonEmptyString left)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left func branch.")]
        public static void FuncFuncMatchReturn_Right(int right)
        {
            var value = Either.From<string, int>(right);

            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left func branch, not the Right task branch.")]
        public static async Task FuncTaskMatchReturn_Left(NonEmptyString left)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = await value.MatchAsync(
                left: l => l.Length,
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right task branch, not the Left func branch.")]
        public static async Task FuncTaskMatchReturn_Right(int right)
        {
            var value = Either.From<string, int>(right);

            var actual = await value.MatchAsync(
                left: l => l.Length,
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        public static async Task TaskFuncMatchReturn_Left(NonEmptyString left)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = await value.MatchAsync(
                left: l => l.Length.Pipe(FromResult),
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        public static async Task TaskFuncMatchReturn_Right(int right)
        {
            var value = Either.From<string, int>(right);

            var actual = await value.MatchAsync(
                left: l => l.Length.Pipe(FromResult),
                right: r => r)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either returns the Left task branch, not the Right func branch.")]
        public static async Task TaskTaskMatchReturn_Left(NonEmptyString left)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = await value.MatchAsync(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(left.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a Right Either returns the Right func branch, not the Left task branch.")]
        public static async Task TaskTaskMatchReturn_Right(int right)
        {
            var value = Either.From<string, int>(right);

            var actual = await value.MatchAsync(
                left: l => l.Length.Pipe(FromResult),
                right: FromResult)
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, not the Right action branch.")]
        public static void ActionActionMatchVoid_Left(
            NonEmptyString left,
            NonEmptyString before,
            NonEmptyString sentinel)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = before.Get;
            var unit = value.Match(
                left: _ => actual = sentinel.Get,
                right: _ => { });

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, not the Left action branch.")]
        public static void ActionActionMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.From<string, int>(right);

            var actual = before;
            var unit = value.Match(
                left: _ => { },
                right: _ => actual = sentinel);

            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left action branch, not the Right task branch.")]
        public static async Task ActionTaskMatchVoid_Left(
            NonEmptyString left,
            NonEmptyString before,
            NonEmptyString sentinel)
        {
            var value = Either.From<string, int>(left.Get);

            var actual = before.Get;
            await value.MatchAsync(
                left: _ => actual = sentinel.Get,
                right: _ => CompletedTask).ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, not the Left action branch.")]
        public static async Task ActionTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.From<string, int>(right);

            var actual = before;
            await value.MatchAsync(
                left: _ => { },
                right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, not the Right action branch.")]
        public static async Task TaskActionMatchVoid_Left(NonEmptyString left, NonEmptyString before, NonEmptyString sentinel)
        {
            var value = Either.From<string, int>(left.Get);

            // act
            var actual = before.Get;
            await value.MatchAsync(
                left: _ => Run(() => actual = sentinel.Get),
                right: _ => { }).ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right action branch, not the Left task branch.")]
        public static async Task TaskActionMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.From<string, int>(right);

            // act
            var actual = before;
            await value.MatchAsync(
                left: _ => CompletedTask,
                right: _ => actual = sentinel).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Matching a Left Either executes the Left task branch, not the Right task branch.")]
        public static async Task TaskTaskMatchVoid_Left(NonEmptyString left, NonEmptyString before, NonEmptyString sentinel)
        {
            var value = Either.From<string, int>(left.Get);

            // act
            var actual = before.Get;
            await value.MatchAsync(
                left: _ => Run(() => actual = sentinel.Get),
                right: _ => CompletedTask).ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Right Either executes the Right task branch, not the Left task branch.")]
        public static async Task TaskTaskMatchVoid_Right(int right, int before, int sentinel)
        {
            var value = Either.From<string, int>(right);

            // act
            var actual = before;
            await value.MatchAsync(
                left: _ => CompletedTask,
                right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }
    }
}
