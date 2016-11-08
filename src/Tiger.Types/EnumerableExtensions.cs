using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="IEnumerable{T}"/>.</summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or a default value
        /// if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <returns>
        /// <see cref="Option{TSome}.None"/> if <paramref name="source"/> is empty;
        /// otherwise, the first element in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static Option<TSource> FirstOrNone<TSource>(
            [NotNull, ItemNotNull] this IEnumerable<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }

            var list = source as IList<TSource>;
            if (list != null && list.Count > 0)
            {
                return list[0];
            }

            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext()) { return e.Current; }
            }

            return Option<TSource>.None;
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition
        /// or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// <see cref="Option{TSome}.None"/> if <paramref name="source"/> is empty
        /// or if no element passes the test specified by <paramref name="predicate"/>;
        /// otherwise, the first element in <paramref name="source"/> that passes
        /// the test specified by <paramref name="predicate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static Option<TSource> FirstOrNone<TSource>(
            [NotNull, ItemNotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, bool> predicate)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            foreach (var element in source)
            {
                if (predicate(element)) { return element; }
            }

            return Option<TSource>.None;
        }

        /// <summary>Combines the provided seed state with each value of this instance.</summary>
        /// <typeparam name="T">The type of the items contained in <paramref name="collection"/>.</typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="collection">The collection to be folded.</param>
        /// <param name="state">The seed value to be combined with each value of this instance.</param>
        /// <param name="folder">
        /// A function to invoke with the seed value and each value of this instance as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each value of this instance
        /// if this instance is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static TState Fold<T, TState>(
            [NotNull, ItemNotNull] this IEnumerable<T> collection,
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, T, TState> folder)
        {
            if (collection == null) { throw new ArgumentNullException(nameof(collection)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }

            var result = state;
            foreach (var item in collection)
            {
                result = folder(result, item);
                Assume(result != null, Resources.ResultIsNull);
            } // ReSharper disable once AssignNullToNotNullAttribute

            return result;
        }

        /// <summary>
        /// Combines the provided seed state with each value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items contained in <paramref name="collection"/>.</typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="collection">The collection to be folded.</param>
        /// <param name="state">The seed value to be combined with each value of this instance.</param>
        /// <param name="folder">
        /// A n asynchronous function to invoke with the seed value
        /// and each value of this instance as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each value of this instance
        /// if this instance is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static async Task<TState> Fold<T, TState>(
            [NotNull, ItemNotNull] this IEnumerable<T> collection,
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, T, Task<TState>> folder)
        {
            if (collection == null) { throw new ArgumentNullException(nameof(collection)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }

            var result = state;
            foreach (var item in collection)
            {
                result = await folder(result, item).ConfigureAwait(false);
                Assume(result != null, Resources.ResultIsNull);
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }
    }
}
