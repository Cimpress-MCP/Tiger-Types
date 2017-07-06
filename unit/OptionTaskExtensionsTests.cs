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
        public static void ValueFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MatchT(
                none: none,
                some: v => v.Length).Result;

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None value branch.")]
        public static void ValueFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MatchT(
                none: none,
                some: v => v.Length).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some func branch.")]
        public static void FuncFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MatchT(
                none: () => none,
                some: v => v.Length).Result;

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None func branch.")]
        public static void FuncFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MatchT(
                none: () => none,
                some: v => v.Length).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some task branch.")]
        public static void FuncTaskMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None func branch.")]
        public static void FuncTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MatchT(
                none: () => none,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some func branch.")]
        public static void TaskFuncMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length).Result;

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None task branch.")]
        public static void TaskFuncMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some task branch.")]
        public static void TaskTaskMatchTReturn_None(int none)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(none, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None task branch.")]
        public static void TaskTaskMatchTReturn_Some(NonNull<string> some, int none)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MatchT(
                none: () => FromResult(none),
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        #endregion

        #region MapT

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public static void FuncMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MapT(v => v.Length).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public static void FuncMapT_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MapT(v => v.Length).Result;

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public static void TaskMapT_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.MapT(v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public static void TaskMapT_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.MapT(v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        #endregion

        #region BindT

        [Fact(DisplayName = "Binding a None Option over a func that returns a None Option " +
            "returns a None Option.")]
        public static void FuncBindT_ReturnNone_None()
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.BindT(_ => Option<int>.None).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a None Option over a func that returns a None Option " +
            "returns a None Option.")]
        public static void FuncBindT_ReturnSome_None(int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var actual = value.BindT(_ => Option.From(sentinel)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option " +
            "returns a None Option.")]
        public static void FuncBindT_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.BindT(_ => Option<int>.None).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option " +
            "returns a Some Option.")]
        public static void FuncBindTReturnSome_Some(NonNull<string> some, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.BindT(_ => Option.From(sentinel)).Result;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option " +
            "returns a None Option.")]
        public static void TaskBindT_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.BindT(_ => FromResult(Option<int>.None)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option " +
            "returns a Some Option.")]
        public static void TaskBindTReturnSome_Some(NonNull<string> some, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var actual = value.BindT(_ => FromResult(Option.From(sentinel))).Result;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel, innerValue);
        }

        #endregion

        #region TapT

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option " +
            "and performs no action.")]
        public static void FuncTap_None(int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = before;
            var actual = value.TapT(_ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
            "and performs an action.")]
        public static void FuncTap_Some(NonNull<string> some, int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var output = before;
            var actual = value.TapT(_ => output = sentinel).Result;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option " +
            "and performs no action.")]
        public static void TaskTap_None(int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option<string>.None);

            // act
            var output = before;
            var actual = value.TapT(_ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option " +
            "and performs an action.")]
        public static void TaskTap_Some(NonNull<string> some, int before, int sentinel)
        {
            // arrange
            var value = FromResult(Option.From(some.Get));

            // act
            var output = before;
            var actual = value.TapT(_ => Run(() => output = sentinel)).Result;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel, output);
        }

        #endregion
    }
}
