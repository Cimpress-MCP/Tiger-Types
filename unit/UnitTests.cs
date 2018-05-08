using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparer;

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

        public static TheoryData<Unit, Unit> ReflexiveEqualityData { get; } =
            new CrossProductTheoryData<Unit, Unit>(_units, _units);

        public static TheoryData<Unit> UnitData { get; } = new TheoryData<Unit>
        {
            new Unit(),
            default,
            Unit.Value
        };

        [Theory(DisplayName = "All units are equal."), MemberData(nameof(ReflexiveEqualityData))]
        public static void Equality_Reflexive(Unit left, Unit right)
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
        public static void Equality_Null(Unit unit) => Assert.NotNull(unit);

        [Theory(DisplayName = "A unit has a hashcode of 1."), MemberData(nameof(UnitData))]
        public static void GetHashCode_One(Unit unit) =>
            // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            Assert.Equal(1, unit.GetHashCode());

        [Theory(DisplayName = "A unit stringfies to empty parentheses."), MemberData(nameof(UnitData))]
        public static void ToString_Parens(Unit unit) => Assert.Equal("()", unit.ToString(), Ordinal);

        [Theory(DisplayName = "A unit dumps to an empty object."), MemberData(nameof(UnitData))]
        public static void Dump_Empty(Unit unit)
        {
            var actual = typeof(Unit)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(unit, Array.Empty<object>())
                .GetType()
                .GetProperties();

            Assert.Empty(actual);
        }
    }
}
