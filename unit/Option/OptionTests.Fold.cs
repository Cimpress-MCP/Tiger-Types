using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to folding <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        static void FuncFold_None(int seed) => Assert.Equal(seed, Option<string>.None.Fold(seed, (s, v) => s + v.Length));

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator over the seed value and the Some value.")]
        static void FuncFold_Some(NonNull<string> some, int seed) =>
            Assert.Equal(seed + some.Get.Length, Option.From(some.Get).Fold(seed, (s, v) => s + v.Length));

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        static async Task TaskFold_None(int seed)
        {
            var actual = await Option<string>.None
                .Fold(seed, (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns result of invoking the accumulator" +
                                "over the seed value and the Some value.")]
        static async Task TaskFold_Some(NonNull<string> some, int seed)
        {
            var actual = await Option.From(some.Get)
                .Fold(seed, (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed + some.Get.Length, actual);
        }
    }
}
