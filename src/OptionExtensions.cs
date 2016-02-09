using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="Option{TSome}"/>.</summary>
    public static class OptionExtensions
    {
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

        #region LINQ

        /// <summary>Determines whether an optional value contains a value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Option{TSome}"/> to check for a None state.</param>
        /// <returns>
        /// <see langword="true"/> if the optional value contains a value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Any<TSource>(this Option<TSource> source) => source.IsSome;

        /// <summary>
        /// Determines whether an optional value contains a value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Option{TSome}"/> whose Some value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Some value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Some value of the source optional value passes the test
        /// in the specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Any<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires(predicate != null);

            return source.Filter(predicate).IsSome;
        }

        /// <summary>
        /// Determines whether an optional value contains a value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Option{TSome}"/> whose Some value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Some value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Some value of the source optional value passes the test
        /// in the specified predicate, or if the source optional value is in the None state;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool All<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires(predicate != null);
            
            return source.IsNone || source.Filter(predicate).IsSome;
        }

        /// <summary>
        /// Determines whether an optional value contains a specified value
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value in which to locate a value.</param>
        /// <param name="value">The value to locate in the optional value.</param>
        /// <returns>
        /// <see langword="true"/> if the optional value contains a Some value that has the specified value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Contains<TSource>(this Option<TSource> source, [NotNull] TSource value)
        {
            Requires(value != null);

            return source.Any(v => EqualityComparer<TSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether an optional value contains a specified value
        /// by using a specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value in which to locate a value.</param>
        /// <param name="value">The value to locate in the optional value.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the optional value contains a Some value that has the specified value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Contains<TSource>(
            this Option<TSource> source,
            [NotNull] TSource value,
            [NotNull] IEqualityComparer<TSource> comparer)
        {
            Requires(value != null);
            Requires(comparer != null);

            return source.Any(v => comparer.Equals(v, value));
        }

        /// <summary>Filters the Some value of an optional value based on a predicate.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to filter.</param>
        /// <param name="predicate">A function to test the Some value for a condition.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> that contains the value from the input
        /// option value that satifies the condition.
        /// </returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TSource> Where<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires(predicate != null);

            return source.Filter(predicate);
        }

        /// <summary>Projects the Some value of an optional value into a new form.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An optional value to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to the Some value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> whose Some value is the result of invoking
        /// the transform function on the Some value of <paramref name="source"/>.
        /// </returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> Select<TSource, TResult>(
            this Option<TSource> source,
            [InstantHandle] [NotNull] Func<TSource, TResult> selector)
        {
            Requires(selector != null);

            return source.Map(selector);
        }

        /// <summary>
        /// Projects the Some value of an optional value to an <see cref="Option{TSome}"/>
        /// and flattens the resulting optional-optional value into an optional value.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The Some type of the optional value returned by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="source">An optional value to project.</param>
        /// <param name="selector">A transform function to apply to the Some value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> whose Some value is the result of invoking the some-to-optional
        /// transform function on the Some value of the input optional value.
        /// </returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> SelectMany<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TResult>> selector)
        {
            Requires(selector != null);

            return source.Bind(selector);
        }

        /// <summary>
        /// Projects the Some value of an optional value to an <see cref="Option{TSome}"/>,
        /// flattens the resulting optional-optional value into an optional value,
        /// and invokes a result selector function on the Some value therein.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOption">
        /// The Some type of the intermediate optional value
        /// collected by <paramref name="optionalSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The Some type of the resulting optional value.</typeparam>
        /// <param name="source">An optional value to project.</param>
        /// <param name="optionalSelector">
        /// A transform function to apply to the Some value of the input optional value.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to the Some value of the intermediate optional value.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> whose Some value is the result of invoking the some-to-optional
        /// transform function <paramref name="optionalSelector"/> on the Some value of
        /// <paramref name="source"/> and them mapping that Some value and their corresponding optional
        /// value to a result optional value.
        /// </returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> SelectMany<TSource, TOption, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TOption>> optionalSelector,
            [NotNull, InstantHandle] Func<TSource, TOption, TResult> resultSelector)
        {
            Requires(optionalSelector != null);
            Requires(resultSelector != null);

            return source.Bind(sv => source.Bind(optionalSelector).Map(cv => resultSelector(sv, cv)));
        }

        #endregion
    }
}
