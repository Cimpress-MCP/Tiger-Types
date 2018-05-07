// <copyright file="Try.LINQ.cs" company="Cimpress, Inc.">
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
    public static partial class Try
    {
        /// <summary>Determines whether the try value contains an Ok value.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Try{TErr, TOk}"/> to check for an Ok state.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains an Ok value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool Any<TErrSource, TOkSource>(in this Try<TErrSource, TOkSource> source) => source.IsOk;

        /// <summary>
        /// Determins whether a try value contains an Ok value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Try{TErr, TOk}"/> whose Ok value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Ok value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Ok value if the source either value passes the test
        /// in the specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Any<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return source.IsOk && predicate(source.Value);
        }

        /// <summary>Determines whether a try value contains an Ok value that satisfies a condition.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Try{TErr, TOk}"/> whose Ok value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Ok value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Ok value of the source try value passes the test
        /// in the specified predicate or if the source either is not in the Ok state;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool All<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return !source.IsOk || source.Filter(ok: predicate).IsOk;
        }

        /// <summary>
        /// Determines whether a try value contains a specified Ok value
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">A try value in which to locate a value.</param>
        /// <param name="value">The value to locate in the try value.</param>
        /// <returns>
        /// <see langword="true"/> if the try value contains an Ok value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Contains<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull] TOkSource value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return source.Any(v => EqualityComparer<TOkSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether a try value contains a specified Ok value
        /// by using the provided equality comparer.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">A try value in which to locate a value.</param>
        /// <param name="value">The value to locate in the try value.</param>
        /// <param name="equalityComparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the try value contains an Ok value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static bool Contains<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull] TOkSource value,
            [CanBeNull] IEqualityComparer<TOkSource> equalityComparer)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return source.Any(v => (equalityComparer ?? EqualityComparer<TOkSource>.Default).Equals(v, value));
        }

        /// <summary>
        /// Returns the specified try value or the default value of <typeparamref name="TOkSource"/>
        /// as a try value if the try value is not in the Ok state.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The try value to return a default value for if it is not in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state with the default value for
        /// the <typeparamref name="TOkSource"/> type as the Ok value
        /// if <paramref name="source"/> is not in the Right state; otherwise, <paramref name="source"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public static Try<TErrSource, TOkSource> DefaultIfEmpty<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source)
            where TOkSource : struct => source.Recover(default(TOkSource));

        /// <summary>
        /// Returns the specified try value or the specified value as a try value
        /// if the try value is not in the Ok state.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The try value to return a default value for if it is not in the Ok state.
        /// </param>
        /// <param name="defaultValue">The value to return if the try value is not in the Ok state.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state with <paramref name="source"/>
        /// as the Ok value if <paramref name="source"/> is not in the Right state;
        /// otherwise, <paramref name="source"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public static Try<TErrSource, TOkSource> DefaultIfEmpty<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull] TOkSource defaultValue)
        {
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            return source.Recover(defaultValue);
        }

        /// <summary>Invokes an action on the Ok value of a try value.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">A try value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>The original value, exhibiting the specified side effects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [LinqTunnel, EditorBrowsable(Never)]
        public static Try<TErrSource, TOkSource> Do<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Action<TOkSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Tap(ok: onNext);
        }

        /// <summary>Invokes an action on the Ok value of a try value.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">A try value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(Never)]
        public static Unit ForEach<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Action<TOkSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Let(onNext);
        }

        /// <summary>Filters the Ok value of a try value based on a predicate.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="Option{TSome}"/> to filter.</param>
        /// <param name="predicate">A function to test the Ok value for a condition.</param>
        /// <returns>
        /// An <see cref="Try{TErr, TOk}"/> that contains the value from the input
        /// option value that satifies the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Try<TErrSource, TOkSource> Where<TErrSource, TOkSource>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return source.Filter(predicate);
        }

        /// <summary>Projects the Ok value of a try value into a new form.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A try value to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to the Ok value.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> whose Right value is the result of invoking
        /// the transform function on the Ok value of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Try<TErrSource, TResult> Select<TErrSource, TOkSource, TResult>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, TResult> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }

            return source.Map(selector);
        }

        /// <summary>
        /// Projects the Ok value of a try value to an <see cref="Try{TErr, TOk}"/>
        /// and flattens the resulting try-try value into a try value.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The Ok type of the try value returned by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="source">A try value to project.</param>
        /// <param name="selector">A transform function to apply to the Ok value.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> whose Ok value is the result of invoking the
        /// ok-to-try transform function on the Ok value of the input try value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Try<TErrSource, TResult> SelectMany<TErrSource, TOkSource, TResult>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, Try<TErrSource, TResult>> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }

            return source.Bind(selector);
        }

        /// <summary>
        /// Projects the Ok value of a try value to a <see cref="Try{TErr, TOk}"/>,
        /// flattens the resulting try-try value into a try value,
        /// and invokes a result selector function on the Ok value therein.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TTry">
        /// The Ok type of the intermediate try value
        /// collected by <paramref name="trySelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The Ok type of the resulting try value.</typeparam>
        /// <param name="source">A try value to project.</param>
        /// <param name="trySelector">
        /// A transform function to apply to the Ok value of the input try value.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to the Ok value of the intermediate try value.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> whose Ok value is the result of invoking
        /// the ok-to-try transform function <paramref name="trySelector"/> on the Ok value
        /// of <paramref name="source"/> and then mapping that Ok value and its corresponding
        /// try value to a result try value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="trySelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Try<TErrSource, TResult> SelectMany<TErrSource, TOkSource, TTry, TResult>(
            this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TOkSource, Try<TErrSource, TTry>> trySelector,
            [NotNull, InstantHandle] Func<TOkSource, TTry, TResult> resultSelector)
        {
            if (trySelector is null) { throw new ArgumentNullException(nameof(trySelector)); }
            if (resultSelector is null) { throw new ArgumentNullException(nameof(resultSelector)); }

            return source.Bind(sv => source.Bind(trySelector).Map(cv => resultSelector(sv, cv)));
        }

        /// <summary>Applies an accumulator function over a try value.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">A <see cref="Try{TErr, TOk}"/> to aggregate over.</param>
        /// <param name="func">An accumulator function to invoke on the Ok value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TErrSource, TOkSource, TAccumulate>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull, InstantHandle] Func<TAccumulate, TOkSource, TAccumulate> func)
            where TAccumulate : struct
        {
            if (func is null) { throw new ArgumentNullException(nameof(func)); }

            return source.Fold(default, func);
        }

        /// <summary>Applies an accumulator function over a try value.</summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">A <see cref="Try{TErr, TOk}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on the Ok value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TErrSource, TOkSource, TAccumulate>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TOkSource, TAccumulate> func)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func is null) { throw new ArgumentNullException(nameof(func)); }

            return source.Fold(seed, func);
        }

        /// <summary>
        /// Applies an accumulator function over a try value.
        /// The specified seed value is used as the initial accumulator value,
        /// and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TErrSource">The Err type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TOkSource">The Ok type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">A <see cref="Try{TErr, TOk}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on the Ok value.</param>
        /// <param name="resultSelector">
        /// A function to transform the final accumulator value into the result value.
        /// </param>
        /// <returns>The transformed final return accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TResult Aggregate<TErrSource, TOkSource, TAccumulate, TResult>(
            in this Try<TErrSource, TOkSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TOkSource, TAccumulate> func,
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
