using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "A None Option becomes a None Try.")]
        public static void ToTry_None() =>
            Assert.True(Option<string>.None.ToTry<Exception, string>().IsNone);

        [Property(DisplayName = "A Some option becomes an Ok Try.")]
        public static void ToTry_Some(NonEmptyString some)
        {
            var actual = Option.From(some.Get).ToTry<Exception, string>();

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue, Ordinal);
        }
    }
}
