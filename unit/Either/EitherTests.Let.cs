using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Left-conditionally executing an action based on a Left Either executes.")]
        static void ActionLetLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Left<string, int>(left.Get).Let(left: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing an action based on a Right Either " +
                                "does not execute.")]
        static void ActionLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Right<string, int>(right).Let(left: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Left Either does not execute.")]
        static void ActionLetRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Left<string, int>(left.Get).Let(right: _ => actual = sentinel);

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing an action based on a Right Either executes.")]
        static void ActionLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            Either.Right<string, int>(right).Let(right: _ => actual = sentinel);

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing a task based on a Left Either executes.")]
        static async Task TaskLetLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Left<string, int>(left.Get).Let(left: _ => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }

        [Property(DisplayName = "Left-conditionally executing a Task based on a Right Either does not execute.")]
        static async Task TaskLetLeft_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Right<string, int>(right).Let(left: _ => Run(() => actual = sentinel));

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Left Either does not execute.")]
        static async Task TaskLetRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Left<string, int>(left.Get).Let(right: _ => Run(() => actual = sentinel));

            Assert.Equal(before, actual);
        }

        [Property(DisplayName = "Right-conditionally executing a Task based on a Right Either executes.")]
        static async Task TaskLetRight_Right(int right, Guid before, Guid sentinel)
        {
            var actual = before;
            await Either.Right<string, int>(right).Let(right: _ => Run(() => actual = sentinel));

            Assert.Equal(sentinel, actual);
        }
    }
}
