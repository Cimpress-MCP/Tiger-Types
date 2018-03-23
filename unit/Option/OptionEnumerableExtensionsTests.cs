﻿using System.Linq;
using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionEnumerableExtensions"/>.</summary>
    public static class OptionEnumerableExtensionsTests
    {
        [Fact(DisplayName = "A null collection of options throws.")]
        public static void Cat_Null_Throws()
        {
            var actual = Record.Exception(() => OptionEnumerableExtensions.Cat<int>(null));

            Assert.NotNull(actual);
        }

        [Property(DisplayName = "A collection of None Options cats to an empty collection.")]
        public static void Cat_AllNone(byte count)
        {
            var actual = Enumerable.Repeat(Option<int>.None, count).Cat();

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Property(DisplayName = "A collection of Some Options cats to a collection of the Some value of its elements.")]
        public static void Cat_AllSome(int[] values)
        {
            var actual = values.Select(Option.From).Cat().ToList();

            Assert.NotNull(actual);
            Assert.Equal(values.Length, actual.Count);
        }

        [Property(DisplayName = "A collection of mixed Option values cats to a collection of the Some values of its elements in the Some state.")]
        public static void Cat_Mixed(int a, int b)
        {
            var mixed = new[]
            {
                Option.From(a),
                Option<int>.None,
                Option.From(b)
            };

            var actual = mixed.Cat().ToList();

            //assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(mixed.Count(o => o.IsSome), actual.Count);
            Assert.Collection(actual,
                i => Assert.Equal(a, i),
                i => Assert.Equal(b, i));
        }
    }
}
