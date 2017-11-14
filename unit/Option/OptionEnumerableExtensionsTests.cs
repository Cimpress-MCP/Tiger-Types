using System.Linq;
using FsCheck.Xunit;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionEnumerableExtensions"/>.</summary>
    public static class OptionEnumerableExtensionsTests
    {
        [Property(DisplayName = "A collection of None Options cats to an empty collection.")]
        static void Cat_AllNone(byte count)
        {
            var nones = Enumerable.Repeat(Option<int>.None, count);

            var actual = nones.Cat();

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Property(DisplayName = "A collection of Some Options cats to a collection of the Some value of its elements.")]
        static void Cat_AllSome(int[] values)
        {
            var actual = values.Select(Option.From).Cat().ToList();

            Assert.NotNull(actual);
            Assert.Equal(values.Length, actual.Count);
        }

        [Fact(DisplayName = "A collection of mixed Option values cats to a collection of the Some values of its elements in the Some state.")]
        static void Cat_Mixed()
        {
            var mixed = new[]
            {
                Option.From(33),
                Option<int>.None,
                Option.From(55)
            };

            var actual = mixed.Cat().ToList();

            //assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(mixed.Count(o => o.IsSome), actual.Count);
            Assert.Collection(actual,
                i => Assert.Equal(33, i),
                i => Assert.Equal(55, i));
        }
    }
}
