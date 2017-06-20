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
