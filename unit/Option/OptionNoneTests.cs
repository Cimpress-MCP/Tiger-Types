using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparer;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="OptionNone"/>.</summary>
    public static class OptionNoneTests
    {
        static readonly OptionNone[] _nones =
        {
            new OptionNone(),
            default,
            Option.None
        };

        public static TheoryData<OptionNone, OptionNone> ReflexiveEqualityData { get; } =
            new CrossProductTheoryData<OptionNone, OptionNone>(_nones, _nones);

        public static TheoryData<OptionNone> OptionNoneData { get; } = new TheoryData<OptionNone>
        {
            new OptionNone(),
            default,
            Option.None
        };

        [Theory(DisplayName = "All OptionNones are equal."), MemberData(nameof(ReflexiveEqualityData))]
        public static void Equality_Reflexive(OptionNone left, OptionNone right)
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

        [Theory(DisplayName = "An OptionNone is not equal to null."), MemberData(nameof(OptionNoneData))]
        [SuppressMessage("xUnit", "xUnit2002", Justification = "That's the test.")]
        public static void Equality_Null(OptionNone none) => Assert.NotNull(none);

        [Theory(DisplayName = "An OptionNone has a hashcode of 0."), MemberData(nameof(OptionNoneData))]
        public static void GetHashCode_Zero(OptionNone none) =>
            // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            Assert.Equal(0, none.GetHashCode());

        [Theory(DisplayName = "An OptionNone stringfies to None."), MemberData(nameof(OptionNoneData))]
        public static void ToString_None(OptionNone none) => Assert.Equal("None", none.ToString(), Ordinal);

        [Theory(DisplayName = "An OptionNone dumps to a None Option."), MemberData(nameof(OptionNoneData))]
        public static void Dump_Empty(OptionNone none)
        {
            var actual = typeof(OptionNone)
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
