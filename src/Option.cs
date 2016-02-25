using System;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Option{TSome}"/>.</summary>
    public static class Option
    {
        /// <summary>Creates an <see cref="Option{TSome}"/> from the provided value.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        [Pure]
        public static Option<TSome> From<TSome>([CanBeNull] TSome value) => new Option<TSome>(value);

        /// <summary>Creates an <see cref="Option{TSome}"/> from the provided value.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        [Pure]
        public static Option<TSome> From<TSome>([CanBeNull] TSome? value)
            where TSome : struct => value ?? Option<TSome>.None;

        /// <summary>
        /// Gets a value that can be converted to an <see cref="Option{TSome}"/> of any Some type.
        /// </summary>
        public static readonly OptionNone None = default(OptionNone);

        /// <summary>Returns the underlying type argument of the specified optional type.</summary>
        /// <param name="optionalType">
        /// A <see cref="Type"/> object that describes a closed generic optional type.
        /// </param>
        /// <returns>
        /// The underlying type argument of <paramref name="optionalType"/> if <paramref name="optionalType"/>
        /// is a closed generic optional type; otherwise <see langword="null" />.
        /// </returns>
        public static Type GetUnderlyingType([NotNull] Type optionalType)
        {
            Requires<ArgumentNullException>(optionalType != null);

            if (!optionalType.IsGenericType ||
                optionalType.IsGenericTypeDefinition)
            { // note(cosborn) Instantiated generics only, please.
                return null;
            }

            return optionalType.GetGenericTypeDefinition() == typeof(Option<>)
                ? optionalType.GetGenericArguments()[0]
                : null;
        }
    }
}
