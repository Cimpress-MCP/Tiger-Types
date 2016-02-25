using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="Option{TSome}"/>.</summary>
    [SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "R# doesn't understand Code Contracts.")]
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
                : value.SomeValue;

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
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Any<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires<ArgumentNullException>(predicate != null);

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
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null" />.</exception>
        [Pure]
        public static bool All<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires<ArgumentNullException>(predicate != null);

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
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Contains<TSource>(this Option<TSource> source, [NotNull] TSource value)
        {
            Requires<ArgumentNullException>(value != null);

            return source.Any(v => EqualityComparer<TSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether an optional value contains a specified value
        /// by using a specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value in which to locate a value.</param>
        /// <param name="value">The value to locate in the optional value.</param>
        /// <param name="equalityComparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the optional value contains a Some value that has the specified value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Contains<TSource>(
            this Option<TSource> source,
            [NotNull] TSource value,
            [CanBeNull] IEqualityComparer<TSource> equalityComparer)
        {
            Requires<ArgumentNullException>(value != null);

            return source.Any(v => (equalityComparer ?? EqualityComparer<TSource>.Default).Equals(v, value));
        }

        /// <summary>
        /// Returns the specified optional value or the type parameter's default value as an optional value
        /// if the optional value is in the None state.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The optional value to return a default value for if it is in the None state.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state with the default value for
        /// the <typeparamref name="TSource"/> type as the Some value
        /// if <paramref name="source"/> is in the None state; otherwise; <paramref name="source"/>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(this Option<TSource> source)
            where TSource : struct
        {
            Ensures(Result<Option<TSource>>().IsSome);

            return source.Recover(default(TSource));
        }

        /// <summary>
        /// Returns the specified optional value or the specified value as an optional value
        /// if the optional value is in the None state.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The optional value to return a default value for if it is in the None state.</param>
        /// <param name="defaultValue">The value to return if the optional value is in the None state.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state <paramref name="defaultValue"/> as the Some value
        /// if <paramref name="source"/> is in the None state; otherwise; <paramref name="source"/>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(
            this Option<TSource> source,
            [NotNull] TSource defaultValue)
        {
            Requires<ArgumentNullException>(defaultValue != null);
            Ensures(Result<Option<TSource>>().IsSome);

            return source.Recover(defaultValue);
        }

        /// <summary>Invokes an action on the Some value of an optional value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>The original value, exhibiting the specified side effects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TSource> Do<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            Requires<ArgumentNullException>(onNext != null);

            return source.Tap(onNext);
        }

        /// <summary>Invokes an action on the Some value of an optional value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ForEach<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            Requires<ArgumentNullException>(onNext != null);

            source.Let(onNext);
        }

        /// <summary>Filters the Some value of an optional value based on a predicate.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to filter.</param>
        /// <param name="predicate">A function to test the Some value for a condition.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> that contains the value from the input
        /// option value that satifies the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TSource> Where<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            Requires<ArgumentNullException>(predicate != null);

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
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> Select<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TResult> selector)
        {
            Requires<ArgumentNullException>(selector != null);

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
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> SelectMany<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TResult>> selector)
        {
            Requires<ArgumentNullException>(selector != null);
            
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
        /// <exception cref="ArgumentNullException"><paramref name="optionalSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public static Option<TResult> SelectMany<TSource, TOption, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TOption>> optionalSelector,
            [NotNull, InstantHandle] Func<TSource, TOption, TResult> resultSelector)
        {
            Requires<ArgumentNullException>(optionalSelector != null);
            Requires<ArgumentNullException>(resultSelector != null);

            return source.Bind(sv => source.Bind(optionalSelector).Map(cv => resultSelector(sv, cv)));
        }

        /// <summary>
        /// Applies an accumulator function over an optional value.
        /// The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator value to be invoked on the Some value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(EditorBrowsableState.Never)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func)
        {
            Requires<ArgumentNullException>(seed != null);
            Requires<ArgumentNullException>(func != null);
            Ensures(Result<TAccumulate>() != null);

            return source.Fold(seed, func);
        }

        /// <summary>
        /// Applies an accumulator function over an optional value.
        /// The specified seed value is used as the initial accumulator value,
        /// and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator value to be invoked on the Some value.</param>
        /// <param name="resultSelector">
        /// A function to transform the final accumulator value into the result value.
        /// </param>
        /// <returns>The transformed final return accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(EditorBrowsableState.Never)]
        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func,
            [NotNull, InstantHandle] Func<TAccumulate, TResult> resultSelector)
        {
            Requires<ArgumentNullException>(seed != null);
            Requires<ArgumentNullException>(func != null);
            Requires<ArgumentNullException>(resultSelector != null);
            Ensures(Result<TResult>() != null);

            var result = source.Fold(seed, func).Pipe(resultSelector);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        #endregion
    }
}
