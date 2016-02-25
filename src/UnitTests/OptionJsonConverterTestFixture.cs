// ReSharper disable All

using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Tiger.Types.Properties;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="OptionTypeConverter"/>.</summary>
    [TestFixture(TestOf = typeof(OptionJsonConverter))]
    public sealed class OptionJsonConverterTestFixture
    {
        const string sentinel = "sentinel";
        const string none = @"null";
        const string someInt = @"42";
        const string someString = @"""" + sentinel + @"""";

        static readonly TestCaseData[] DeserializeSource =
        {
            new TestCaseData(none, typeof(Option<int>)).Returns(Option<int>.None),
            new TestCaseData(someInt, typeof(Option<int>)).Returns(Option.From(42)),
            new TestCaseData(none, typeof(Option<string>)).Returns(Option<string>.None),
            new TestCaseData(someString, typeof(Option<string>)).Returns(Option.From(sentinel))
        };

        [TestCaseSource(nameof(DeserializeSource))]
        public object Deserialize(string json, Type serializationType)
        {
            // arrange, act
            return JsonConvert.DeserializeObject(json, serializationType);
        }

        static readonly TestCaseData[] SerializeSource =
        {
            new TestCaseData(Option<int>.None).Returns(none),
            new TestCaseData(Option.From(42)).Returns(someInt),
            new TestCaseData(Option<string>.None).Returns(none),
            new TestCaseData(Option.From(sentinel)).Returns(someString)
        };

        [TestCaseSource(nameof(SerializeSource))]
        public string Serialize(object value)
        {
            // arrange, act
            return JsonConvert.SerializeObject(value);
        }

        [TestCase(typeof(Option<int>), ExpectedResult = true)]
        [TestCase(typeof(Option<string>), ExpectedResult = true)]
        [TestCase(typeof(int), ExpectedResult = false)]
        [TestCase(typeof(string), ExpectedResult = false)]
        public bool CanConvert(Type serializationType)
        {
            // arrange
            var converter = new OptionJsonConverter();

            // act
            return converter.CanConvert(serializationType);
        }

        [Test]
        public void WriteJson()
        {
            // arrange
            var converter = new OptionJsonConverter();
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter);
            var serializer = new JsonSerializer();

            // act
            converter.WriteJson(writer, null, serializer);
            var actual = stringWriter.ToString();

            // assert
            Assert.That(actual, Is.EqualTo(none));
        }
    }
}
