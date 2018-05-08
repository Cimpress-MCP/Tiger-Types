using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Attempting to left-conditionally execute a null action throws.")]
        public static void ActionLetLeft_Null_Throws(Either<string, int> either)
        {
            var actual = Record.Exception(() => either.Let(left: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Left-conditionally executing an action based on a Left Either executes.")]
        public static void ActionLetLeft_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.From<string, int>(left.Get).Let(left: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing an action based on a Right Either " +
                                "does not execute.")]
        public static void ActionLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.From<string, int>(right).Let(left: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Attempting to right-conditionally execute a null action throws.")]
        public static void ActionLetRight_Null_Throws(Either<string, int> either)
        {
            var actual = Record.Exception(() => either.Let(right: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Left Either does not execute.")]
        public static void ActionLetRight_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.From<string, int>(left.Get).Let(right: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Right Either executes.")]
        public static void ActionLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.From<string, int>(right).Let(right: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Attempting to left-conditionally execute a null task throws.")]
        public static async Task TaskLetLeft_Null_Throws(Either<string, int> either)
        {
            var actual = await Record.ExceptionAsync(() => either.LetAsync(left: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Left-conditionally executing a task based on a Left Either executes.")]
        public static async Task TaskLetLeft_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.From<string, int>(left.Get).LetAsync(left: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing a Task based on a Right Either does not execute.")]
        public static async Task TaskLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.From<string, int>(right).LetAsync(left: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Attempting to right-conditionally execute a null action throws.")]
        public static async Task TaskLetRight_Null_Throws(Either<string, int> either)
        {
            var actual = await Record.ExceptionAsync(() => either.LetAsync(right: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Left Either does not execute.")]
        public static async Task TaskLetRight_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.From<string, int>(left.Get).LetAsync(right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Right Either executes.")]
        public static async Task TaskLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.From<string, int>(right).LetAsync(right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }
    }
}
