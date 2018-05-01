// <copyright file="CollectionTaskExtensions.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of <see cref="Task{TResult}"/>,
    /// specialized for collections.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class CollectionTaskExtensions
    {
        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the member type of <paramref name="enumerableTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> with the transformation applied to each member.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<IEnumerable<TOut>> MapT<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<IEnumerable<TIn>> enumerableTaskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> mapper)
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (mapper is null) { throw new ArgumentNullException(nameof(mapper)); }

            return enumerableTaskValue.Map(ev => ev.Map(mapper));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the member type of <paramref name="enumerableTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> with the transformation applied to each member.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<ImmutableArray<TOut>> MapTAsync<TIn, TOut>(
            [NotNull, ItemNotNull] this Task<IEnumerable<TIn>> enumerableTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> mapper)
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (mapper is null) { throw new ArgumentNullException(nameof(mapper)); }

            return enumerableTaskValue.Bind(ev => ev.MapAsync(mapper));
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with each value of
        /// the result of a <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the Result type of <paramref name="enumerableTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to fold.</param>
        /// <param name="folder">
        /// A function to invoke with the seed value and each element of
        /// the result of <paramref name="enumerableTaskValue"/> as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each element of
        /// the result of <paramref name="enumerableTaskValue"/> if the result of
        /// <paramref name="enumerableTaskValue"/> is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<TState> FoldT<T, TState>(
            [NotNull, ItemNotNull] this Task<IReadOnlyCollection<T>> enumerableTaskValue,
            [NotNull, InstantHandle] Func<TState, T, TState> folder)
            where TState : struct
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            return enumerableTaskValue.Map(ev => ev.Fold(folder));
        }

        /// <summary>
        /// Combines the provided seed state with each value of
        /// the result of a <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the Result type of <paramref name="enumerableTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to fold.</param>
        /// <param name="state">
        /// The seed value to combine with each element of
        /// the result of <paramref name="enumerableTaskValue"/>.
        /// </param>
        /// <param name="folder">
        /// A function to invoke with the seed value and each element of
        /// the result of <paramref name="enumerableTaskValue"/> as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each element of
        /// the result of <paramref name="enumerableTaskValue"/> if the result of
        /// <paramref name="enumerableTaskValue"/> is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static Task<TState> FoldT<T, TState>(
            [NotNull, ItemNotNull] this Task<IReadOnlyCollection<T>> enumerableTaskValue,
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, T, TState> folder)
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            return enumerableTaskValue.Map(ev => ev.Fold(state, folder));
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with each value of
        /// the result of a <see cref="Task{TResult}"/>, asynchronously.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the Result type of <paramref name="enumerableTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to fold.</param>
        /// <param name="folder">
        /// An asynchronous function to invoke with the seed value and each element of
        /// the result of <paramref name="enumerableTaskValue"/> as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each element of
        /// the result of <paramref name="enumerableTaskValue"/> if the result of
        /// <paramref name="enumerableTaskValue"/> is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<TState> FoldTAsync<T, TState>(
            [NotNull, ItemNotNull] this Task<IReadOnlyCollection<T>> enumerableTaskValue,
            [NotNull, InstantHandle] Func<TState, T, Task<TState>> folder)
            where TState : struct
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            return enumerableTaskValue.Bind(ev => ev.FoldAsync(folder));
        }

        /// <summary>
        /// Combines the provided seed state with each value of
        /// the result of a <see cref="Task{TResult}"/>, asynchronously.
        /// </summary>
        /// <typeparam name="T">
        /// The element type of the Result type of <paramref name="enumerableTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="enumerableTaskValue">The <see cref="Task{TResult}"/> to fold.</param>
        /// <param name="state">
        /// The seed value to combine with each element of
        /// the result of <paramref name="enumerableTaskValue"/>.
        /// </param>
        /// <param name="folder">
        /// An asynchronous function to invoke with the seed value and each element of
        /// the result of <paramref name="enumerableTaskValue"/> as the arguments.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with each element of
        /// the result of <paramref name="enumerableTaskValue"/> if the result of
        /// <paramref name="enumerableTaskValue"/> is not empty; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerableTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public static Task<TState> FoldTAsync<T, TState>(
            [NotNull, ItemNotNull] this Task<IReadOnlyCollection<T>> enumerableTaskValue,
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, T, Task<TState>> folder)
        {
            if (enumerableTaskValue is null) { throw new ArgumentNullException(nameof(enumerableTaskValue)); }
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            return enumerableTaskValue.Bind(ev => ev.FoldAsync(state, folder));
        }
    }
}
