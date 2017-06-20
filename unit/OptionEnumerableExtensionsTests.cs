using System.Linq;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionEnumerableExtensions"/>.</summary>
    public static class OptionEnumerableExtensionsTests
    {
        [Fact]
        public static void Cat_AllNone()
        {
            // arrange
            var nones = new[]
            {
                Option<int>.None,
                Option<int>.None,
                Option<int>.None
            };

            // act
            var actual = nones.Cat();

            //assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public static void Cat_AllSome()
        {
            // arrange
            var somes = new[]
            {
                Option.From(33),
                Option.From(44),
                Option.From(55)
            };

            // act
            var actual = somes.Cat();

            //assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            var actualList = actual.ToList();
            Assert.Equal(somes.Length, actualList.Count);
        }

        [Fact]
        public static void Cat_Mixed()
        {
            // arrange
            var mixed = new[]
            {
                Option.From(33),
                Option<int>.None,
                Option.From(55)
            };

            // act
            var actual = mixed.Cat();

            //assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            var actualList = actual.ToList();
            Assert.Equal(mixed.Count(o => o.IsSome), actualList.Count);
        }
    }
}
