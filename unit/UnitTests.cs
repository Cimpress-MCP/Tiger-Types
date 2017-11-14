using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.StringComparer;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="Unit"/>.</summary>
    public static class UnitTests // note(cosborn) lol
    {
        static readonly Unit[] _units =
        {
            new Unit(),
            default,
            Unit.Value
        };

        public static TheoryData<Unit, Unit> ReflexiveEqualityData =
            new CrossProductTheoryData<Unit, Unit>(_units, _units);

        public static TheoryData<Unit> UnitData = new TheoryData<Unit>
        {
            new Unit(),
            default,
            Unit.Value
        };

        [Theory(DisplayName = "All units are equal."), MemberData(nameof(ReflexiveEqualityData))]
        static void Equality_Reflexive(Unit left, Unit right)
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

        [Theory(DisplayName = "A unit is not equal to null."), MemberData(nameof(UnitData))]
        [SuppressMessage("xUnit", "xUnit2002", Justification = "That's the test.")]
        static void Equality_Null(Unit actual) => Assert.NotNull(actual);

        [Theory(DisplayName = "A unit has a hashcode of 1."), MemberData(nameof(UnitData))]
        static void GetHashCode_One(Unit value) =>
            // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            Assert.Equal(1, value.GetHashCode());

        [Theory(DisplayName = "A unit stringfies to empty parentheses."), MemberData(nameof(UnitData))]
        static void ToString_Parens(Unit value) => Assert.Equal("()", value.ToString(), Ordinal);
    }
}
