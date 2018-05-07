using System;
using FsCheck.Xunit;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to replacing <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Atempting to replace an Option with null throws.")]
        public static void Replace_Null_Throws(Option<int> option)
        {
            var actual = Record.Exception(() => option.Replace((string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("replacement", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Replacing a None Option returns a None Option.")]
        public static void Replace_None(int replacement)
        {
            var actual = Option<string>.None.Replace(replacement);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Replacing a Some Option returns a Some Option.")]
        public static void Replace_Some(int value, int replacement)
        {
            var actual = Option.From(value).Replace(replacement);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(replacement, innerValue);
        }
    }
}
