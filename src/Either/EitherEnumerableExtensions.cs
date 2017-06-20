using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using JetBrains.Annotations;


namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of <see cref="IEnumerable{T}"/>,
    /// specialized for <see cref="Either{TLeft,TRight}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class EitherEnumerableExtensions
    {
        /// <summary>Maps a collection of either values to their Right values.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <param name="eitherEnumerableValue">A collection of either values.</param>
        /// <returns>
        /// A collection of the Right values of the elements of <paramref name="eitherEnumerableValue"/>
        /// which are in the Right state.
        /// </returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<TRight> CatRight<TLeft, TRight>(
            [NotNull] this IEnumerable<Either<TLeft, TRight>> eitherEnumerableValue)
        {
            if (eitherEnumerableValue == null) { throw new ArgumentNullException(nameof(eitherEnumerableValue)); }

            return eitherEnumerableValue.Fold(
                ImmutableList<TRight>.Empty, // ReSharper disable once ConvertClosureToMethodGroup
                (acc, curr) => curr.Match(left: l => acc, right: r => acc.Add(r)));
        }

        /// <summary>Maps a collection of either values to their Left values.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <param name="eitherEnumerableValue">A collection of either values.</param>
        /// <returns>
        /// A collection of the Left values of the elements of <paramref name="eitherEnumerableValue"/>
        /// which are in the Left state.
        /// </returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<TLeft> CatLeft<TLeft, TRight>(
            [NotNull] this IEnumerable<Either<TLeft, TRight>> eitherEnumerableValue)
        {
            if (eitherEnumerableValue == null) { throw new ArgumentNullException(nameof(eitherEnumerableValue)); }

            return eitherEnumerableValue.Fold(
                ImmutableList<TLeft>.Empty, // ReSharper disable once ConvertClosureToMethodGroup
                (acc, curr) => curr.Match(left: l => acc.Add(l), right: r => acc));
        }

        /// <summary>Partitions a collection of either values into two collections.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <param name="eitherEnumerableValue">A collection of either values.</param>
        /// <returns>
        /// A tuple whose first component is the Left values of the elements of <paramref name="eitherEnumerableValue"/>
        /// and whose second component is the Right values of the elements of <paramref name="eitherEnumerableValue"/>.
        /// </returns>
        public static (IEnumerable<TLeft> lefts, IEnumerable<TRight> rights) Partition<TLeft, TRight>(
            [NotNull] this IEnumerable<Either<TLeft, TRight>> eitherEnumerableValue)
        {
            if (eitherEnumerableValue == null) { throw new ArgumentNullException(nameof(eitherEnumerableValue)); }

            return eitherEnumerableValue.Fold(
                (lefts: ImmutableList<TLeft>.Empty, rights: ImmutableList<TRight>.Empty),
                (acc, curr) => curr.Match(
                    left: l => (acc.lefts.Add(l), acc.rights),
                    right: r => (acc.lefts, acc.rights.Add(r))));
        }
    }
}