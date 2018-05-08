using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to binding <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Binding an option over a null func throws.")]
        public static void FuncBind_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.Bind((Func<string, Option<int>>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Binding a None Option over a func returns a None Option.")]
        public static void FuncBind_None(Func<string, Option<int>> some) =>
            Assert.True(Option<string>.None.Bind(some).IsNone);

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option returns a None Option.")]
        public static void FuncBind_ReturnNone_Some(NonEmptyString some) =>
            Assert.True(Option.From(some.Get).Bind(_ => Option<int>.None).IsNone);

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option returns a Some Option.")]
        public static void FuncBindReturnSome_Some(NonEmptyString some)
        {
            var actual = Option.From(some.Get).Bind(v => Option.From(v.Length));

            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Property(DisplayName = "Binding an option over a null task throws.")]
        public static async Task TaskBind_Null_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.BindAsync((Func<string, Task<Option<int>>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("some", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Binding a None Option over a task returns a None Option.")]
        public static async Task TaskBind_None(Func<string, Task<Option<int>>> some)
        {
            var actual = await Option<string>.None.BindAsync(some).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option returns a None Option.")]
        public static async Task TaskBind_ReturnNone_Some(NonEmptyString some)
        {
            var actual = await Option.From(some.Get)
                .BindAsync(_ => FromResult(Option<int>.None))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option returns a Some Option.")]
        public static async Task TaskBindReturnSome_Some(NonEmptyString some)
        {
            var actual = await Option.From(some.Get)
                .BindAsync(v => FromResult(Option.From(v.Length)))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }
    }
}
