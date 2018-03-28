using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to mapping from <see cref="OptionTaskExtensions"/>.</summary>
    public static partial class OptionTaskExtensionsTests
    {
        [Property(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public static async Task FuncMapT_None(Func<string, int> mapper)
        {
            var actual = await FromResult(Option<string>.None).MapT(mapper).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public static async Task FuncMapT_Some(NonEmptyString some, Func<string, int> mapper)
        {
            var actual = await FromResult(Option.From(some.Get)).MapT(mapper).ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(mapper(some.Get), innerValue);
        }

        [Property(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public static async Task TaskMapT_None(Func<string, Task<int>> mapper)
        {
            var actual = await FromResult(Option<string>.None).MapTAsync(mapper).ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public static async Task TaskMapT_Some(NonEmptyString some, int result)
        {
            var actual = await FromResult(Option.From(some.Get))
                .MapTAsync(_ => FromResult(result))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(result, innerValue);
        }
    }
}
