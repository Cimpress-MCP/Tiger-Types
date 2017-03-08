﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Option{TSome}"/>.</summary>
    [PublicAPI]
    public static class Option
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
        /// 
        /// </summary>
        /// <typeparam name="TSome"></typeparam>
        /// <param name="optionOptionValue"></param>
        /// <returns></returns>
        public static Option<TSome> Join<TSome>(this Option<Option<TSome>> optionOptionValue) =>
            optionOptionValue.GetValueOrDefault();

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
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

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
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

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
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

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
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return source.Any(v => (equalityComparer ?? EqualityComparer<TSource>.Default).Equals(v, value));
        }

        /// <summary>
        /// Returns the specified optional value or the default value of <typeparamref name="TSource"/>
        /// as an optional value if the optional value is in the None state.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The optional value to return a default value for if it is in the None state.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state with the default value for
        /// the <typeparamref name="TSource"/> type as the Some value
        /// if <paramref name="source"/> is in the None state; otherwise; <paramref name="source"/>.
        /// </returns>
        [EditorBrowsable(Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(this Option<TSource> source)
            where TSource : struct => source.Recover(default(TSource));

        /// <summary>
        /// Returns the specified optional value or the specified value as an optional value
        /// if the optional value is in the None state.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The optional value to return a default value for if it is in the None state.</param>
        /// <param name="defaultValue">The value to return if the optional value is in the None state.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state with <paramref name="defaultValue"/>
        /// as the Some value if <paramref name="source"/> is in the None state;
        /// otherwise, <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultValue"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(
            this Option<TSource> source,
            [NotNull] TSource defaultValue)
        {
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            return source.Recover(defaultValue);
        }

        /// <summary>Invokes an action on the Some value of an optional value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>The original value, exhibiting the specified side effects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [LinqTunnel, EditorBrowsable(Never)]
        public static Option<TSource> Do<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            if (onNext == null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Tap(onNext);
        }

        /// <summary>Invokes an action on the Some value of an optional value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An optional value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(Never)]
        public static Unit ForEach<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            if (onNext == null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Let(onNext);
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
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Option<TSource> Where<TSource>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

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
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Option<TResult> Select<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TResult> selector)
        {
            if (selector == null) { throw new ArgumentNullException(nameof(selector)); }

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
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Option<TResult> SelectMany<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TResult>> selector)
        {
            if (selector == null) { throw new ArgumentNullException(nameof(selector)); }

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
        /// <paramref name="source"/> and then mapping that Some value and their corresponding optional
        /// value to a result optional value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionalSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Option<TResult> SelectMany<TSource, TOption, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TOption>> optionalSelector,
            [NotNull, InstantHandle] Func<TSource, TOption, TResult> resultSelector)
        {
            if (optionalSelector == null) { throw new ArgumentNullException(nameof(optionalSelector)); }
            if (resultSelector == null) { throw new ArgumentNullException(nameof(resultSelector)); }

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
        /// <param name="func">An accumulator function to be invoked on the Some value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }

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
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func,
            [NotNull, InstantHandle] Func<TAccumulate, TResult> resultSelector)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }
            if (resultSelector == null) { throw new ArgumentNullException(nameof(resultSelector)); }

            var result = source.Fold(seed, func).Pipe(resultSelector);
            Assume(result != null, Resources.ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

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
            IComparer<TSome> comparer)
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
