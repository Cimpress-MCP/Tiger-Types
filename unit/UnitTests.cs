using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="Unit"/>.</summary>
    public sealed class UnitTests // note(cosborn) lol
    {
        static readonly Unit[] _units =
        {
            new Unit(),
            default(Unit),
            Unit.Value
        };

        public static TheoryData<Unit, Unit> ReflexiveEqualityData =
            new CrossProductTheoryData<Unit, Unit>(_units, _units);

        public static TheoryData<Unit> UnitData = new TheoryData<Unit>
        {
            new Unit(),
            default(Unit),
            Unit.Value
        };

        [Theory(DisplayName = "All units are equal."), MemberData(nameof(ReflexiveEqualityData))]
        public void Equality_Reflexive(Unit left, Unit right)
        {
            // assert
            Assert.Equal(left, right);
            Assert.True(left.Equals(right));
            Assert.True(left == right);
            Assert.False(left != right);
        }

        [Theory(DisplayName = "A unit is not equal to null."), MemberData(nameof(UnitData))]
        public void Equality_Null(Unit actual)
        {
            // assert
            Assert.NotNull(actual);
        }

        [Theory(DisplayName = "A unit has a hashcode of 1."), MemberData(nameof(UnitData))]
        public void GetHashCode_One(Unit value)
        { // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(1, actual);
        }

        [Theory(DisplayName = "A unit stringfies to empty parentheses."), MemberData(nameof(UnitData))]
        public void ToString_Parens(Unit value)
        {
            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("()", actual);
        }
    }
}
