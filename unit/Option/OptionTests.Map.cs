using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to mapping <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        static void FuncMap_None()
        {
            var actual = Option<string>.None.Map(v => v.Length);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        static void FuncMap_Some(NonNull<string> some)
        {
            var actual = Option.From(some.Get).Map(v => v.Length);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        static async Task TaskMap_None()
        {
            var actual = await Option<string>.None
                .Map(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        static async Task TaskMap_Some(NonNull<string> some)
        {
            var actual = await Option.From(some.Get)
                .Map(v => v.Length.Pipe(FromResult))
                .ConfigureAwait(false);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }
    }
}
