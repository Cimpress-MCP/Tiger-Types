using System;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;

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
        public static TOut Pipe<TIn, TOut>(
            [NotNull] this TIn value,
            [NotNull, InstantHandle] Func<TIn, TOut> piper)
        {
            Requires(value != null);
            Requires(piper != null);

            return piper(value);
        }
    }
}
