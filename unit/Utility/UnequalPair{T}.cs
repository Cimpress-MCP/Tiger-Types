using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Represents a pair of values which are not equal to each other.</summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    [UsedImplicitly]
    public readonly struct UnequalPair<T>
        : IEquatable<UnequalPair<T>>
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
        public UnequalPair((T left, T right) pair)
        {
            _pair = pair;
        }

        #region Overrides

        /// <inheritdoc/>
        [NotNull]
        public override string ToString() => $"Unequal {_pair}";

        /// <inheritdoc/>
        public override int GetHashCode() => _pair.GetHashCode();

        #endregion

        #region Operators and Named Alternatives

        /// <inheritdoc/>
        public override bool Equals(object obj) => _pair.Equals(obj);

        /// <summary>Compares two instances of the <see cref="UnequalPair{T}"/> struct for equality.</summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>
        /// <see langword="true"/> if the provided instances are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(UnequalPair<T> left, UnequalPair<T> right) => left.Equals(right);

        /// <summary>Compares two instances of the <see cref="UnequalPair{T}"/> struct for inequality.</summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>
        /// <see langword="true"/> if the provided instances are unequal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(UnequalPair<T> left, UnequalPair<T> right) => !(left == right);

        /// <summary>Converts this instance to the underlying tuple.</summary>
        /// <param name="pair">The pair to convert.</param>
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static implicit operator (T left, T right) (UnequalPair<T> pair) => pair._pair;

        /// <inheritdoc/>
        [SuppressMessage("ReSharper", "ImpureMethodCallOnReadonlyValueField", Justification = "Lies???")]
        public bool Equals(UnequalPair<T> other) => _pair.Equals(other._pair);

        #endregion
    }
}
