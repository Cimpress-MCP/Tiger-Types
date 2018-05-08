using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to splitting <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Splitting a value over a null func throws.")]
        public static void FuncSplit_NullSplitter_Throws(string from)
        {
            var actual = Record.Exception(() => Option.Split(from, splitter: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("splitter", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Splitting a null value over a func returns a None Option.")]
        public static void FuncSplit_Null_None(Func<string, bool> splitter) => Assert.True(Option.Split(null, splitter).IsNone);

        [Property(DisplayName = "Splitting a nullable value over a null func throws.")]
        public static void FuncSplit_Nullable_NullSplitter_Throws(int? some)
        {
            var actual = Record.Exception(() => Option.Split(some, splitter: (Func<int, bool>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("splitter", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Splitting a null nullable value over a func returns a None Option.")]
        public static void FuncSplit_NullableNull_None(Func<int, bool> splitter) =>
            Assert.True(Option.Split(null, splitter).IsNone);

        [Property(DisplayName = "Splitting a value over a func, failing the condition, returns a None Option.")]
        public static void FuncSplit_ReturnFalse_None(NonEmptyString some) =>
            Assert.True(Option.Split(some.Get, _ => false).IsNone);

        [Property(DisplayName = "Splitting a nullable value over a func, failing the condition, returns a None Option.")]
        public static void FuncSplit_NullableReturnFalse_None(int some) =>
            Assert.True(Option.Split((int?)some, (int _) => false).IsNone);

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Some Option.")]
        public static void FuncSplit_Some(NonEmptyString some)
        {
            var actual = Option.Split(some.Get, _ => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, returns a Some Option.")]
        public static void FuncSplit_Nullable_Some(int some)
        {
            var actual = Option.Split((int?)some, (int _) => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Splitting a value over a null task throws.")]
        public static async Task TaskSplit_NullSplitter_Throws(string from)
        {
            var actual = await Record.ExceptionAsync(() => Option
                .SplitAsync(from, splitter: null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("splitter", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Splitting a null value over a task returns a None Option.")]
        public static async Task TaskSplit_Null_None(Func<string, Task<bool>> splitter)
        {
            var actual = await Option.SplitAsync(null, splitter).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a null task throws.")]
        public static async Task TaskSplit_Nullable_NullSplitter_Throws(int? from)
        {
            var actual = await Record.ExceptionAsync(() => Option
                .SplitAsync(from, splitter: (Func<int, Task<bool>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("splitter", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Splitting a nullable null value over a task returns a None Option.")]
        public static async Task TaskSplit_NullableNull_None(Func<int, Task<bool>> splitter)
        {
            var actual = await Option.SplitAsync(null, splitter).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a task, failing the condition, returns a None Option.")]
        public static async Task TaskSplit_ReturnFalse_None(NonEmptyString some)
        {
            var actual = await Option.SplitAsync(some.Get, _ => FromResult(false)).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a task, failing the condition, returns a None Option.")]
        public static async Task TaskSplit_NullableReturnFalse_None(int some)
        {
            var actual = await Option.SplitAsync((int?)some, (int _) => FromResult(false)).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Some Option.")]
        public static async Task TaskSplit_Some(NonEmptyString some)
        {
            var actual = await Option.SplitAsync(some.Get, _ => FromResult(true)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, returns a Some Option.")]
        public static async Task TaskSplit_Nullable_Some(int some)
        {
            var actual = await Option.SplitAsync((int?)some, (int _) => FromResult(true)).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }
    }
}
