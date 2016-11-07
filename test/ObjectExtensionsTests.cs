// ReSharper disable All

using System;
using Xunit;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="ObjectExtensions"/>.</summary>
    public sealed class ObjectExtensionsTests
    {
        #region Pipe

        [Fact(DisplayName= "Piping a value through a function is identical to invoking the " +
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
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
