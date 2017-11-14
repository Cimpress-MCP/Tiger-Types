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
        [Property(DisplayName = "Left-tapping a Left Either over a func returns a Left Either and performs an action")]
        static void FuncTapLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Left<string, int>(left.Get).Tap(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a func returns a Right Either and performs no action.")]
        static void FuncTapLeft_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Right<string, int>(right).Tap(left: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a func returns a Left Either and performs no action.")]
        static void FuncTapRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Left<string, int>(left.Get).Tap(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a func returns a Right Either and performs an action.")]
        static void FuncTapRight_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = Either.Right<string, int>(right).Tap(right: _ => output = sentinel);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Left Either over a task returns a Left Either and perform an action")]
        static async Task TaskTapLeft_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await Either.Left<string, int>(left.Get)
                .Tap(left: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Left-tapping a Right Either over a task returns a Right Either and perform no action.")]
        static async Task TaskTapLeft_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await Either.Right<string, int>(right)
                .Tap(left: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Left Either over a task returns a Left Either and perform no action.")]
        static async Task TaskTapRight_Left(NonNull<string> left, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await Either.Left<string, int>(left.Get)
                .Tap(right: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(left.Get, innerValue);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Right-tapping a Right Either over a task returns a Right Either and perform an action.")]
        static async Task TaskTapRight_Right(int right, Guid before, Guid sentinel)
        {
            var output = before;
            var actual = await Either.Right<string, int>(right)
                .Tap(right: _ => Run(() => output = sentinel))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsRight);
            var innerValue = (int)actual;
            Assert.Equal(right, innerValue);
            Assert.Equal(sentinel, output);
        }
    }
}