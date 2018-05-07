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
    /// <summary>Tests related to the functionality of <see cref="TryErr{TErr}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class TryErrTests
    {
        [Property(DisplayName = "Attempting to create a TryErr with null throws.")]
        public static void Construct_Null_Throws()
        {
            var actual = Record.Exception(() => Try.Err<string>(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("err", ane.Message, StringComparison.Ordinal);
        }

        [Property(DisplayName = "A TryErr converts to an Err Try.")]
        public static void ErrTry(NonEmptyString err)
        {
            Try<string, Version> actual = Try.Err(err.Get);

            Assert.True(actual.IsErr);
            var innerValue = (string)actual;
            Assert.Equal(err.Get, innerValue);
        }

        [Property(DisplayName = "Two TryErrs are equal if their Err values are equal.")]
        public static void Equal_Equal(NonEmptyString err)
        {
            var left = Try.Err(err.Get);
            var right = Try.Err(err.Get);

            Assert.Equal(left, right);
            Assert.Equal(right, left);

            Assert.True(left.Equals(right));
            Assert.True(right.Equals(left));

            Assert.True(left == right);
            Assert.True(right == left);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Property(DisplayName = "Two TryErrs are not equal if their Err values are not equal.")]
        public static void Equal_NotEqual(UnequalNonNullPair<NonEmptyString> pair)
        {
            var left = Try.Err(pair.Left.Get);
            var right = Try.Err(pair.Right.Get);

            Assert.NotEqual(left, right);
            Assert.NotEqual(right, left);

            Assert.False(left.Equals(right));
            Assert.False(right.Equals(left));

            Assert.False(left == right);
            Assert.False(right == left);

            Assert.True(left != right);
            Assert.True(right != left);
        }

        [Property(DisplayName = "A TryErr stringifies to Err.")]
        public static void ToString_Err(NonEmptyString err) =>
            Assert.Equal($"Err({err.Get})", Try.Err(err.Get).ToString(), Ordinal);

        [Property(DisplayName = "A TryErr has the hashcode of its value.")]
        public static void GetHashCode_Value(NonEmptyString err) =>
            Assert.Equal(EqualityComparer<string>.Default.GetHashCode(err.Get), Try.Err(err.Get).GetHashCode());

        [Property(DisplayName = "A TryErr dumps to an Err Try.")]
        public static void Dump_Empty(NonEmptyString err)
        {
            var @try = Try.Err(err.Get);
            var actual = typeof(TryErr<string>)
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
                    Assert.Equal("Err", value);
                },
                pi =>
                {
                    Assert.Equal("Value", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal(err.Get, value);
                });
        }
    }
}
