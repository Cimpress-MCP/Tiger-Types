using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Either{TLeft,TRight}"/>.</summary>
    /// <typeparam name="T">The type of the state that is not specified in the creation method.</typeparam>
    /// <remarks>You can think of this as a variant of <see cref="Either{TLeft,TRight}"/> that is
    /// partially applied with whichever of the Left and Right type is not specified in
    /// the creation method.</remarks>
    public static class Either<T>
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        public static Either<TLeft, T> WithLeft<TLeft>([NotNull] TLeft leftValue)
        {
            Requires(leftValue != null);

            return Either<TLeft, T>.FromLeft(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        public static Either<T, TRight> WithRight<TRight>([NotNull] TRight rightValue)
        {
            Requires(rightValue != null);

            return Either<T, TRight>.FromRight(rightValue);
        }
    }
}
