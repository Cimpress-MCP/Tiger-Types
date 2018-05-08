// <copyright file="DictionaryExtensions.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;

namespace Tiger.Types
{
    /* note(cosborn)
     * This looks a little weird only because it's not the case that
     * IDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>.
     * (Which was for reasonable reasons.) I duplicate the extension methods in order to
     * disambiguate the cases for calling one on an instance of Dictionary<TKey, TValue>.
     */

    /// <summary>Extensions to the functionality of dictionaries.</summary>
    public static class DictionaryExtensions
    {
        /// <summary>Gets the value associated with the specified key, if present.</summary>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <typeparam name="TKey">The key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The value type of <paramref name="dictionary"/>.</typeparam>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="key"/> is not
        /// associated with a value in <paramref name="dictionary"/>, or if it is associated with
        /// the value <see langword="null"/>;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value
        /// set to the value associated with <paramref name="key"/> in <paramref name="dictionary"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TValue> TryGetValue<TKey, TValue>(
            [NotNull] this Dictionary<TKey, TValue> dictionary,
            [NotNull] TKey key)
        {
            if (dictionary is null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            return !dictionary.TryGetValue(key, out var value)
                ? Option<TValue>.None
                : value;
        }

        /// <summary>Gets the value associated with the specified key, if present.</summary>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <typeparam name="TKey">The key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The value type of <paramref name="dictionary"/>.</typeparam>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="key"/> is not
        /// associated with a value in <paramref name="dictionary"/>, or if it is associated with
        /// the value <see langword="null"/>;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value
        /// set to the value associated with <paramref name="key"/> in <paramref name="dictionary"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TValue> TryGetValue<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key)
        {
            if (dictionary is null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            return !dictionary.TryGetValue(key, out var value)
                ? Option<TValue>.None
                : value;
        }

        /// <summary>Gets the value associated with the specified key, if present.</summary>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <typeparam name="TKey">The key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The value type of <paramref name="dictionary"/>.</typeparam>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="key"/> is not
        /// associated with a value in <paramref name="dictionary"/>, or if it is associated with
        /// the value <see langword="null"/>;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value
        /// set to the value associated with <paramref name="key"/> in <paramref name="dictionary"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TValue> TryGetValue<TKey, TValue>(
            [NotNull] this IReadOnlyDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key)
        {
            if (dictionary is null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            return !dictionary.TryGetValue(key, out var value)
                ? Option<TValue>.None
                : value;
        }

        /// <summary>Gets the value associated with the specified key, if present.</summary>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <typeparam name="TKey">The key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The value type of <paramref name="dictionary"/>.</typeparam>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="key"/> is not
        /// associated with a value in <paramref name="dictionary"/>, or if it is associated with
        /// the value <see langword="null"/>;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value
        /// set to the value associated with <paramref name="key"/> in <paramref name="dictionary"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TValue> TryGetValue<TKey, TValue>(
            [NotNull] this IImmutableDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key)
        {
            if (dictionary is null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            return !dictionary.TryGetValue(key, out var value)
                ? Option<TValue>.None
                : value;
        }
    }
}
