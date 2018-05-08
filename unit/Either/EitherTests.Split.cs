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
        [Property(DisplayName = "Splitting a value over a func, failing the condition, returns a Left Either.")]
        public static void FuncSplit_Left(NonEmptyString value)
        {
            var actual = Either.Split(value.Get, _ => false);

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Right Either.")]
        public static void FuncSplit_Right(NonEmptyString value)
        {
            var actual = Either.Split(value.Get, _ => true);

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a task, failing the condition, returns a Left Either.")]
        public static async Task TaskSplit_Left(NonEmptyString value)
        {
            var actual = await Either.SplitAsync(value.Get, _ => FromResult(false)).ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            Assert.False(actual.IsRight);
        }

        [Property(DisplayName = "Splitting a value over a task, passing the condition, returns a Right Either.")]
        public static async Task TaskSplit_Right(NonEmptyString value)
        {
            var actual = await Either.SplitAsync(value.Get, _ => FromResult(true)).ConfigureAwait(false);

            Assert.False(actual.IsLeft);
            Assert.True(actual.IsRight);
        }
    }
}
