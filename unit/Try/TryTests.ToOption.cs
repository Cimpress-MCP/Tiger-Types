using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Try{TErr, TOk}"/>.</summary>
    public static partial class TryTests
    {
        [Fact(DisplayName = "A None Try becomes a None Option.")]
        public static void ToOption_None() =>
            Assert.True(Try<Exception, string>.None.ToOption().IsNone);

        [Property(DisplayName = "An Err Try becomes a None Option.")]
        public static void ToOption_Err(NonEmptyString err) =>
            Assert.True(Try<string, Version>.From(err.Get).ToOption().IsNone);

        [Property(DisplayName = "An Ok Try becomes a Some Option.")]
        public static void ToOption_Ok(Version ok)
        {
            var actual = Try<string, Version>.From(ok).ToOption();

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }
    }
}
