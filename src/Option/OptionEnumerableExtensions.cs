// <copyright file="OptionEnumerableExtensions.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of <see cref="IEnumerable{T}"/>,
    /// specialized for <see cref="Option{TSome}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class OptionEnumerableExtensions
    {
        /// <summary>Maps a collection of optional values to their Some values.</summary>
        /// <typeparam name="TSome">The Some type of the element type of <paramref name="optionEnumerableValue"/>.</typeparam>
        /// <param name="optionEnumerableValue">A collection of optional values.</param>
        /// <returns>
        /// A collection of the Some values of the elements of <paramref name="optionEnumerableValue"/>
        /// which are in the Some state.
        /// </returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<TSome> Cat<TSome>([NotNull] this IEnumerable<Option<TSome>> optionEnumerableValue)
        {
            if (optionEnumerableValue == null) { throw new ArgumentNullException(nameof(optionEnumerableValue)); }

            return optionEnumerableValue.Fold(
                ImmutableList<TSome>.Empty, // ReSharper disable once ConvertClosureToMethodGroup
                (acc, curr) => curr.Match(none: acc, some: c => acc.Add(c)));
        }
    }
}
