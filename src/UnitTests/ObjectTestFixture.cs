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
