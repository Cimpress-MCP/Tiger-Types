using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Option{TSome}"/>.</summary>
    [PublicAPI]
    public static partial class Option
    {
        /// <summary>
        /// A value that can be converted to an <see cref="Option{TSome}"/> of any Some type.
        /// </summary>
        public static readonly OptionNone None = default(OptionNone);

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

        #region Extensions

        /// <summary>Converts an <see cref="Option{TSome}"/> into a <see cref="Nullable{T}"/>.</summary>
        /// <typeparam name="TSome">The Some type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <returns>
        /// The Some value of <paramref name="value"/> if it is in the Some state;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        [Pure, CanBeNull]
        public static TSome? ToNullable<TSome>(this Option<TSome> value)
            where TSome : struct => value.IsNone
                ? (TSome?)null
                : value.Value;

        /// <summary>
        /// Converts an <see cref="Option{TSome}"/> into an <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of <paramref name="fallback"/>.</typeparam>
        /// <typeparam name="TSome">The Some type of <paramref name="value"/></typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <param name="fallback">The value to use as a fallback.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the Some value of <paramref name="value"/> if <paramref name="value"/>
        /// is in the Some state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Left state with its Left value set to <paramref name="fallback"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TSome> ToEither<TLeft, TSome>(
            this Option<TSome> value,
            [NotNull] TLeft fallback)
        {
            if (fallback == null) { throw new ArgumentNullException(nameof(fallback)); }

            return value.Map(v => new Either<TLeft, TSome>(v)).GetValueOrDefault(fallback);
        }

        /// <summary>
        /// Converts an <see cref="Option{TSome}"/> into an <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of <paramref name="fallback"/>.</typeparam>
        /// <typeparam name="TSome">The Some type of <paramref name="value"/></typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <param name="fallback">A function producing the value to use as a fallback.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the Some value of <paramref name="value"/> if <paramref name="value"/>
        /// is in the Some state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Left state with its Left value set to <paramref name="fallback"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TSome> ToEither<TLeft, TSome>(
            this Option<TSome> value,
            [NotNull, InstantHandle] Func<TLeft> fallback)
        {
            if (fallback == null) { throw new ArgumentNullException(nameof(fallback)); }

            return value.Map(v => new Either<TLeft, TSome>(v)).GetValueOrDefault(() => fallback());
        }

        /// <summary>
        /// Converts an <see cref="Option{TSome}"/> into an <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of <paramref name="fallback"/>.</typeparam>
        /// <typeparam name="TSome">The Some type of <paramref name="value"/></typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <param name="fallback">A function producing the value to use as a fallback.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the Some value of <paramref name="value"/> if <paramref name="value"/>
        /// is in the Some state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Left state with its Left value set to <paramref name="fallback"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Either<TLeft, TSome>> ToEither<TLeft, TSome>(
            this Option<TSome> value,
            [NotNull, InstantHandle] Func<Task<TLeft>> fallback)
        {
            if (fallback == null) { throw new ArgumentNullException(nameof(fallback)); }

            return value.Map(v => new Either<TLeft, TSome>(v))
                .GetValueOrDefault(async () => await fallback().ConfigureAwait(false));
        }

        /// <summary>
        /// Joins an optional <see cref="Option{TSome}"/> into an <see cref="Option{TSome}"/>.
        /// </summary>
        /// <typeparam name="TSome">The Some type of <paramref name="optionOptionValue"/>.</typeparam>
        /// <param name="optionOptionValue">The value to be joined.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="optionOptionValue"/>
        /// is in the None state; otherwise, the Some value of <paramref name="optionOptionValue"/>.
        /// </returns>
        public static Option<TSome> Join<TSome>(this Option<Option<TSome>> optionOptionValue) =>
            optionOptionValue.GetValueOrDefault();

        #endregion

        #region Split

        /// <summary>Splits a value into an optional value based on a provided condition.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/> or if the condition <paramref name="splitter"/>
        /// is not satisfied by <paramref name="value"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TSome> Split<TSome>(
            [CanBeNull] TSome value,
            [NotNull, InstantHandle] Func<TSome, bool> splitter)
        {
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return From(value).Filter(splitter);
        }

        /// <summary>Splits a value into an optional value based on a provided condition.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/> or if the condition <paramref name="splitter"/>
        /// is not satisfied by <paramref name="value"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Option<TSome> Split<TSome>(
            [CanBeNull] TSome? value,
            [NotNull, InstantHandle] Func<TSome, bool> splitter)
            where TSome : struct
        {
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return From(value).Filter(splitter);
        }

        /// <summary>Splits a value into an optional value based on a provided condition, asynchronously.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/> or if the condition <paramref name="splitter"/>
        /// is not satisfied by <paramref name="value"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull, PublicAPI]
        public static Task<Option<TSome>> Split<TSome>(
            [CanBeNull] TSome value,
            [NotNull, InstantHandle] Func<TSome, Task<bool>> splitter)
        {
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return From(value).Filter(splitter);
        }

        /// <summary>Splits a value into an optional value based on a provided condition, asynchronously.</summary>
        /// <typeparam name="TSome">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/> or if the condition <paramref name="splitter"/>
        /// is not satisfied by <paramref name="value"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state with its Some value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TSome>> Split<TSome>(
            [CanBeNull] TSome? value,
            [NotNull] [InstantHandle] Func<TSome, Task<bool>> splitter)
            where TSome : struct
        {
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return From(value).Filter(splitter);
        }

        #endregion

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

            if (!optionalType.IsConstructedGenericType)
            { // note(cosborn) Constructed generics only, please.
                return null;
            }

            return optionalType.GetGenericTypeDefinition() == typeof(Option<>)
                ? optionalType.GenericTypeArguments[0]
                : null;
        }

        /// <summary>
        /// Indicates whether two specified <see cref="Option{TSome}"/> objects are equal
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared for equality.</typeparam>
        /// <param name="o1">The left <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">The right <see cref="Option{TSome}"/> object.</param>
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
            return EqualityComparer<TSome>.Default.Equals(o1.Value, o2.Value);
        }

        /// <summary>
        /// Indicates whether two specified <see cref="Option{TSome}"/> objects are equal
        /// by using the specified equality comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared for equality.</typeparam>
        /// <param name="o1">The left <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">The right <see cref="Option{TSome}"/> object.</param>
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
            return (equalityComparer ?? EqualityComparer<TSome>.Default).Equals(o1.Value, o2.Value);
        }

        /// <summary>
        /// Compares the relative values of two <see cref="Option{TSome}"/> objects
        /// using the default comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared.</typeparam>
        /// <param name="o1">The left <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">The right <see cref="Option{TSome}"/> object.</param>
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
            return Math.Sign(Comparer<TSome>.Default.Compare(o1.Value, o2.Value));
        }

        /// <summary>
        /// Compares the relative values of two <see cref="Option{TSome}"/> objects
        /// using the specified comparer.
        /// </summary>
        /// <typeparam name="TSome">The Some type of the objects to be compared.</typeparam>
        /// <param name="o1">The left <see cref="Option{TSome}"/> object.</param>
        /// <param name="o2">The right <see cref="Option{TSome}"/> object.</param>
        /// <param name="comparer">A comparer to compare values.</param>
        /// <returns>
        /// An integer that indicates the relative values of the <paramref name="o1"/>
        /// and <paramref name="o2"/> parameters.
        /// </returns>
        [Pure]
        public static int OptionalCompare<TSome>(
            this Option<TSome> o1,
            Option<TSome> o2,
            [CanBeNull] IComparer<TSome> comparer)
        {
            if (o1.IsNone && o2.IsNone) { return 0; }
            if (o1.IsNone || o2.IsNone)
            {
                return o1.IsNone ? -1 : 1;
            }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return Math.Sign((comparer ?? Comparer<TSome>.Default).Compare(o1.Value, o2.Value));
        }

        #endregion
    }
}
