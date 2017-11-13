using System;
using System.Collections.Generic;
using FsCheck;
using JetBrains.Annotations;
using static JetBrains.Annotations.ImplicitUseTargetFlags;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Generators for unit test data.</summary>
    [UsedImplicitly(Members)]
    public static class Generators
    {
        /// <summary>Generates an arbitrary instance of <see cref="UnequalNonNullPair{T}"/>.</summary>
        /// <typeparam name="T">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<UnequalNonNullPair<T>> UnequalNonNullPair<T>()
            where T : class => Arb.Generate<NonNull<T>>()
            .Two().Select(t => (left: t.Item1.Get, right: t.Item2.Get))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalNonNullPair<T>(t), unnp => (unnp.Left, unnp.Right));

        /// <summary>Generates an arbitrary instance of <see cref="UnequalPair{T}"/>.</summary>
        /// <typeparam name="T">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<UnequalPair<T>> UnequalPair<T>() => Arb.Generate<T>()
            .Two().Select(t => (left: t.Item1, right: t.Item2))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalPair<T>(t), up => (up.Left, up.Right));

        /// <summary>Generates an arbitrary instance of <see cref="System.Version"/>.</summary>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<Version> Version() => Arb.Generate<byte>()
            .Four().Select(t => new Version(t.Item1, t.Item2, t.Item3, t.Item4))
            .ToArbitrary();
    }
}
