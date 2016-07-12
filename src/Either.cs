using System;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Either{TLeft,TRight}"/>.</summary>
    public static class Either
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new Either<TLeft, TRight>(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new Either<TLeft, TRight>(rightValue);
        }
    }
}
