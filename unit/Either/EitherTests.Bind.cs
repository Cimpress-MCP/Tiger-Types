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
        [Property(DisplayName = "Right-Binding a Left Either over a func returns a Left Either.")]
        public static void FuncBindRight_Left(NonEmptyString left, Version sentinel)
        {
            var actual = Either.From<string, int>(left.Get)
                .Bind(right: _ => Either.From<string, Version>(sentinel));

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        public static void FuncBindRight_Right_Left(int right, NonEmptyString sentinel)
        {
            var actual = Either.From<string, int>(right)
                .Bind(right: _ => Either.From<string, Version>(sentinel.Get));

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        public static void FuncBindRight_Right_Right(int right, Version sentinel)
        {
            var actual = Either.From<string, int>(right)
                .Bind(right: _ => Either.From<string, Version>(sentinel));

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a func returns a Right Either.")]
        public static void FuncBindLeft_Right(int right, Guid sentinel)
        {
            var actual = Either.From<string, int>(right)
                .Bind(left: _ => Either.From<Guid, int>(sentinel));

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        public static void FuncBindLeft_Left_Right(NonEmptyString left, int sentinel)
        {
            var actual = Either.From<string, int>(left.Get)
                .Bind(left: _ => Either.From<Guid, int>(sentinel));

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        public static void FuncBindLeft_Left_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = Either.From<string, int>(left.Get)
                .Bind(left: _ => Either.From<Guid, int>(sentinel));

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Left Either over a task returns a Left Either.")]
        public static async Task TaskBindRight_Left(NonEmptyString left, Version sentinel)
        {
            var actual = await Either.From<string, int>(left.Get)
                .BindAsync(right: _ => FromResult(Either.From<string, Version>(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task TaskBindRight_Right_Left(int right, NonEmptyString sentinel)
        {
            var actual = await Either.From<string, int>(right)
                .BindAsync(right: _ => FromResult(Either.From<string, Version>(sentinel.Get)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Right-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task TaskBindRight_Right_Right(int right, Version sentinel)
        {
            var actual = await Either.From<string, int>(right)
                .BindAsync(right: _ => FromResult(Either.From<string, Version>(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Right Either over a task returns a Right Either.")]
        public static async Task TaskBindLeft_Right(int right, Guid sentinel)
        {
            var actual = await Either.From<string, int>(right)
                .BindAsync(left: _ => FromResult(Either.From<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task TaskBindLeft_Left_Right(NonEmptyString left, int sentinel)
        {
            var actual = await Either.From<string, int>(left.Get)
                .BindAsync(left: _ => FromResult(Either.From<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Left-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task TaskBindLeft_Left_Left(NonEmptyString left, Guid sentinel)
        {
            var actual = await Either.From<string, int>(left.Get)
                .BindAsync(left: _ => FromResult(Either.From<Guid, int>(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        public static void FuncFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(right).Bind(
                left: _ => Either.From<Guid, Version>(leftSentinel),
                right: _ => Either.From<Guid, Version>(rightSentinel));

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        public static void FuncFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(right).Bind(
                left: _ => Either.From<Guid, Version>(rightSentinel),
                right: _ => Either.From<Guid, Version>(leftSentinel));

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        public static void FuncFuncBindBoth_Left_Right(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(left.Get).Bind(
                left: _ => Either.From<Guid, Version>(rightSentinel),
                right: _ => Either.From<Guid, Version>(leftSentinel));

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        public static void FuncFuncBindBoth_Left_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = Either.From<string, int>(left.Get).Bind(
                left: _ => Either.From<Guid, Version>(leftSentinel),
                right: _ => Either.From<Guid, Version>(rightSentinel));

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Right Either returns a Right Either.")]
        public static async Task TaskFuncBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(leftSentinel)),
                right: _ => Either.From<Guid, Version>(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a func that returns a Left Either returns a Left Either.")]
        public static async Task TaskFuncBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(rightSentinel)),
                right: _ => Either.From<Guid, Version>(leftSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task TaskFuncBindBoth_Left_Right(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(rightSentinel)),
                right: _ => Either.From<Guid, Version>(leftSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task TaskFuncBindBoth_Left_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(leftSentinel)),
                right: _ => Either.From<Guid, Version>(rightSentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task FuncTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => Either.From<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task FuncTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => Either.From<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Right Either returns a Right Either.")]
        public static async Task FuncTaskBindBoth_Left_Right(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => Either.From<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a func that returns a Left Either returns a Left Either.")]
        public static async Task FuncTaskBindBoth_Left_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => Either.From<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task TaskTaskBindBoth_Right_Right(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => Either.From<Guid, Version>(leftSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Right Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task TaskTaskBindBoth_Right_Left(int right, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(right).BindAsync(
                left: _ => Either.From<Guid, Version>(rightSentinel),
                right: _ => FromResult(Either.From<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Right Either returns a Right Either.")]
        public static async Task TaskTaskBindBoth_Left_Right(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(rightSentinel)),
                right: _ => FromResult(Either.From<Guid, Version>(leftSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (Version)actual;
            Assert.Equal(rightSentinel, innerValue);
        }

        [Property(DisplayName = "Bi-Binding a Left Either over a task that returns a Left Either returns a Left Either.")]
        public static async Task TaskTaskBindBoth_Left_Left(NonEmptyString left, Guid leftSentinel, Version rightSentinel)
        {
            var actual = await Either.From<string, int>(left.Get).BindAsync(
                left: _ => FromResult(Either.From<Guid, Version>(leftSentinel)),
                right: _ => FromResult(Either.From<Guid, Version>(rightSentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(leftSentinel, innerValue);
        }
    }
}
