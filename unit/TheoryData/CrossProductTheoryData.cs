using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>
    /// Represents a set of data for a theory with 2 parameters. Theory data will be
    /// generated as the cross product of collections.
    /// </summary>
    /// <typeparam name="T1">The first type of the theory data.</typeparam>
    /// <typeparam name="T2">The second type of the theory data.</typeparam>
    [SuppressMessage("Microsoft:Guidelines", "CA1710", Justification = "There is a stronger naming convention.")]
    public sealed class CrossProductTheoryData<T1, T2>
        : TheoryData<T1, T2>
    {
        /// <summary>Initializes a new instance of the <see cref="CrossProductTheoryData{T1,T2}"/> class.</summary>
        /// <param name="collection1">The first collection providing data to the thery.</param>
        /// <param name="collection2">The second collection providing data to the thery.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection1"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="collection2"/> is <see langword="null"/>.</exception>
        public CrossProductTheoryData(
            [NotNull] IReadOnlyCollection<T1> collection1,
            [NotNull] IReadOnlyCollection<T2> collection2)
        {
            if (collection1 is null) { throw new ArgumentNullException(nameof(collection1)); }
            if (collection2 is null) { throw new ArgumentNullException(nameof(collection2)); }

            foreach (var item1 in collection1)
            {
                foreach (var item2 in collection2)
                {
                    Add(item1, item2);
                }
            }
        }
    }
}
