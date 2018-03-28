// <copyright file="Either.cs" company="Cimpress, Inc.">
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
using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static Tiger.Types.EitherState;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Either{TLeft,TRight}"/>.</summary>
    [PublicAPI]
    public static partial class Either
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new Either<TLeft, TRight>(leftValue);
        }

        /// <summary>Creates an <see cref="EitherLeft{TLeft}"/> from the provided value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="EitherLeft{TLeft}"/> that can be converted to an <see cref="Either{TLeft,TRight}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static EitherLeft<TLeft> Left<TLeft>([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new EitherLeft<TLeft>(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new Either<TLeft, TRight>(rightValue);
        }

        /// <summary>Creates an <see cref="EitherRight{TRight}"/> from the provided value.</summary>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="EitherRight{TRight}"/> that can be converted to an <see cref="Either{TLeft,TRight}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static EitherRight<TRight> Right<TRight>([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new EitherRight<TRight>(rightValue);
        }

        #region Extensions

        /// <summary>
        /// Converts an <see cref="Either{TLeft,TRight}"/> into an <see cref="Option{TSome}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of <paramref name="eitherValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of <paramref name="eitherValue"/>.</typeparam>
        /// <param name="eitherValue">The value to be converted.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state with its Some value set to the
        /// Right value of <paramref name="eitherValue"/> if <paramref name="eitherValue"/> is in the
        /// Right state; otherwise, an <see cref="Option{TSome}"/> in the None state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Option<TRight> ToOption<TLeft, TRight>(in this Either<TLeft, TRight> eitherValue)
        {
            if (eitherValue.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return eitherValue.Map(v => new Option<TRight>(v)).GetValueOrDefault();
        }

        /// <summary>Joins one layer of <see cref="Either{TLeft,TRight}"/> from a value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of <paramref name="eitherEitherValue"/> and
        /// the Left type of the Left type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the Right type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <param name="eitherEitherValue">The value to be joined.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state whose Left value
        /// is the Left value of this instance if this instance is in the Left state,
        /// or the Right value of this instance if this instance is in the Right state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Either<TLeft, TRight> Join<TLeft, TRight>(
            in this Either<TLeft, Either<TLeft, TRight>> eitherEitherValue) =>
            eitherEitherValue.Match(
                left: l => new Either<TLeft, TRight>(l),
                right: r => r);

        /// <summary>Joins one layer of <see cref="Either{TLeft,TRight}"/> from a value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of the Left type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of <paramref name="eitherEitherValue"/> and
        /// the Right type of the Right type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <param name="eitherEitherValue">The value to be joined.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value
        /// is the Right value of this instance if this instance is in the Right state,
        /// or the Left value of this instance if this instance is in the Left state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Either<TLeft, TRight> Join<TLeft, TRight>(
            in this Either<Either<TLeft, TRight>, TRight> eitherEitherValue) =>
            eitherEitherValue.Match(
                left: l => l,
                right: r => new Either<TLeft, TRight>(r));

        /// <summary>
        /// Collapses an <see cref="Either{TLeft,TRight}"/> whose Left and Right types
        /// match into a value, based on its state.
        /// </summary>
        /// <typeparam name="TSame">The Left and Right type of <paramref name="eitherValue"/>.</typeparam>
        /// <param name="eitherValue">The value to be collapsed.</param>
        /// <returns>
        /// The Left value of this instance if this instance is in the Left state, or
        /// the Right value of this instance if this instance is in the Right state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, EditorBrowsable(Never)]
        public static TSame Collapse<TSame>(in this Either<TSame, TSame> eitherValue) =>
            eitherValue.Match(
                left: l => l,
                right: r => r);

        /// <summary>
        /// Asserts that the provided either value is in the Right state,
        /// throwing the provided exception if it is not.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of <paramref name="eitherValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of <paramref name="eitherValue"/>.</typeparam>
        /// <typeparam name="TException">The return type of <paramref name="left"/>.</typeparam>
        /// <param name="eitherValue">The value whose state to test.</param>
        /// <param name="left">
        /// A function from <typeparamref name="TLeft"/> producing an exception to be thrown
        /// if <paramref name="eitherValue"/> is in the Left state.
        /// </param>
        /// <returns>The Right value of <paramref name="eitherValue"/>.</returns>
        /// <exception cref="Exception"><paramref name="eitherValue"/> is in the Left state.</exception>
        [NotNull]
        public static TRight Assert<TLeft, TRight, TException>(
            in this Either<TLeft, TRight> eitherValue,
            [NotNull, InstantHandle] Func<TLeft, TException> left)
            where TException : Exception
        {
            if (eitherValue.IsLeft)
            {
                throw left((TLeft)eitherValue);
            }

            return eitherValue.Value;
        }

        #endregion

        #region Split

        /// <summary>Splits a value into an either value based on a provided condition.</summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Left state with its Left value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static Either<TValue, TValue> Split<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return splitter(value)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Left state with its Left value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static async Task<Either<TValue, TValue>> SplitAsync<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return await splitter(value).ConfigureAwait(false)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TValue"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static Either<TOut, TOut> Split<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter,
            [NotNull, InstantHandle] Func<TValue, TOut> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return Split(value, splitter).Map(
                left: mapper,
                right: mapper);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TValue"/>
        /// to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> SplitAsync<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter,
            [NotNull, InstantHandle] Func<TValue, Task<TOut>> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return Split(value, splitter).MapAsync(
                left: mapper,
                right: mapper);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TValue"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> SplitAsync<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter,
            [NotNull, InstantHandle] Func<TValue, TOut> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            // todo(cosborn) I'm starting to regret skipping bi-mappable T-versions.
            return SplitAsync(value, splitter).Map(ev => ev.Map(
                left: mapper,
                right: mapper));
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TValue"/>
        /// to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> SplitAsync<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter,
            [NotNull, InstantHandle] Func<TValue, Task<TOut>> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            // todo(cosborn) I'm starting to regret skipping bi-mappable T-versions.
            return SplitAsync(value, splitter).Bind(ev => ev.MapAsync(
                left: mapper,
                right: mapper));
        }

        #endregion
    }
}
