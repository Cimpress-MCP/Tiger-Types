using FsCheck.Xunit;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="ObjectExtensions"/>.</summary>
    public static class ObjectExtensionsTests
    {
        #region Pipe

        [Property(DisplayName = "Piping a value through a function is identical to invoking the " +
            "function with that value directly.")]
        public static void Pipe(int value)
        {
            // arrange
            int Piper(int v) => v + 1;

            // act
            var actual = value.Pipe(Piper);

            // assert
            var expected = Piper(value);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
