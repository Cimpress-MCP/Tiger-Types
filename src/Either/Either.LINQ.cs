// <copyright file="Either.LINQ.cs" company="Cimpress, Inc.">
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
using static Tiger.Types.EitherState;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>LINQ support.</summary>
    public static partial class Either
    {
        /// <summary>Determines whether the either value contains a Right value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Either{TLeft,TRight}"/> to check for a Right state.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a Right value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Any<TLeftSource, TRightSource>(in this Either<TLeftSource, TRightSource> source)
        {
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.IsRight;
        }

        /// <summary>
        /// Determines whether an either value contains a Right value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Either{TLeft,TRight}"/> whose Right value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Right value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Right value if the source either value passes the test
        /// in the specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Any<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.IsRight && predicate(source.Value);
        }

        /// <summary>
        /// Determines whether an either value contains a Right value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Either{TLeft,TRight}"/> whose Right value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Right value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Right value of the source either value passes the test
        /// in the specified predicate or if the source either is in the Left state;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool All<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return !source.IsRight || source.IsRight && predicate(source.Value);
        }

        /// <summary>
        /// Determines whether an either value contains a specified Right value
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value in which to locate a value.</param>
        /// <param name="value">The value to locate in the either value.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a Right value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Contains<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Any(v => EqualityComparer<TRightSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether an either value contains a specified Right value
        /// by using the provided <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value in which to locate a value.</param>
        /// <param name="value">The value to locate in the either value.</param>
        /// <param name="equalityComparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a Right value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Contains<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource value,
            [CanBeNull] IEqualityComparer<TRightSource> equalityComparer)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Any(v => (equalityComparer ?? EqualityComparer<TRightSource>.Default).Equals(v, value));
        }

        /// <summary>
        /// Returns the specified either value or the default value of <typeparamref name="TRightSource"/>
        /// as an either value if the either value is not in the Right state.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The either value to return a default value for if it is not in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with the default value for
        /// the <typeparamref name="TRightSource"/> type as the Right value
        /// if <paramref name="source"/> is not in the Right state; otherwise, <paramref name="source"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> DefaultIfEmpty<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source)
            where TRightSource : struct => source.Recover(default(TRightSource));

        /// <summary>
        /// Returns the specified either value or the specified value as an either value
        /// if the either value is not in the Right state.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The either value to return a default value for if it is not in the Right state.
        /// </param>
        /// <param name="defaultValue">The value to return if the either value is not in the Right state.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with <paramref name="defaultValue"/>
        /// as the Right value if <paramref name="source"/> is not in the Right state;
        /// otherwise, <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultValue"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> DefaultIfEmpty<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource defaultValue)
        {
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            return source.Recover(defaultValue);
        }

        /// <summary>Invokes an action on the Right value of an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>The original value, exhibiting the specified side effects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> Do<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Action<TRightSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Tap(right: onNext);
        }

        /// <summary>Invokes an action on the Right value of an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(Never)]
        public static Unit ForEach<TLeftSource, TRightSource>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Action<TRightSource> onNext)
        {
            if (onNext is null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Let(onNext);
        }

        /// <summary>Projects the Right value of an either value into a new form.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An either value to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to the Right value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking
        /// the transform function on the Right value of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> Select<TLeftSource, TRightSource, TResult>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, TResult> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Map(selector);
        }

        /// <summary>
        /// Projects the Right value of an either value to an <see cref="Either{TLeft,TRight}"/>
        /// and flattens the resulting either-either value into an either value.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The Right type of the either value returned by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="source">An either value to project.</param>
        /// <param name="selector">A transform function to apply to the Right value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking the
        /// right-to-either transform function on the Right value of the input either value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> SelectMany<TLeftSource, TRightSource, TResult>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, Either<TLeftSource, TResult>> selector)
        {
            if (selector is null) { throw new ArgumentNullException(nameof(selector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Bind(selector);
        }

        /// <summary>
        /// Projects the Right value of an either value to an <see cref="Either{TLeft,TRight}"/>,
        /// flattens the resulting either-either value into an either value,
        /// and invokes a result selector function on the Right value therein.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TEither">
        /// The Right type of the intermediate either value
        /// collected by <paramref name="eitherSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The Right type of the resulting either value.</typeparam>
        /// <param name="source">An either value to project.</param>
        /// <param name="eitherSelector">
        /// A transform function to apply to the Right value of the input either value.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to the Right value of the intermediate either value.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking
        /// the right-to-either transform function <paramref name="eitherSelector"/> on the Right value
        /// of <paramref name="source"/> and then mapping that Right value and its corresponding
        /// either value to a result either value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> SelectMany<TLeftSource, TRightSource, TEither, TResult>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, Either<TLeftSource, TEither>> eitherSelector,
            [NotNull, InstantHandle] Func<TRightSource, TEither, TResult> resultSelector)
        {
            if (eitherSelector is null) { throw new ArgumentNullException(nameof(eitherSelector)); }
            if (resultSelector is null) { throw new ArgumentNullException(nameof(resultSelector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Bind(sv => source.Bind(eitherSelector).Map(cv => resultSelector(sv, cv)));
        }

        /// <summary>Applies an accumulator function over an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="Either{TLeft,TRight}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on the Right value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TLeftSource, TRightSource, TAccumulate>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TRightSource, TAccumulate> func)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func is null) { throw new ArgumentNullException(nameof(func)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Fold(seed, func);
        }

        /// <summary>
        /// Applies an accumulator function over an either value.
        /// The specified seed value is used as the initial accumulator value,
        /// and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An <see cref="Either{TLeft,TRight}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on the Right value.</param>
        /// <param name="resultSelector">
        /// A function to transform the final accumulator value into the result value.
        /// </param>
        /// <returns>The transformed final return accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TResult Aggregate<TLeftSource, TRightSource, TAccumulate, TResult>(
            in this Either<TLeftSource, TRightSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TRightSource, TAccumulate> func,
            [NotNull, InstantHandle] Func<TAccumulate, TResult> resultSelector)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func is null) { throw new ArgumentNullException(nameof(func)); }
            if (resultSelector is null) { throw new ArgumentNullException(nameof(resultSelector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = source.Fold(seed, func).Pipe(resultSelector);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }
    }
}
