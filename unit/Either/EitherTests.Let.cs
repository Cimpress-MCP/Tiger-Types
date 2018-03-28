using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Left-conditionally executing an action based on a Left Either executes.")]
        public static void ActionLetLeft_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Left<string, int>(left.Get).Let(left: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing an action based on a Right Either " +
                                "does not execute.")]
        public static void ActionLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Right<string, int>(right).Let(left: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Left Either does not execute.")]
        public static void ActionLetRight_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Left<string, int>(left.Get).Let(right: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Right Either executes.")]
        public static void ActionLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Right<string, int>(right).Let(right: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing a task based on a Left Either executes.")]
        public static async Task TaskLetLeft_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Left<string, int>(left.Get).LetAsync(left: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing a Task based on a Right Either does not execute.")]
        public static async Task TaskLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Right<string, int>(right).LetAsync(left: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Left Either does not execute.")]
        public static async Task TaskLetRight_Left(NonEmptyString left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Left<string, int>(left.Get).LetAsync(right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Right Either executes.")]
        public static async Task TaskLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Right<string, int>(right).LetAsync(right: _ => Run(() => actual = sentinel)).ConfigureAwait(false);

            Assert.Equal(sentinel, actual);
        }
    }
}
