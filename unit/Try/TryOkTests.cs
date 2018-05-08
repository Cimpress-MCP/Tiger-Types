using System;
using System.Collections.Generic;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="TryOk{TOk}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class TryOkTests
    {
        [Property(DisplayName = "Attempting to create a TryOk with null throws.")]
        public static void Construct_Null_Throws()
        {
            var actual = Record.Exception(() => Try.Ok<Version>(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("ok", ane.Message, StringComparison.Ordinal);
        }

        [Property(DisplayName = "A TryOk converts to an Ok Try.")]
        public static void OkTry(Version ok)
        {
            Try<string, Version> actual = Try.Ok(ok);

            Assert.True(actual.IsOk);
            var innerValue = actual.Value;
            Assert.Equal(ok, innerValue);
        }

        [Property(DisplayName = "Two TryOks are equal if their Ok values are equal.")]
        public static void Equal_Equal(Version ok)
        {
            var left = Try.Ok(ok);
            var right = Try.Ok(ok);

            Assert.Equal(left, right);
            Assert.Equal(right, left);

            Assert.True(left.Equals(right));
            Assert.True(right.Equals(left));

            Assert.True(left == right);
            Assert.True(right == left);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Property(DisplayName = "Two TryOks are not equal if their Ok values are not equal.")]
        public static void Equal_NotEqual(UnequalNonNullPair<Version> pair)
        {
            var left = Try.Ok(pair.Left);
            var right = Try.Ok(pair.Right);

            Assert.NotEqual(left, right);
            Assert.NotEqual(right, left);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));

            Assert.False(left == right);
            Assert.False(right == left);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "A Try Ok stringifies to Ok.")]
        public static void ToString_Ok(Version ok) =>
            Assert.Equal($"Ok({ok})", Try.Ok(ok).ToString(), Ordinal);

        [Property(DisplayName = "A Try Ok has the hashcode of its value.")]
        public static void GetHashCode_Value(Version ok) =>
            Assert.Equal(EqualityComparer<Version>.Default.GetHashCode(ok), Try.Ok(ok).GetHashCode());

        [Property(DisplayName = "A TryOk dumps to an Ok Try.")]
        public static void Dump_Ok(Version ok)
        {
            var @try = Try.Ok(ok);
            var actual = typeof(TryOk<Version>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(@try, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            Assert.All(properties, Assert.NotNull);
            Assert.Collection(properties,
                pi =>
                {
                    Assert.Equal("State", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal("Ok", value);
                },
                pi =>
                {
                    Assert.Equal("Value", pi.Name);
                    var value = Assert.IsType<Version>(pi.GetValue(actual));
                    Assert.Equal(ok, value);
                });
        }
    }
}
