// ReSharper disable All

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="DictionaryExtensions"/>.</summary>
    [TestFixture(TestOf = typeof(DictionaryExtensions))]
    public sealed class DictionaryTestFixture
    {
        const string sentinel = "sentinel";

        Dictionary<string, string> _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["present"] = sentinel,
                ["null"] = null
            };
        }

        static readonly TestCaseData[] TryGetValueSource =
        {
            new TestCaseData("present").Returns(Option.From(sentinel)),
            new TestCaseData("nonpresent").Returns(Option<string>.None),
            new TestCaseData("null").Returns(Option<string>.None)
        };

        [TestCaseSource(nameof(TryGetValueSource))]
        public Option<string> Dictionary_TryGetValue(string key)
        {
            // arrange, act
            return _sut.TryGetValue(key);
        }

        [TestCaseSource(nameof(TryGetValueSource))]
        public Option<string> IDictionary_TryGetValue(string key)
        {
            // arrange
            var sut = (IDictionary<string, string>)_sut;

            // arrange, act
            return sut.TryGetValue(key);
        }

        [TestCaseSource(nameof(TryGetValueSource))]
        public Option<string> IReadOnlyDictionary_TryGetValue(string key)
        {
            // arrange
            var sut = (IReadOnlyDictionary<string, string>)_sut;

            // arrange, act
            return sut.TryGetValue(key);

        }
    }
}
