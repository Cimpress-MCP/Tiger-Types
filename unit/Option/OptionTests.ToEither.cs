using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Converting an Option to an Either with a null fallback throws.")]
        public static void ValueToEither_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).ToEither(fallback: (Version)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("fallback", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Converting a None Option to an Either results in a Left Either.")]
        public static void ValueToEither_None(Guid fallback)
        {
            var actual = Option<string>.None.ToEither(fallback);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(fallback, innerValue);
        }

        [Property(DisplayName = "Converting a Some Option to an Either results in a Right Either.")]
        public static void ValueToEither_Some(NonEmptyString some, Guid fallback)
        {
            var actual = Option.From(some.Get).ToEither(fallback);

            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Converting an Option to an Either with a null fallback func throws.")]
        public static void FuncToEither_Null_Throws(string from)
        {
            var actual = Record.Exception(() => Option.From(from).ToEither(fallback: (Func<Version>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("fallback", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Converting a None Option to an Either results in a Left Either.")]
        public static void FuncToEither_None(Guid fallback)
        {
            var actual = Option<string>.None.ToEither(() => fallback);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(fallback, innerValue);
        }

        [Property(DisplayName = "Converting a Some Option to an Either results in a Right Either.")]
        public static void FuncToEither_Some(NonEmptyString some, Guid fallback)
        {
            var actual = Option.From(some.Get).ToEither(() => fallback);

            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Converting an Option to an Either with a null fallback func throws.")]
        public static async Task TaskToEither_Null_Throws(string from)
        {
            var actual = await Record.ExceptionAsync(() => Option.From(from)
                .ToEitherAsync(fallback: (Func<Task<Version>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("fallback", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Converting a None Option to an Either results in a Left Either.")]
        public static async Task TaskToEither_None(Guid fallback)
        {
            var actual = await Option<string>.None
                .ToEitherAsync(() => FromResult(fallback))
                .ConfigureAwait(false);

            Assert.True(actual.IsLeft);
            var innerValue = (Guid)actual;
            Assert.Equal(fallback, innerValue);
        }

        [Property(DisplayName = "Converting a Some Option to an Either results in a Right Either.")]
        public static async Task TaskToEither_Some(NonEmptyString some, Guid fallback)
        {
            var actual = await Option.From(some.Get)
                .ToEitherAsync(() => FromResult(fallback))
                .ConfigureAwait(false);

            Assert.True(actual.IsRight);
            var innerValue = (string)actual;
            Assert.Equal(some.Get, innerValue);
        }
    }
}
