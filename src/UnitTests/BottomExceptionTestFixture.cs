// ReSharper disable All

using System;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    /// <summary>
    /// Tests related to <see cref="BottomException"/>.
    /// </summary>
    [TestFixture(TestOf = typeof(BottomException))]
    public sealed class BottomExceptionTestFixture
    {
        const string Sentinel = "sentinel";

        [Test]
        public void Constructor()
        {
            // arrange, act
            var actual = new BottomException();

            // assert
            Assert.Pass();
        }

        [Test]
        public void Constructor_Message_null()
        {
            // arrange, act
            var actual = new BottomException(null);

            // assert
            Assert.That(actual, Has.Message.Contains(typeof(BottomException).FullName));
        }

        [TestCase(Sentinel)]
        [TestCase("")]
        public void Constructor_Message_Value(string message)
        {
            // arrange, act
            var actual = new BottomException(message);

            // assert
            Assert.That(actual, Has.Message.EqualTo(message));
        }

        [Test]
        public void Constructor_MessageException_ValueNull()
        {
            // arrange, act
            var actual = new BottomException(Sentinel, null);

            // assert
            Assert.That(actual, Has.Message.EqualTo(Sentinel).And.InnerException.Null);
        }

        [Test]
        public void Constructor_MessageException_ValueValue()
        {
            // arrange, act
            var actual = new BottomException(Sentinel, new Exception());

            // assert
            Assert.That(actual, Has.Message.EqualTo(Sentinel).And.InnerException.Not.Null);
        }

        [Test]
        public void Constructor_Serialization()
        {
            // arrange
            var serializationInfo = new SerializationInfo(typeof(BottomException), new FormatterConverter());
            new BottomException(Sentinel).GetObjectData(serializationInfo, new StreamingContext());

            // act
            var actual = new FakeBottomException(serializationInfo, new StreamingContext());

            // assert
            Assert.That(actual, Has.Message.EqualTo(Sentinel));
        }
    }
}
