using System;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <content>Operators and named alternatives.</content>
    public partial class Union<T1, T2>
    {
        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [CanBeNull]
        public static implicit operator Union<T1, T2>([CanBeNull] T1 value) => value == null
            ? null
            : new Union<T1, T2>(value);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [CanBeNull]
        public static implicit operator Union<T1, T2>([CanBeNull] T2 value) => value == null
            ? null
            : new Union<T1, T2>(value);

        /// <summary>Unwraps the first value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        [NotNull]
        public static explicit operator T1([NotNull] Union<T1, T2> value) => value.Value1;

        /// <summary>Unwraps the second value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        [NotNull]
        public static explicit operator T2([NotNull] Union<T1, T2> value) => value.Value2;

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==([CanBeNull] Union<T1, T2> left, [CanBeNull] Union<T1, T2> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=([CanBeNull] Union<T1, T2> left, [CanBeNull] Union<T1, T2> right) =>
            !(left == right);
    }
}
