using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparer;
using static Tiger.Types.Resources;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Try{TErr, TOk}"/>.</summary>
    public static partial class TryTests
    {
        [Fact(DisplayName = "The untyped None converts to a None Try.")]
        public static void LiteralNone_IsNone()
        {
            Try<string, Version> actual = Try.None;

            Assert.True(actual.IsNone);
            Assert.False(actual.IsErr);
            Assert.False(actual.IsOk);
        }

        [Fact(DisplayName = "A null Err value converts to a None Try.")]
        public static void NullErr_IsNone()
        {
            Try<string, Version> actual = (string)null;

            Assert.True(actual.IsNone);
            Assert.False(actual.IsErr);
            Assert.False(actual.IsOk);
        }

        [Fact(DisplayName = "A null Ok value converts to a None Try.")]
        public static void NullOk_IsNone()
        {
            Try<string, Version> actual = (Version)null;

            Assert.True(actual.IsNone);
            Assert.False(actual.IsErr);
            Assert.False(actual.IsOk);
        }

        [Property(DisplayName = "An Err value converts to an Err Try.")]
        public static void ValueErr_IsErr(NonEmptyString err)
        {
            Try<string, Version> actual = err.Get;

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "An Ok value converts to an Ok Try.")]
        public static void ValueOk_IsOk(Version ok)
        {
            Try<string, Version> actual = ok;

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Fact(DisplayName = "Forcibly unwrapping a None Try to its Err value throws.")]
        public static void CastErr_None_Throws()
        {
            var actual = Record.Exception(() => (string)Try<string, Version>.None);

            var ane = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(TryIsNotErr, ane.Message, Ordinal);
        }

        [Fact(DisplayName = "Forcibly unwrapping a None Try to its Ok value throws.")]
        public static void CastOk_None_Throws()
        {
            var actual = Record.Exception(() => (Version)Try<string, Version>.None);

            var ane = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(TryIsNotOk, ane.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping an Err Try to its Err value throws.")]
        public static void CastErr_Err_Value(NonEmptyString err) =>
            Assert.Equal(err.Get, (string)Try<string, Version>.From(err.Get), Ordinal);

        [Property(DisplayName = "Forcibly unwrapping an Err Try to its Ok value throws.")]
        public static void CastOk_Err_Throws(NonEmptyString err)
        {
            var actual = Record.Exception(() => (Version)Try<string, Version>.From(err.Get));

            var ane = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(TryIsNotOk, ane.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping an Ok Try to its Err value throws.")]
        public static void CastErr_Ok_Throws(Version ok)
        {
            var actual = Record.Exception(() => (string)Try<string, Version>.From(ok));

            var ane = Assert.IsType<InvalidOperationException>(actual);
            Assert.Equal(TryIsNotOk, ane.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping an Ok Try produces its Ok value.")]
        public static void CastOk_Ok_Value(Version ok) =>
            Assert.Equal(ok, (Version)Try<string, Version>.From(ok));
    }
}
