// ReSharper disable All

using Xunit;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to the functionality of <see cref="Unit"/>.</summary>
    public sealed class UnitTests // note(cosborn) lol
    {
        static Unit[] Units =
        {
            new Unit(), default(Unit), Unit.Value
        };

        public static TheoryData<Unit, Unit> ReflexiveEqualityData =
            new CrossProductTheoryData<Unit, Unit>(Units, Units);

        public static TheoryData<Unit> UnitData = new TheoryData<Unit>
        {
            new Unit(), default(Unit), Unit.Value
        };

        [Theory(DisplayName = "All units are equal."), MemberData(nameof(ReflexiveEqualityData))]
        public void Equality_Reflexive(Unit left, Unit right)
        {
            // arrange
            
            // act
            
            // assert
            Assert.Equal(left, right);
            Assert.True(left.Equals(right));
            Assert.True(left == right);
            Assert.False(left != right);
        }

        [Theory(DisplayName = "A unit is not equal to null."), MemberData(nameof(UnitData))]
        public void Equality_Null(Unit actual)
        {
            // arrange

            // act

            // assert
            Assert.NotNull(actual);
        }

        [Theory(DisplayName = "A unit has a hashcode of 1."), MemberData(nameof(UnitData))]
        public void GetHashCode_One(Unit value)
        { // note(cosborn) Any value is fine, as long as they're equal. One might be better than zero.
            // arrange

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(1, actual);
        }

        [Theory(DisplayName = "A unit stringfies to empty parentheses."), MemberData(nameof(UnitData))]
        public void ToString_Parens(Unit value)
        {
            // arrange

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("()", actual);
        }
    }
}
