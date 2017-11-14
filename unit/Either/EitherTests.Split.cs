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
        [Property(DisplayName = "Splitting a value over a func, failing the condition, returns a Left Either.")]
        static void FuncSplit_Left(NonNull<string> value)
        {
            var actual = Either.Split(value.Get, _ => false);

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Right Either.")]
        static void FuncSplit_Right(NonNull<string> value)
        {
            var actual = Either.Split(value.Get, _ => true);

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a task, failing the condition, returns a Left Either.")]
        static async Task TaskSplit_Left(NonNull<string> value)
        {
            var actual = await Either.Split(value.Get, _ => FromResult(false)).ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a task, passing the condition, returns a Right Either.")]
        static async Task TaskSplit_Right(NonNull<string> value)
        {
            var actual = await Either.Split(value.Get, _ => FromResult(true)).ConfigureAwait(false);

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }
    }
}
