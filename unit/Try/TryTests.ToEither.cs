using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Try{TErr, TOk}"/>.</summary>
    public static partial class TryTests
    {
        [Property(DisplayName = "Converting a Try to an Either with a null fallback throws.")]
        public static void ToEither_NullLeftValue_Throw(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.ToEither(left: (string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try converts to a Left Either.")]
        public static void ToEither_NoneValue_Left(NonEmptyString none)
        {
            var actual = Try<string, Version>.None.ToEither(left: none.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(none.Get, innerValue);
        }

        [Property(DisplayName = "An Err Try converts to a Left Either.")]
        public static void ToEither_ErrValue_Left(NonEmptyString err, NonEmptyString none)
        {
            var actual = Try<string, Version>.From(err.Get).ToEither(left: none.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try converts to a Right Either.")]
        public static void ToEither_OkValue_Left(Version ok, NonEmptyString none)
        {
            var actual = Try<string, Version>.From(ok).ToEither(left: none.Get);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Converting a Try to an Either with a null fallback throws.")]
        public static void ToEither_NullLeftFunc_Throw(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.ToEither(left: (Func<string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try converts to a Left Either.")]
        public static void ToEither_NoneFunc_Left(NonEmptyString none)
        {
            var actual = Try<string, Version>.None.ToEither(left: () => none.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(none.Get, innerValue);
        }

        [Property(DisplayName = "An Err Try converts to a Left Either.")]
        public static void ToEither_ErrFunc_Left(NonEmptyString err, NonEmptyString none)
        {
            var actual = Try<string, Version>.From(err.Get).ToEither(left: () => none.Get);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try converts to a Right Either.")]
        public static void ToEither_OkFunc_Left(Version ok, NonEmptyString none)
        {
            var actual = Try<string, Version>.From(ok).ToEither(left: () => none.Get);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Converting a Try to an Either with a null fallback throws.")]
        public static void ToEither_NullRightValue_Throw(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.ToEither(right: (Version)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try converts to a Right Either.")]
        public static void ToEither_NoneValue_Right(Version none)
        {
            var actual = Try<string, Version>.None.ToEither(right: none);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(none, innerValue);
        }

        [Property(DisplayName = "An Err Try converts to a Left Either.")]
        public static void ToEither_ErrValue_Right(NonEmptyString err, Version none)
        {
            var actual = Try<string, Version>.From(err.Get).ToEither(right: none);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try converts to a Right Either.")]
        public static void ToEither_OkValue_Right(Version ok, Version none)
        {
            var actual = Try<string, Version>.From(ok).ToEither(right: none);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Converting a Try to an Either with a null fallback throws.")]
        public static void ToEither_NullRightFunc_Throw(Try<string, Version> @try)
        {
            var actual = Record.Exception(() => @try.ToEither(right: (Func<Version>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try converts to a Right Either.")]
        public static void ToEither_NoneFunc_Right(Func<Version> none)
        {
            var actual = Try<string, Version>.None.ToEither(right: none);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(none(), innerValue);
        }

        [Property(DisplayName = "An Err Try converts to a Left Either.")]
        public static void ToEither_ErrFunc_Right(NonEmptyString err, Func<Version> none)
        {
            var actual = Try<string, Version>.From(err.Get).ToEither(right: none);

            Assert.True(actual.IsLeft);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok Try converts to a Right Either.")]
        public static void ToEither_OkFunc_Right(Version ok, Version none)
        {
            var actual = Try<string, Version>.From(ok).ToEither(right: () => none);

            Assert.True(actual.IsRight);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }
    }
}
