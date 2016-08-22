// ReSharper disable All
using NUnit.Framework;
using System;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="ObjectExtensions"/>.</summary>
    [TestFixture(TestOf = typeof(ObjectExtensions))]
    public sealed class ObjectExtensionsTestFixture
    {
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
