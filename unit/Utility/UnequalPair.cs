using JetBrains.Annotations;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Extensions to the functionality of the <see cref="UnequalPair{T}"/> struct.</summary>
    [PublicAPI]
    static class UnequalPair
    {
        public static void Deconstruct<T>(this UnequalPair<T> pair, out T left, out T right)
        {
            left = pair.Left;
            right = pair.Right;
        }
    }
}
