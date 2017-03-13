using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="IEnumerable{T}"/>.</summary>
    [PublicAPI]
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

            // note(cosborn) Logic cribbed from FirstOrDefault.
            if (source is IList<TSource> list && list.Count > 0)
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

            // note(cosborn) Logic cribbed from FirstOrDefault.
            foreach (var element in source)
            {
                if (predicate(element)) { return element; }
            }

            return Option<TSource>.None;
        }

        /// <summary>
        /// Maps a transformation over each element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TIn">The element type of <paramref name="enumerableValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="enumerableValue">The value to map.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that is the result of applying <paramref name="mapper"/>
        /// to each element of <paramref name="enumerableValue"/>.</returns>
        [NotNull, ItemNotNull, Pure, LinqTunnel]
        public static IEnumerable<TOut> Map<TIn, TOut>(
            [NotNull, ItemNotNull] this IEnumerable<TIn> enumerableValue,
            [NotNull, InstantHandle] Func<TIn, TOut> mapper)
        {
            if (enumerableValue == null) { throw new ArgumentNullException(nameof(enumerableValue)); }
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return enumerableValue.Select(mapper);
        }

        /// <summary>
        /// Maps a transformation over each element of an <see cref="IEnumerable{T}"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">The element type of <paramref name="enumerableValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="enumerableValue">The value to map.</param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TIn"/>
        /// to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that is the result of applying <paramref name="mapper"/>
        /// to each element of <paramref name="enumerableValue"/>.</returns>
        [NotNull, ItemNotNull, Pure, LinqTunnel]
        public static Task<IEnumerable<TOut>> Map<TIn, TOut>(
            [NotNull, ItemNotNull] this IEnumerable<TIn> enumerableValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> mapper)
        {
            if (enumerableValue == null) { throw new ArgumentNullException(nameof(enumerableValue)); }
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return enumerableValue.Select(mapper).Pipe(Task.WhenAll).Map(Enumerable.AsEnumerable);
        }

        /// <summary>Combines the provided seed state with each element of this instance.</summary>
        /// <typeparam name="T">The element type of <paramref name="collection"/>.</typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="collection">The collection to be folded.</param>
        /// <param name="state">The seed value to be combined with each element of this instance.</param>
        /// <param name="folder">
        /// A function to invoke with the seed value and each element of this instance as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each element of this instance
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
                Assume(result != null, ResultIsNull);
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
        /// An asynchronous function to invoke with the seed value
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
                Assume(result != null, ResultIsNull);
            } // ReSharper disable once AssignNullToNotNullAttribute

            return result;
        }
    }
}
