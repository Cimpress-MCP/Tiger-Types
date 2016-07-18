using JetBrains.Annotations;
using System;
using System.Collections.Generic;

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
        public static Option<TSome> From<TSome>([CanBeNull] TSome value) => value == null
            ? Option<TSome>.None
            : new Option<TSome>(value);

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
            where TSome : struct => value == null
            ? Option<TSome>.None
            : new Option<TSome>(value.Value);

        /// <summary>
        /// A value that can be converted to an <see cref="Option{TSome}"/> of any Some type.
        /// </summary>
        public static readonly OptionNone None = default(OptionNone);

        #region Utilities

        /// <summary>Returns the underlying type argument of the specified optional type.</summary>
        /// <param name="optionalType">
        /// A <see cref="Type"/> object that describes a closed generic optional type.
        /// </param>
        /// <returns>
        /// The underlying type argument of <paramref name="optionalType"/> if <paramref name="optionalType"/>
        /// is a closed generic optional type; otherwise <see langword="null" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionalType"/> is <see langword="null" />.</exception>
        [Pure, CanBeNull]
        public static Type GetUnderlyingType([NotNull] Type optionalType)
        {
            if (optionalType == null) { throw new ArgumentNullException(nameof(optionalType)); }

            if (!optionalType.IsGenericType ||
                optionalType.IsGenericTypeDefinition)
            { // note(cosborn) Instantiated generics only, please.
                return null;
            }

            return optionalType.GetGenericTypeDefinition() == typeof(Option<>)
                ? optionalType.GetGenericArguments()[0]
                : null;
        }

        /// <summary>
        /// Indicates whether two specified <see cref="Option{TSome}"/> objects are equal
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared for equality.</typeparam>
        /// <param name="o1">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">An <see cref="Option{TSome}"/> object.</param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="o1"/> parameter is equal to the
        /// <paramref name="o2"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool OptionalEquals<TSome>(this Option<TSome> o1, Option<TSome> o2)
        {
            if (o1.IsNone && o2.IsNone) { return true; }
            if (o1.IsNone || o2.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return EqualityComparer<TSome>.Default.Equals(o1.SomeValue, o2.SomeValue);
        }

        /// <summary>
        /// Indicates whether two specified <see cref="Option{TSome}"/> objects are equal
        /// by using the specified equality comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared for equality.</typeparam>
        /// <param name="o1">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="equalityComparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="o1"/> parameter is equal to the
        /// <paramref name="o2"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool OptionalEquals<TSome>(
            this Option<TSome> o1,
            Option<TSome> o2,
            [CanBeNull] IEqualityComparer<TSome> equalityComparer)
        {
            if (o1.IsNone && o2.IsNone) { return true; }
            if (o1.IsNone || o2.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return (equalityComparer ?? EqualityComparer<TSome>.Default).Equals(o1.SomeValue, o2.SomeValue);
        }

        /// <summary>
        /// Compares the relative values of two <see cref="Option{TSome}"/> objects
        /// using the default comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared.</typeparam>
        /// <param name="o1">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">An <see cref="Option{TSome}"/> object.</param>
        /// <returns>
        /// An integer that indicates the relative values of the <paramref name="o1"/>
        /// and <paramref name="o2"/> parameters.
        /// </returns>
        [Pure]
        public static int OptionalCompare<TSome>(this Option<TSome> o1, Option<TSome> o2)
        {
            if (o1.IsNone && o2.IsNone) { return 0; }
            if (o1.IsNone || o2.IsNone)
            {
                return o1.IsNone ? -1 : 1;
            }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return Math.Sign(Comparer<TSome>.Default.Compare(o1.SomeValue, o2.SomeValue));
        }

        /// <summary>
        /// Compares the relative values of two <see cref="Option{TSome}"/> objects
        /// using the specified comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared.</typeparam>
        /// <param name="o1">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">An <see cref="Option{TSome}"/> object.</param>
        /// <param name="comparer">A comparer to compare values.</param>
        /// <returns>
        /// An integer that indicates the relative values of the <paramref name="o1"/>
        /// and <paramref name="o2"/> parameters.
        /// </returns>
        [Pure]
        public static int OptionalCompare<TSome>(
            this Option<TSome> o1,
            Option<TSome> o2,
            IComparer<TSome> comparer)
        {
            if (o1.IsNone && o2.IsNone) { return 0; }
            if (o1.IsNone || o2.IsNone)
            {
                return o1.IsNone ? -1 : 1;
            }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return Math.Sign((comparer ?? Comparer<TSome>.Default).Compare(o1.SomeValue, o2.SomeValue));
        }

        #endregion
    }
}
