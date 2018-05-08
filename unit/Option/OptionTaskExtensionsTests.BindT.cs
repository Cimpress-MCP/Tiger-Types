using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to binding from <see cref="OptionTaskExtensions"/>.</summary>
    public static partial class OptionTaskExtensionsTests
    {
        [Fact(DisplayName = "Binding a None Option over a func that returns a None Option returns a None Option.")]
        public static async Task FuncBindT_ReturnNone_None()
        {
            var actual = await FromResult(Option<string>.None)
                .BindT(_ => Option<int>.None)
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a None Option over a func that returns a Some Option returns a None Option.")]
        public static async Task FuncBindT_ReturnSome_None(int sentinel)
        {
            var actual = await FromResult(Option<string>.None)
                .BindT(_ => Option.From(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option returns a None Option.")]
        public static async Task FuncBindT_ReturnNone_Some(NonEmptyString some)
        {
            var actual = await FromResult(Option.From(some.Get))
                .BindT(_ => Option<int>.None)
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option returns a Some Option.")]
        public static async Task FuncBindTReturnSome_Some(NonEmptyString some, int sentinel)
        {
            var actual = await FromResult(Option.From(some.Get))
                .BindT(_ => Option.From(sentinel))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option returns a None Option.")]
        public static async Task TaskBindT_ReturnNone_Some(NonEmptyString some)
        {
            var actual = await FromResult(Option.From(some.Get))
                .BindTAsync(_ => FromResult(Option<int>.None))
                .ConfigureAwait(false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option returns a Some Option.")]
        public static async Task TaskBindTReturnSome_Some(NonEmptyString some, int sentinel)
        {
            var actual = await FromResult(Option.From(some.Get))
                .BindTAsync(_ => FromResult(Option.From(sentinel)))
                .ConfigureAwait(false);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }
    }
}
