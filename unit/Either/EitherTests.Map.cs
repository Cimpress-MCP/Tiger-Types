using System;
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
        [Property(DisplayName = "Left-Mapping a Left Either over a func returns a Left Either.")]
        static void FuncMapLeft_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = Either.Left<string, int>(left.Get).Map(left: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a func returns a Right Either.")]
        static void FuncMapLeft_Right(int right, Guid sentinel)
        {
            var actual = Either.Right<string, int>(right).Map(left: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskMapLeft_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Map(left: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Mapping a Right Either over a task returns a Right Either.")]
        static async Task TaskMapLeft_Right(int right, Guid sentinel)
        {
            var actual = await Either.Right<string, int>(right)
                .Map(left: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a func returns a Left Either.")]
        static void FuncMapRight_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = Either.Left<string, int>(left.Get)
                .Map(right: _ => sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a func returns a Right Either.")]
        static void FuncMapRight_Right(int right, Guid sentinel)
        {
            var actual = Either.Right<string, int>(right)
                .Map(right: _ => sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskMapRight_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Map(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Mapping a Right Either over a task returns a Right Either.")]
        static async Task TaskMapRight_Right(int right, Guid sentinel)
        {
            var actual = await Either.Right<string, int>(right)
                .Map(right: _ => FromResult(sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        static void FuncFuncMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Left<string, int>(left.Get).Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        static void FuncFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Right<string, int>(right).Map(
                left: _ => leftSentinel,
                right: _ => rightSentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a func returns a Left Either.")]
        static async Task FuncTaskMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Map(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        static async Task FuncTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Map(
                left: _ => FromResult(leftSentinel),
                right: _ => rightSentinel)
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskFuncMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Map(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a func returns a Right Either.")]
        static async Task TaskFuncMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Map(
                left: _ => leftSentinel,
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Left Either over a task returns a Left Either.")]
        static async Task TaskTaskMap_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Map(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Mapping a Right Either over a task returns a Right Either.")]
        static async Task TaskTaskMap_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Map(
                left: _ => FromResult(leftSentinel),
                right: _ => FromResult(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }
    }
}
