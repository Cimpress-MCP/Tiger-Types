// <copyright file="EitherCollectionExtensions.cs" company="Cimpress, Inc.">
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
    /// specialized for <see cref="Either{TLeft,TRight}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class EitherCollectionExtensions
    {
        /// <summary>Maps a collection of either values to their Right values.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <param name="eitherEnumerableValue">A collection of either values.</param>
        /// <returns>
        /// A collection of the Right values of the elements of <paramref name="eitherEnumerableValue"/>
        /// which are in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherEnumerableValue"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, LinqTunnel]
        public static IEnumerable<TRight> CatRight<TLeft, TRight>(
            [NotNull] this IEnumerable<Either<TLeft, TRight>> eitherEnumerableValue)
        {
            if (eitherEnumerableValue is null) { throw new ArgumentNullException(nameof(eitherEnumerableValue)); }

            return eitherEnumerableValue.Where(ev => ev.IsRight).Select(ev => ev.Value);
        }

        /// <summary>Maps a collection of either values to their Left values.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherEnumerableValue"/>.</typeparam>
        /// <param name="eitherEnumerableValue">A collection of either values.</param>
        /// <returns>
        /// A collection of the Left values of the elements of <paramref name="eitherEnumerableValue"/>
        /// which are in the Left state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherEnumerableValue"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, LinqTunnel]
        public static IEnumerable<TLeft> CatLeft<TLeft, TRight>(
            [NotNull] this IEnumerable<Either<TLeft, TRight>> eitherEnumerableValue)
        {
            if (eitherEnumerableValue is null) { throw new ArgumentNullException(nameof(eitherEnumerableValue)); }

            return eitherEnumerableValue.Where(ev => ev.IsLeft).Select(ev => (TLeft)ev);
        }

        /// <summary>Partitions a collection of either values into two collections.</summary>
        /// <typeparam name="TLeft">The Left type of the element type of <paramref name="eitherCollectionValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the element type of <paramref name="eitherCollectionValue"/>.</typeparam>
        /// <param name="eitherCollectionValue">A collection of either values.</param>
        /// <returns>
        /// A tuple whose first component is the Left values of the elements of <paramref name="eitherCollectionValue"/>
        /// and whose second component is the Right values of the elements of <paramref name="eitherCollectionValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherCollectionValue"/> is <see langword="null"/>.</exception>
        public static (ImmutableArray<TLeft> lefts, ImmutableArray<TRight> rights) Partition<TLeft, TRight>(
            [NotNull] this IReadOnlyCollection<Either<TLeft, TRight>> eitherCollectionValue)
        {
            if (eitherCollectionValue is null) { throw new ArgumentNullException(nameof(eitherCollectionValue)); }

            return eitherCollectionValue.Fold(
                (lefts: ImmutableArray<TLeft>.Empty, rights: ImmutableArray<TRight>.Empty),
                (acc, curr) => curr.Match(
                    left: l => (acc.lefts.Add(l), acc.rights),
                    right: r => (acc.lefts, acc.rights.Add(r))));
        }
    }
}
