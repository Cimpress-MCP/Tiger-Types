﻿// ReSharper disable All

using System.Collections.Generic;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="DictionaryExtensions"/>.</summary>
    public sealed class DictionaryExtensionsTests
    {
        const string sentinel = "sentinel";

        public static readonly TheoryData<string, Option<string>>  TryGetValueTheoryData =
            new TheoryData<string, Option<string>>
            {
                { "present", Option.From(sentinel) },
                { "nonpresent", Option<string>.None },
                { "null", Option<string>.None }
            };
        
        [Theory(DisplayName = "A value can optionally be retrieved from a dictionary.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public void Dictionary_TryGetValue(string key, Option<string> expected)
        {
            // arrange
            var sut = new Dictionary<string, string>
            {
                ["present"] = sentinel,
                ["null"] = null
            };

            // act
            var actual = sut.TryGetValue(key);

            // arrange, act
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "A value can optionally be retrieved from a dictionary by interface.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public void IDictionary_TryGetValue(string key, Option<string> expected)
        {
            // arrange
            var sut = new Dictionary<string, string>
            {
                ["present"] = sentinel,
                ["null"] = null
            } as IDictionary<string, string>;

            // act
            var actual = sut.TryGetValue(key);

            // arrange, act
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "A value can optionally be retrieved from a read-only dictionary by interface.")]
        [MemberData(nameof(TryGetValueTheoryData))]
        public void IReadOnlyDictionary_TryGetValue(string key, Option<string> expected)
        {
            // arrange
            var sut = new Dictionary<string, string>
            {
                ["present"] = sentinel,
                ["null"] = null
            } as IReadOnlyDictionary<string, string>;

            // act
            var actual = sut.TryGetValue(key);

            // arrange, act
            Assert.Equal(expected, actual);
        }
    }
}
