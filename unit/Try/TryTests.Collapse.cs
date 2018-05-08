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
        [Property(DisplayName = "Collapsing a Try with null as the fallback throws.")]
        public static void Collapse_NullValue_Throws(Try<Version, Version> @try)
        {
            var actual = Record.Exception(() => @try.Collapse((Version)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try collapses to the fallback value.")]
        public static void Collapse_Value_None(Version none) =>
            Assert.Equal(none, Try<Version, Version>.None.Collapse(none));

        [Property(DisplayName = "An Err Try collapses to its Err value.")]
        public static void Collapse_Value_Err(Version err, Version none) =>
            Assert.Equal(err, Try<Version, Version>.From(err: err).Collapse(none));

        [Property(DisplayName = "An Ok Try collapses to its Ok value.")]
        public static void Collapse_Value_Ok(Version ok, Version none) =>
            Assert.Equal(ok, Try<Version, Version>.From(ok: ok).Collapse(none));

        [Property(DisplayName = "Collapsing a Try with null as the fallback throws.")]
        public static void Collapse_NullFunc_Throws(Try<Version, Version> @try)
        {
            var actual = Record.Exception(() => @try.Collapse((Func<Version>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("none", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A None Try collapses to the fallback value.")]
        public static void Collapse_Func_None(Version none) =>
            Assert.Equal(none, Try<Version, Version>.None.Collapse(() => none));

        [Property(DisplayName = "An Err Try collapses to its Err value.")]
        public static void Collapse_Func_Err(Version err, Func<Version> none) =>
            Assert.Equal(err, Try<Version, Version>.From(err: err).Collapse(none));

        [Property(DisplayName = "An Ok Try collapses to its Ok value.")]
        public static void Collapse_Func_Ok(Version ok, Func<Version> none) =>
            Assert.Equal(ok, Try<Version, Version>.From(ok: ok).Collapse(none));
    }
}
