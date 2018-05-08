using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to folding <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static void FuncFold_NullSeed_Throws(Option<int> option, Func<string, int, string> folder)
        {
            var actual = Record.Exception(() => option.Fold(null, folder));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static void FuncFold_NullFolder_Throws(Option<int> option, NonEmptyString state)
        {
            var actual = Record.Exception(() => option.Fold(state.Get, null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void FuncFold_None(int seed) => Assert.Equal(seed, Option<string>.None.Fold(seed, (s, v) => s + v.Length));

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator over the seed value and the Some value.")]
        public static void FuncFold_Some(NonEmptyString some, int seed) =>
            Assert.Equal(seed + some.Get.Length, Option.From(some.Get).Fold(seed, (s, v) => s + v.Length));

        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static async Task TaskFold_NullSeed_Throws(Option<int> option, Func<string, int, Task<string>> folder)
        {
            var actual = await Record.ExceptionAsync(() => option.FoldAsync(null, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over an option with a null seed throws.")]
        public static async Task TaskFold_NullFolder_Throws(Option<int> option, NonEmptyString state)
        {
            var actual = await Record.ExceptionAsync(() => option.FoldAsync(state.Get, null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static async Task TaskFold_None(int seed)
        {
            var actual = await Option<string>.None
                .FoldAsync(seed, (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns result of invoking the accumulator" +
                                "over the seed value and the Some value.")]
        public static async Task TaskFold_Some(NonEmptyString some, int seed)
        {
            var actual = await Option.From(some.Get)
                .FoldAsync(seed, (s, v) => FromResult(s + v.Length))
                .ConfigureAwait(false);

            Assert.Equal(seed + some.Get.Length, actual);
        }
    }
}
