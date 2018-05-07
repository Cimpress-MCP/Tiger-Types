using System;
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
        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapLeft_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = Either.From<string, int>(left.Get).Map(left: _ => sentinel);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapLeft_Right(int right, Guid sentinel)
        {
            var actual = Either.From<string, int>(right).Map(left: _ => sentinel);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskMapLeft_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = await Either.From<string, int>(left.Get)
                .MapAsync(left: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        public static async Task TaskMapLeft_Right(int right, Guid sentinel)
        {
            var actual = await Either.From<string, int>(right)
                .MapAsync(left: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncMapRight_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = Either.From<string, int>(left.Get)
                .Map(right: _ => sentinel);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncMapRight_Right(int right, Guid sentinel)
        {
            var actual = Either.From<string, int>(right)
                .Map(right: _ => sentinel);

            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskMapRight_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = await Either.From<string, int>(left.Get)
                .MapAsync(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        public static async Task TaskMapRight_Right(int right, Guid sentinel)
        {
            var actual = await Either.From<string, int>(right)
                .MapAsync(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public static void FuncFuncMap_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(left.Get).Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public static void FuncFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(right).Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        public static async Task FuncTaskMap_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).MapAsync(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel)
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public static async Task FuncTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).MapAsync(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel)
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskFuncMap_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).MapAsync(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        public static async Task TaskFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).MapAsync(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        public static async Task TaskTaskMap_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).MapAsync(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        public static async Task TaskTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).MapAsync(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }
    }
}
