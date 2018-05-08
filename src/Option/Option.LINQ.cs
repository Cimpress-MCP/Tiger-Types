// <copyright file="Option.LINQ.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>LINQ support.</summary>
    public static partial class Option
    {
        /// <summary>Determines whether an optional value contains a value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Option{TSome}"/> to check for a None state.</param>
        /// <returns>
        /// <see langword="true"/> if the optional value contains a value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Any<TSource>(in this Option<TSource> source) => source.IsSome;

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return !source.IsSome || source.Filter(predicate).IsSome;
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
        public static bool Contains<TSource>(in this Option<TSource> source, [NotNull] TSource value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return source.Any(v => EqualityComparer<TSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether an optional value contains a specified value
        /// by using the provided <see cref="IEqualityComparer{T}"/>.
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
            in this Option<TSource> source,
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
        [Pure, EditorBrowsable(Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(in this Option<TSource> source)
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
        [Pure, EditorBrowsable(Never)]
        public static Option<TSource> DefaultIfEmpty<TSource>(
            in this Option<TSource> source,
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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Action<TSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TResult> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }

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
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TResult>> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }

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
        /// <paramref name="source"/> and then mapping that Some value and its corresponding optional
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
            if (optionalSelector is null) { throw new ArgumentNullException(nameof(optionalSelector)); }
            if (resultSelector is null) { throw new ArgumentNullException(nameof(resultSelector)); }

            return source.Bind(sv => source.Bind(optionalSelector).Map(cv => resultSelector(sv, cv)));
        }

        /// <summary>Applies an accumulator function over an optional value.</summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to aggregate over.</param>
        /// <param name="func">An accumulator function to invoke on the Some value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            in this Option<TSource> source,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func)
            where TAccumulate : struct
        {
            if (func is null) { throw new ArgumentNullException(nameof(func)); }

            return source.Fold(default, func);
        }

        /// <summary>
        /// Applies an accumulator function over an optional value.
        /// The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The Some type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on the Some value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            in this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func is null) { throw new ArgumentNullException(nameof(func)); }

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
        /// <param name="func">An accumulator value to invoke on the Some value.</param>
        /// <param name="resultSelector">
        /// A function to transform the final accumulator value into the result value.
        /// </param>
        /// <returns>The transformed final return accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            in this Option<TSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TSource, TAccumulate> func,
            [NotNull, InstantHandle] Func<TAccumulate, TResult> resultSelector)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func is null) { throw new ArgumentNullException(nameof(func)); }
            if (resultSelector is null) { throw new ArgumentNullException(nameof(resultSelector)); }

            var result = source.Fold(seed, func).Pipe(resultSelector);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }
    }
}
