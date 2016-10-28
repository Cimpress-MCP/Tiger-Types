using System;
using System.Threading.Tasks;
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

        /// <summary>Splits a value into an either value based on a provided condition.</summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static Either<TValue, TValue> Split<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return splitter(value)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static async Task<Either<TValue, TValue>> Split<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return await splitter(value).ConfigureAwait(false)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }
    }
}
