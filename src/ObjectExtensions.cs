using JetBrains.Annotations;
using System;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="object"/>.</summary>
    public static class ObjectExtensions
    {
        /// <summary>Invokes <paramref name="piper"/> with <paramref name="value"/> as its argument.</summary>
        /// <typeparam name="TIn">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The type to produce.</typeparam>
        /// <param name="value">The value to be piped.</param>
        /// <param name="piper">A function to invoke with <paramref name="value"/> as its argument.</param>
        /// <returns>The value of invoking <paramref name="value"/> to <paramref name="piper"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="piper"/> is <see langword="null"/>.</exception>
        public static TOut Pipe<TIn, TOut>(
            [NotNull] this TIn value,
            [NotNull, InstantHandle] Func<TIn, TOut> piper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (piper == null) { throw new ArgumentNullException(nameof(piper)); }

            return piper(value);
        }
    }
}
