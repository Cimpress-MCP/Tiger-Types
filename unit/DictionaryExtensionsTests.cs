using System.Collections.Generic;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="DictionaryExtensions"/>.</summary>
    public static class DictionaryExtensionsTests
    {
        public static readonly TheoryData<string, Option<string>> TryGetValueTheoryData =
            new TheoryData<string, Option<string>>
            {
                { "present", Option.From("sentinel") },
                { "nonpresent", Option<string>.None },
                { "null", Option<string>.None }
            };

        [Theory(DisplayName = "A value can optionally be retrieved from a dictionary.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public static void Dictionary_TryGetValue(string key, Option<string> expected)
        {
            var sut = new Dictionary<string, string>
            {
                ["present"] = "sentinel",
                ["null"] = null
            };

            Assert.Equal(expected, sut.TryGetValue(key));
        }

        [Theory(DisplayName = "A value can optionally be retrieved from a dictionary by interface.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public static void IDictionary_TryGetValue(string key, Option<string> expected)
        {
            var sut = new Dictionary<string, string>
            {
                ["present"] = "sentinel",
                ["null"] = null
            } as IDictionary<string, string>;

            Assert.Equal(expected, sut.TryGetValue(key));
        }

        [Theory(DisplayName = "A value can optionally be retrieved from a read-only dictionary by interface.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public static void IReadOnlyDictionary_TryGetValue(string key, Option<string> expected)
        {
            var sut = new Dictionary<string, string>
            {
                ["present"] = "sentinel",
                ["null"] = null
            } as IReadOnlyDictionary<string, string>;

            Assert.Equal(expected, sut.TryGetValue(key));
        }
    }
}
