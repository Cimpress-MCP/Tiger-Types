using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to mapping <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Mapping a None Option over a null func throws.")]
        public static void FuncMap_None_Null_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Map((Func<string, int>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public static void FuncMap_None()
        {
            var actual = Option<string>.None.Map(v => v.Length);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a null func throws.")]
        public static void FuncMap_Some_Null_Throws(NonEmptyString some)
        {
            var actual = Record.Exception(() => Option.From(some.Get).Map((Func<string, int>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public static void FuncMap_Some(NonEmptyString some)
        {
            var actual = Option.From(some.Get).Map(v => v.Length);

            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a null task throws.")]
        public static async Task TaskMap_None_Null_Throws()
        {
            var actual = await Record.ExceptionAsync(() => Option<string>.None
                .MapAsync((Func<string, Task<int>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public static async Task TaskMap_None()
        {
            var actual = await Option<string>.None
                .MapAsync(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a null task throws.")]
        public static async Task TaskMap_Some_Null_Throws(NonEmptyString some)
        {
            var actual = await Record.ExceptionAsync(() => Option.From(some.Get)
                .MapAsync((Func<string, Task<int>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public static async Task TaskMap_Some(NonEmptyString some)
        {
            var actual = await Option.From(some.Get)
                .MapAsync(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }
    }
}
