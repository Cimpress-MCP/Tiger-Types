// <copyright file="TryCollectionExtensions.cs" company="Cimpress, Inc.">
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
using System.Linq;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of collections,
    /// specialized for <see cref="Try{TErr, TOk}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class TryCollectionExtensions
    {
        /// <summary>Maps a collection of try values to their Ok values.</summary>
        /// <typeparam name="TErr">The Err type of the element type of <paramref name="tryEnumerableValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the element type of <paramref name="tryEnumerableValue"/>.</typeparam>
        /// <param name="tryEnumerableValue">A collection of try values.</param>
        /// <returns>
        /// A collection of the Ok values of the elements of <paramref name="tryEnumerableValue"/>
        /// which are in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryEnumerableValue"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, LinqTunnel]
        public static IEnumerable<TOk> CatOk<TErr, TOk>(
            [NotNull] this IEnumerable<Try<TErr, TOk>> tryEnumerableValue)
        {
            if (tryEnumerableValue is null) { throw new ArgumentNullException(nameof(tryEnumerableValue)); }

            return tryEnumerableValue.Where(tv => tv.IsOk).Select(tv => tv.Value);
        }

        /// <summary>Maps a collection of try values to their Err values.</summary>
        /// <typeparam name="TErr">The Err type of the element type of <paramref name="tryEnumerableValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the element type of <paramref name="tryEnumerableValue"/>.</typeparam>
        /// <param name="tryEnumerableValue">A collection of try values.</param>
        /// <returns>
        /// A collection of the Err values of the elements of <paramref name="tryEnumerableValue"/>
        /// which are in the Err state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryEnumerableValue"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, LinqTunnel]
        public static IEnumerable<TErr> CatErr<TErr, TOk>(
            [NotNull] this IEnumerable<Try<TErr, TOk>> tryEnumerableValue)
        {
            if (tryEnumerableValue is null) { throw new ArgumentNullException(nameof(tryEnumerableValue)); }

            return tryEnumerableValue.Where(tv => tv.IsErr).Select(tv => (TErr)tv);
        }

        /// <summary>Partitions a collection of try values into two collections.</summary>
        /// <typeparam name="TErr">The Err type of the element type of <paramref name="tryCollectionValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the element type of <paramref name="tryCollectionValue"/>.</typeparam>
        /// <param name="tryCollectionValue">A collection of try values.</param>
        /// <returns>
        /// A tuple whose first component if the Err values of the elements of <paramref name="tryCollectionValue"/>
        /// and whose second component is the Ok values of the elements of <paramref name="tryCollectionValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryCollectionValue"/> is <see langword="null"/>.</exception>
        public static (ImmutableArray<TErr> errs, ImmutableArray<TOk> oks) Partition<TErr, TOk>(
            [NotNull] this IReadOnlyCollection<Try<TErr, TOk>> tryCollectionValue)
        {
            if (tryCollectionValue is null) { throw new ArgumentNullException(nameof(tryCollectionValue)); }

            return tryCollectionValue.Fold(
                (errs: ImmutableArray<TErr>.Empty, oks: ImmutableArray<TOk>.Empty),
                (acc, curr) => curr.Match(
                    none: acc,
                    err: e => (acc.errs.Add(e), acc.oks),
                    ok: v => (acc.errs, acc.oks.Add(v))));
        }
    }
}
