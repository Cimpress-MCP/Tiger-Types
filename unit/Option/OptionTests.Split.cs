using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to splitting <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Splitting a null value over a func returns a None Option.")]
        static void FuncSplit_Null_None(bool split)
        {
            var actual = Option.Split(null, (string _) => split);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a null nullable value over a func returns a None Option.")]
        static void FuncSplit_NullableNull_None(bool split)
        {
            var actual = Option.Split(null, (int _) => split);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, failing the condition, returns a None Option.")]
        static void FuncSplit_ReturnFalse_None(NonNull<string> some)
        {
            var actual = Option.Split(some.Get, _ => false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, failing the condition, returns a None Option.")]
        static void FuncSplit_NullableReturnFalse_None(int some)
        {
            var actual = Option.Split((int?)some, (int _) => false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Some Option.")]
        static void FuncSplit_Some(NonNull<string> some)
        {
            var actual = Option.Split(some.Get, _ => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, returns a Some Option.")]
        static void FuncSplit_Nullable_Some(int some)
        {
            var actual = Option.Split((int?)some, (int _) => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Splitting a null value over a task returns a None Option.")]
        static async Task TaskSplit_Null_None(bool split)
        {
            var actual = await Option.Split(null, (string _) => FromResult(split))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable null value over a task returns a None Option.")]
        static async Task TaskSplit_NullableNull_None(bool split)
        {
            var actual = await Option.Split(null, (int _) => FromResult(split))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a task, failing the condition, returns a None Option.")]
        static async Task TaskSplit_ReturnFalse_None(NonNull<string> some)
        {
            var actual = await Option.Split(some.Get, _ => FromResult(false))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a task, failing the condition, returns a None Option.")]
        static async Task TaskSplit_NullableReturnFalse_None(int some)
        {
            var actual = await Option.Split((int?)some, (int _) => FromResult(false))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Some Option.")]
        static async Task TaskSplit_Some(NonNull<string> some)
        {
            var actual = await Option.Split(some.Get, _ => FromResult(true))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, returns a Some Option.")]
        static async Task TaskSplit_Nullable_Some(int some)
        {
            var actual = await Option.Split((int?)some, (int _) => FromResult(true))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }
    }
}
