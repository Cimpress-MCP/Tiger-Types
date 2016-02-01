using System.Diagnostics;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Option{T}"/>.</summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class Option
    {
        /// <summary>Creates an <see cref="Option{T}"/> from the provided value.</summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{T}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> From<T>([CanBeNull] T value) => Option<T>.From(value);

        /// <summary>Creates an <see cref="Option{T}"/> from the provided value.</summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{T}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> From<T>([CanBeNull] T? value)
            where T : struct => value == null ? None : Option<T>.From((T)value);

        /// <summary>
        /// Gets a value that can be converted to an <see cref="Option{T}"/> of any Some type.
        /// </summary>
        public static OptionNone None => default(OptionNone);
    }
}
