// ReSharper disable All
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="OptionTypeConverter"/>.</summary>
    [TestFixture(TestOf = typeof(OptionTypeConverter))]
    public sealed class OptionTypeConverterTestFixture
    {
        [TestCase(typeof(Option<int>), typeof(int), ExpectedResult = true)]
        [TestCase(typeof(Option<string>), typeof(string), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(string), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(int?), ExpectedResult = true)]
        // note(cosborn) Yes: `TypeDescriptor.GetConverter(typeof(int)).CanConvertFrom(typeof(int)) == false`.
        [TestCase(typeof(Option<int>), typeof(Option<int>), ExpectedResult = false)]
        [TestCase(typeof(Option<int>), typeof(long), ExpectedResult = false)]
        [TestCase(typeof(Option<string>), typeof(int), ExpectedResult = false)]
        public bool CanConvertFrom(Type converterType, Type conversionType)
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(converterType);

            // act, assert
            return converter.CanConvertFrom(conversionType);
        }

        static readonly TestCaseData[] ConvertFromSource =
        {
            new TestCaseData(typeof(Option<int>), 33).Returns(Option.From(33)),
            new TestCaseData(typeof(Option<int>), "33").Returns(Option.From(33)),
            new TestCaseData(typeof(Option<int>), (int?)33).Returns(Option.From(33)),
            new TestCaseData(typeof(Option<int>), string.Empty).Returns(Option<int>.None),
            new TestCaseData(typeof(Option<int>), null).Returns(Option<int>.None)
        };

        [TestCaseSource(nameof(ConvertFromSource))]
        public object ConvertFrom(Type converterType, object value)
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(converterType);

            // act, assert
            return converter.ConvertFrom(value);
        }
        [Test]

        public void ConvertFrom_Fail()
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(typeof(Option<int>));

            // act
            var ex = Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(33L));

            // assert
            Assert.That(ex, Has.Message.Contains(nameof(OptionTypeConverter)));
        }

        [TestCase(typeof(Option<int>), typeof(int), ExpectedResult = true)]
        [TestCase(typeof(Option<string>), typeof(string), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(string), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(int?), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(Option<int>), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(long), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(InstanceDescriptor), ExpectedResult = true)]
        [TestCase(typeof(Option<long>), typeof(int), ExpectedResult = true)]
        [TestCase(typeof(Option<int>), typeof(List<int>), ExpectedResult = false)]
        public bool CanConvertTo(Type converterType, Type conversionType)
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(converterType);

            // act, assert
            return converter.CanConvertTo(conversionType);
        }

        static readonly TestCaseData[] ConvertToSource =
        {
            new TestCaseData(typeof(Option<int>), typeof(int), Option.From(33)).Returns(33),
            new TestCaseData(typeof(Option<string>), typeof(string), Option<string>.None).Returns(string.Empty),
            new TestCaseData(typeof(Option<int>), typeof(int?), Option<int>.None).Returns(null),
            new TestCaseData(typeof(Option<string>), typeof(string), Option.From("megatron")).Returns("megatron"),
            new TestCaseData(typeof(Option<string>), typeof(string), null).Returns(string.Empty),
            new TestCaseData(typeof(Option<int>), typeof(Option<int>), Option.From(33)).Returns(Option.From(33))
        };

        [TestCaseSource(nameof(ConvertToSource))]
        public object ConvertTo(Type converterType, Type conversionType, object value)
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(converterType);

            // act, assert
            return converter.ConvertTo(value, conversionType);
        }

        static readonly TestCaseData[] ConvertTo_FailSource =
        {
            new TestCaseData(typeof(Option<int>), typeof(List<int>), Option.From(33)),
            new TestCaseData(typeof(Option<int>), typeof(int), 33),
            new TestCaseData(typeof(Option<int>), typeof(Option<int>), 33),
            new TestCaseData(typeof(Option<int>), typeof(IEnumerable<int>), new List<int> { 33 })
        };

        [TestCaseSource(nameof(ConvertTo_FailSource))]
        public void ConvertTo_Fail(Type converterType, Type conversionType, object value)
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(converterType);

            // act
            var ex = Assert.Throws<NotSupportedException>(() => converter.ConvertTo(value, conversionType));

            // assert
            Assert.That(ex, Has.Message.Contains(nameof(OptionTypeConverter)));
        }

        [Test]
        public void ConvertTo_InstanceDescriptor()
        {
            // arrange
            var converter = TypeDescriptor.GetConverter(typeof(Option<int>));

            // act
            var actual = (InstanceDescriptor)converter.ConvertTo(Option.From(33), typeof(InstanceDescriptor));

            // assert
            Assert.That(Option.From(33), Is.EqualTo(actual.Invoke()));
        }
    }
}
