using JetBrains.Annotations;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Extensions to the functionality of the <see cref="UnequalNonNullPair{T}"/> struct.</summary>
    [PublicAPI]
    static class UnequalNonNullPair
    {
        [ContractAnnotation("=>left:notnull,right:notnull")]
        public static void Deconstruct<T>(this UnequalNonNullPair<T> pair, out T left, out T right)
        {
            left = pair.Left;
            right = pair.Right;
        }
    }
}
