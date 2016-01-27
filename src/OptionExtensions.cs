using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="object"/>.</summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class OptionExtensions
    {
        /// <summary>Converts an <see cref="Option{T}"/> into a <see cref="Nullable{T}"/>.</summary>
        /// <typeparam name="T">The Some type of <paramref name="value"/>.</typeparam>
        /// <param name="value">THe value to be converted.</param>
        /// <returns>
        /// The Some value of <paramref name="value"/> if it is in the Some state;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        [CanBeNull]
        public static T? ToNullable<T>(this Option<T> value)
            where T : struct => value.Match(
                none: () => (T?)null,
                some: v => v);
    }
}
