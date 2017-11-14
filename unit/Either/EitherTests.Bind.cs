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
        [Property(DisplayName = "Right-Binding a Left Either over a func returns a Left Either.")]
        static void FuncBindRight_Left(NonNull<string> left, Version sentinel)
        {
            var actual = Either.Left<string, int>(left.Get)
                .Bind(right: _ => Either.Right<string, Version>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        static void FuncBindRight_Right_Left(int right, NonNull<string> sentinel)
        {
            var actual = Either.Right<string, int>(right)
                .Bind(right: _ => Either.Left<string, Version>(sentinel.Get));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        static void FuncBindRight_Right_Right(int right, Version sentinel)
        {
            var actual = Either.Right<string, int>(right)
                .Bind(right: _ => Either.Right<string, Version>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func returns a Right Either.")]
        static void FuncBindLeft_Right(int right, Guid sentinel)
        {
            var actual = Either.Right<string, int>(right)
                .Bind(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        static void FuncBindLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            var actual = Either.Left<string, int>(left.Get)
                .Bind(left: _ => Either.Right<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        static void FuncBindLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = Either.Left<string, int>(left.Get)
                .Bind(left: _ => Either.Left<Guid, int>(sentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task returns a Left Either.")]
        static async Task TaskBindRight_Left(NonNull<string> left, Version sentinel)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Bind(right: _ => FromResult(Either.Right<string, Version>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskBindRight_Right_Left(int right, NonNull<string> sentinel)
        {
            var actual = await Either.Right<string, int>(right)
                .Bind(right: _ => FromResult(Either.Left<string, Version>(sentinel.Get)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskBindRight_Right_Right(int right, Version sentinel)
        {
            var actual = await Either.Right<string, int>(right)
                .Bind(right: _ => FromResult(Either.Right<string, Version>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task returns a Right Either.")]
        static async Task TaskBindLeft_Right(int right, Guid sentinel)
        {
            var actual = await Either.Right<string, int>(right)
                .Bind(left: _ => FromResult(Either.Left<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskBindLeft_Left_Right(NonNull<string> left, int sentinel)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Bind(left: _ => FromResult(Either.Right<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskBindLeft_Left_Left(NonNull<string> left, Guid sentinel)
        {
            var actual = await Either.Left<string, int>(left.Get)
                .Bind(left: _ => FromResult(Either.Left<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        static void FuncFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Right<string, int>(right).Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => Either.Right<Guid, Version>(rightSentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        static void FuncFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Right<string, int>(right).Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => Either.Left<Guid, Version>(leftSentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        static void FuncFuncBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Left<string, int>(left.Get).Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => Either.Left<Guid, Version>(leftSentinel));

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        static void FuncFuncBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.Left<string, int>(left.Get).Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => Either.Right<Guid, Version>(rightSentinel));

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        static async Task TaskFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => Either.Right<Guid, Version>(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        static async Task TaskFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => Either.Left<Guid, Version>(leftSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskFuncBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => Either.Left<Guid, Version>(leftSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskFuncBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => Either.Right<Guid, Version>(rightSentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        static async Task FuncTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        static async Task FuncTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        static async Task FuncTaskBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        static async Task FuncTaskBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => Either.Left<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Right<string, int>(right).Bind(
                left: _ => Either.Right<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        static async Task TaskTaskBindBoth_Left_Right(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)),
                right: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        static async Task TaskTaskBindBoth_Left_Left(NonNull<string> left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.Left<string, int>(left.Get).Bind(
                left: _ => FromResult(Either.Left<Guid, Version>(leftSentinel)),
                right: _ => FromResult(Either.Right<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }
    }
}
