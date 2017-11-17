using FsCheck.Xunit;
using Xunit;

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
            int Piper(int v) => v + 1;

            Assert.Equal(Piper(value), value.Pipe(Piper));
        }

        #endregion
    }
}
