// ReSharper disable All

using System;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    [TestFixture]
    public sealed class ObjectTestFixture
    {
        const string Sentinel = "sentinel";

        #region Pipe

        #region Null Throws

        [Test, Precondition]
        public void Pipe_NullValue_Throws()
        {
            // arrange
            string value = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Pipe(v => v.Length));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("value"));
        }

        [Test, Precondition]
        public void Pipe_NullPiper_Throws()
        {
            // arrange
            var value = Sentinel;
            Func<string, int> piper = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Pipe(piper));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("piper"));
        }

        #endregion

        [Test(Description = "Piping a value through a function should be equal to invoking the " +
                            "function with that value directly.")]
        public void Pipe()
        {
            // arrange
            var value = 0;
            Func<int, int> piper = v => v + 1;

            // act
            var expected = piper(value);
            var actual = value.Pipe(piper);

            // assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

    }
}
