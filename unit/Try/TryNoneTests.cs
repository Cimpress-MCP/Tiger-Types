using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="TryNone"/>.</summary>
    public static class TryNoneTests
    {
        static readonly TryNone[] _nones =
        {
            new TryNone(),
            default,
            Try.None
        };

        public static TheoryData<TryNone, TryNone> ReflexiveEqualityData { get; } =
            new CrossProductTheoryData<TryNone, TryNone>(_nones, _nones);

        public static TheoryData<TryNone> TryNoneData { get; } = new TheoryData<TryNone>()
        {
            new TryNone(),
            default,
            Try.None
        };

        [Theory(DisplayName = "All TryNones are equal."), MemberData(nameof(ReflexiveEqualityData))]
        public static void Equality_Reflexive(TryNone left, TryNone right)
        {
            Assert.Equal(left, right);
            Assert.Equal(right, left);

            Assert.True(left.Equals(right));
            Assert.True(right.Equals(left));

            Assert.True(left == right);
            Assert.True(right == left);

            Assert.False(left != right);
            Assert.False(right != left);
        }

        [Theory(DisplayName = "A TryNone is not equal to null."), MemberData(nameof(TryNoneData))]
        [SuppressMessage("xUnit", "xUnit2002", Justification = "That's the test.")]
        public static void Equality_Null(TryNone none) => Assert.NotNull(none);

        [Theory(DisplayName = "A TryNone has a hashcode of 0."), MemberData(nameof(TryNoneData))]
        public static void GetHashCode_Zero(TryNone none) =>
            // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            Assert.Equal(0, none.GetHashCode());

        [Theory(DisplayName = "A TryNone stringfies to None."), MemberData(nameof(TryNoneData))]
        public static void ToString_None(TryNone none) =>
            Assert.Equal("None", none.ToString(), Ordinal);

        [Theory(DisplayName = "A TryNone dumps to a None Try."), MemberData(nameof(TryNoneData))]
        public static void Dump_Empty(TryNone none)
        {
            var actual = typeof(TryNone)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(none, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            var property = Assert.Single(properties);
            Assert.NotNull(property);
            Assert.Equal("State", property.Name);
            Assert.Equal("None", property.GetValue(actual));
        }
    }
}
