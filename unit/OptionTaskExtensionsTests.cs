using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionTaskExtensions"/>.</summary>
    public static class OptionTaskExtensionsTests
    {
        #region MatchT

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
            "not the Some func branch.")]
        public static async Task ValueFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: none,
                some: v => v.Length);

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None value branch.")]
        public static async Task ValueFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MatchT(
                none: none,
                some: v => v.Length);

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some func branch.")]
        public static async Task FuncFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => none,
                some: v => v.Length);

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None func branch.")]
        public static async Task FuncFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MatchT(
                none: () => none,
                some: v => v.Length);

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some task branch.")]
        public static async Task FuncTaskMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None func branch.")]
        public static async Task FuncTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some func branch.")]
        public static async Task TaskFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length);

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None task branch.")]
        public static async Task TaskFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length);

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some task branch.")]
        public static async Task TaskTaskMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None task branch.")]
        public static async Task TaskTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        #endregion

        #region MapT

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public static async Task FuncMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MapT(v => v.Length);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public static async Task FuncMapT_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MapT(v => v.Length);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public static async Task TaskMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.MapT(v => v.Length.Pipe(FromResult));

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public static async Task TaskMapT_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.MapT(v => v.Length.Pipe(FromResult));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        #endregion

        #region BindT

        [Fact(DisplayName = "Binding a None Option over a func that returns a None Option " +
            "returns a None Option.")]
        public static async Task FuncBindT_ReturnNone_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.BindT(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a None Option over a func that returns a None Option " +
            "returns a None Option.")]
        public static async Task FuncBindT_ReturnSome_None(int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = await value.BindT(_ => Option.From(sentinel));

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option " +
            "returns a None Option.")]
        public static async Task FuncBindT_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.BindT(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option " +
            "returns a Some Option.")]
        public static async Task FuncBindTReturnSome_Some(NonNull<string> some, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.BindT(_ => Option.From(sentinel));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option " +
            "returns a None Option.")]
        public static async Task TaskBindT_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.BindT(_ => FromResult(Option<int>.None));

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option " +
            "returns a Some Option.")]
        public static async Task TaskBindTReturnSome_Some(NonNull<string> some, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = await value.BindT(_ => FromResult(Option.From(sentinel)));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region TapT

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option " +
            "and performs no action.")]
        public static async Task FuncTap_None(int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = before;
            var actual = await value.TapT(_ => output = sentinel);

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
            "and performs an action.")]
        public static async Task FuncTap_Some(NonNull<string> some, int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var output = before;
            var actual = await value.TapT(_ => output = sentinel);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option " +
            "and performs no action.")]
        public static async Task TaskTap_None(int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = before;
            var actual = await value.TapT(_ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option " +
            "and performs an action.")]
        public static async Task TaskTap_Some(NonNull<string> some, int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var output = before;
            var actual = await value.TapT(_ => Run(() => output = sentinel));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        #endregion
    }
}
