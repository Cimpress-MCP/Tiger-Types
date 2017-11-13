using JetBrains.Annotations;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Represents a pair of values which are neither <see langword="null"/> nor equal to each other.</summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    public struct UnequalNonNullPair<T>
    {
        /// <summary>Gets the left member of the pair.</summary>
        public T Left => _pair.left;

        /// <summary>
        /// Gets the right member of the pair.
        /// </summary>
        public T Right => _pair.right;

        readonly (T left, T right) _pair;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnequalPair{T}"/> struct.
        /// </summary>
        /// <param name="pair">The underlying tuple.</param>
        public UnequalNonNullPair((T left, T right) pair)
        {
            _pair = pair;
        }

        /// <inheritdoc/>
        [NotNull]
        public override string ToString() => $"UnequalNotNull {_pair}";

        /// <summary>Converts this instance to the underlying tuple.</summary>
        /// <returns>A value tuple containing the same values as this instance.</returns>
        public (T left, T right) ToValueTuple() => _pair;

        /// <summary>Converts this instance to the underlying tuple.</summary>
        /// <param name="pair">The pair to convert.</param>
        public static implicit operator (T left, T right) (UnequalNonNullPair<T> pair) => pair._pair;
    }
}
