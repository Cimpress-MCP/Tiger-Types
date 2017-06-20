using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionTaskExtensions"/>.</summary>
    public sealed class OptionTaskExtensionsTests
    {
        const string sentinel = "sentinel";

        #region MatchT

        [Fact(DisplayName = "Matching a None Option returns the None func branch, " +
                            "not the Some func branch.")]
        public async Task FuncFuncMatchTReturn_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => 42,
                some: v => v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some func branch, " +
                            "not the None func branch.")]
        public async Task FuncFuncMatchTReturn_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MatchT(
                none: () => 42,
                some: v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None func branch, " +
                            "not the Some task branch.")]
        public async Task FuncTaskMatchTReturn_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some task branch, " +
                            "not the None func branch.")]
        public async Task FuncTaskMatchTReturn_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MatchT(
                none: () => 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None task branch, " +
                            "not the Some func branch.")]
        public async Task TaskFuncMatchTReturn_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => FromResult(42),
                some: v => v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some func branch, " +
                            "not the None task branch.")]
        public async Task TaskFuncMatchTReturn_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MatchT(
                none: () => FromResult(42),
                some: v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None task branch, " +
                            "not the Some task branch.")]
        public async Task TaskTaskMatchTReturn_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => FromResult(42),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatchTReturn_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MatchT(
                none: () => FromResult(42),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        #endregion

        #region MapT

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public async Task FuncMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MapT(v => v.Length);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public async Task FuncMapT_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MapT(v => v.Length);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(sentinel.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public async Task TaskMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MapT(v => v.Length.Pipe(FromResult));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public async Task TaskMapT_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.MapT(v => v.Length.Pipe(FromResult));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(sentinel.Length, length);
        }

        #endregion

        #region BindT

        [Fact(DisplayName = "Binding a None Option over a func returns a None Option.")]
        public async Task FuncBindT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.BindT(v => v.Length == 0
                ? Option<int>.None
                : Option.From(v.Length));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a None Option " +
                            "returns a None Option.")]
        public async Task FuncBindT_ReturnNone_Some()
        {
            // arrange
            var value = FromResult(Option.From(string.Empty));

            // act
            var actual = await value.BindT(v => v.Length == 0
                ? Option<int>.None
                : Option.From(v.Length));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a Some Option " +
                            "returns a Some Option.")]
        public async Task FuncBindTReturnSome_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.BindT(v => v.Length == 0
                ? Option<int>.None
                : Option.From(v.Length));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a task returning a None Option " +
                            "returns a None Option.")]
        public async Task TaskBindT_ReturnNone_Some()
        {
            // arrange
            var value = FromResult(Option.From(string.Empty));

            // act
            var actual = await value.BindT(v =>
                FromResult(v.Length == 0
                    ? Option<int>.None
                    : Option.From(v.Length)));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Binding a Some Option over a task returning a Some Option " +
                            "returns a Some Option.")]
        public async Task TaskBindTReturnSome_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var actual = await value.BindT(v =>
                FromResult(v.Length == 0
                    ? Option<int>.None
                    : Option.From(v.Length)));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(sentinel.Length, actual);
        }

        #endregion

        #region TapT

        [Fact(DisplayName = "Tapping a None Option over a func returns a None Option " +
                            "and performs no action.")]
        public async Task FuncTap_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => output = string.Empty);

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
                            "and performs an action.")]
        public async Task FuncTap_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var output = string.Empty;
            var actual = await value.TapT(v => output = sentinel);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a None Option over a task returns a None Option " +
                            "and performs no action.")]
        public async Task TaskTap_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = sentinel;
            var actual = await value.TapT(v => Run(() => output = string.Empty));

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a Some Option over a task returns a Some Option " +
                            "and performs an action.")]
        public async Task TaskTap_Some()
        {
            // arrange
            var value = FromResult(Option.From(sentinel));

            // act
            var output = string.Empty;
            var actual = await value.TapT(v => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(sentinel, output);
        }

        #endregion
    }
}
