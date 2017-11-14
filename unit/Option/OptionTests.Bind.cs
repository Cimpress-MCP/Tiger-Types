using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to binding <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Binding a None Option over a func returning a None Option returns a None Option.")]
        static void FuncBind_ReturnNone_None()
        {
            var actual = Option<string>.None.Bind(_ => Option<int>.None);

            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Binding a None Option over a func returning a Some Option returns a None Option.")]
        static void FuncBind_ReturnSome_None()
        {
            var actual = Option<string>.None.Bind(v => Option.From(v.Length));

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option returns a None Option.")]
        static void FuncBind_ReturnNone_Some(NonNull<string> some)
        {
            var actual = Option.From(some.Get).Bind(_ => Option<int>.None);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option returns a Some Option.")]
        static void FuncBindReturnSome_Some(NonNull<string> some)
        {
            var actual = Option.From(some.Get).Bind(v => Option.From(v.Length));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option returns a None Option.")]
        static async Task TaskBind_ReturnNone_Some(NonNull<string> some)
        {
            var actual = await Option.From(some.Get)
                .Bind(_ => FromResult(Option<int>.None))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option returns a Some Option.")]
        static async Task TaskBindReturnSome_Some(NonNull<string> some)
        {
            var actual = await Option.From(some.Get)
                .Bind(v => FromResult(Option.From(v.Length)))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }
    }
}
