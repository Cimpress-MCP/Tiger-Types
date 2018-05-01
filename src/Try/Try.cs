// <copyright file="Try.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Try{TErr, TOk}"/>.</summary>
    [PublicAPI]
    public static partial class Try
    {
        /// <summary>
        /// A value that can be converted to a <see cref="Try{TErr, TOk}"/>
        /// of any Err and Ok type.
        /// </summary>
        public static readonly TryNone None;

        /// <summary>Creates a <see cref="Try{TErr, TOk}"/> from the provided value.</summary>
        /// <typeparam name="TErr">The Err type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <typeparam name="TOk">The Ok type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <param name="value">The value from which to create the <see cref="Try{TErr, TOk}"/>.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if <paramref name="value"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the value of <paramref name="value"/>.
        /// </returns>
        public static Try<TErr, TOk> From<TErr, TOk>([CanBeNull] TOk value) => value == null
            ? Try<TErr, TOk>.None
            : Try<TErr, TOk>.From(value);

        /// <summary>Creates a <see cref="Try{TErr, TOk}"/> from the provided value.</summary>
        /// <typeparam name="TErr">The Err type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <typeparam name="TOk">The Ok type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <param name="value">The value from which to create the <see cref="Try{TErr, TOk}"/>.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if <paramref name="value"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Err state whose Err value is the value of <paramref name="value"/>.
        /// </returns>
        public static Try<TErr, TOk> From<TErr, TOk>([CanBeNull] TErr value) => value == null
            ? Try<TErr, TOk>.None
            : Try<TErr, TOk>.From(value);

        /// <summary>Creates a <see cref="Try{TErr, TOk}"/> from the provided value.</summary>
        /// <typeparam name="TErr">The Err type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <typeparam name="TOk">The Ok type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <param name="value">The value from which to create the <see cref="Try{TErr, TOk}"/>.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state is <paramref name="value"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the value of <paramref name="value"/>.
        /// </returns>
        public static Try<TErr, TOk> From<TErr, TOk>([CanBeNull] TOk? value)
            where TOk : struct => value == null
            ? Try<TErr, TOk>.None
            : Try<TErr, TOk>.From(value.Value);

        /// <summary>Creates a <see cref="Try{TErr, TOk}"/> from the provided value.</summary>
        /// <typeparam name="TErr">The Err type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <typeparam name="TOk">The Ok type of the <see cref="Try{TErr, TOk}"/> to create.</typeparam>
        /// <param name="value">The value from which to create the <see cref="Try{TErr, TOk}"/>.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if <paramref name="value"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Err state whose Err value is the value of <paramref name="value"/>.
        /// </returns>
        public static Try<TErr, TOk> From<TErr, TOk>([CanBeNull] TErr? value)
            where TErr : struct => value == null
            ? Try<TErr, TOk>.None
            : Try<TErr, TOk>.From(value.Value);

        #region Extensions

        /// <summary>Converts a <see cref="Try{TErr, TOk}"/> into an <see cref="Option{TSome}"/>.</summary>
        /// <typeparam name="TErr">The Err type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <param name="tryValue">The value to convert.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="tryValue"/> is in the None state
        /// or if <paramref name="tryValue"/> is in the Err state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state whose Some value
        /// is the Ok value of <paramref name="tryValue"/>.
        /// </returns>
        public static Option<TOk> ToOption<TErr, TOk>(in this Try<TErr, TOk> tryValue) => tryValue.Match(
            none: Option<TOk>.None,
            err: _ => Option<TOk>.None,
            ok: Option.From);

        /// <summary>Converts a <see cref="Try{TErr, TOk}"/> into an <see cref="Either{TLeft, TRight}"/>.</summary>
        /// <typeparam name="TErr">The Err type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <param name="tryValue">The value to convert.</param>
        /// <param name="fallback">A function producing the value to use as fallback.</param>
        /// <returns>
        /// An <see cref="Either{TLeft, TRight}"/> in the Left state if <paramref name="tryValue"/>
        /// is in the Err state,
        /// an <see cref="Either{TLeft, TRight}"/> in the Right state whose Right value
        /// is the Ok value of <paramref name="tryValue"/> if <paramref name="tryValue"/> is in the Ok state,
        /// or an <see cref="Either{TLeft, TRight}"/> in the Right state whose Right value
        /// is the result of invoking <paramref name="fallback"/> if <paramref name="tryValue"/> is in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
        public static Either<TErr, TOk> ToEither<TErr, TOk>(
            in this Try<TErr, TOk> tryValue,
            [NotNull, InstantHandle] Func<TOk> fallback)
        {
            if (fallback is null) { throw new ArgumentNullException(nameof(fallback)); }

            return tryValue.Match(
                none: () => fallback(),
                err: e => new Either<TErr, TOk>(e),
                ok: v => new Either<TErr, TOk>(v));
        }

        /// <summary>Converts a <see cref="Try{TErr, TOk}"/> into an <see cref="Either{TLeft, TRight}"/>.</summary>
        /// <typeparam name="TErr">The Err type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <param name="tryValue">The value to convert.</param>
        /// <param name="fallback">A function producing the value to use as fallback.</param>
        /// <returns>
        /// An <see cref="Either{TLeft, TRight}"/> in the Left state if <paramref name="tryValue"/>
        /// is in the Err state,
        /// an <see cref="Either{TLeft, TRight}"/> in the Right state whose Right value
        /// is the Ok value of <paramref name="tryValue"/> if <paramref name="tryValue"/> is in the Ok state,
        /// or an <see cref="Either{TLeft, TRight}"/> in the Left state whose Left value
        /// is the result of invoking <paramref name="fallback"/> if <paramref name="tryValue"/> is in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
        public static Either<TErr, TOk> ToEither<TErr, TOk>(
            in this Try<TErr, TOk> tryValue,
            [NotNull, InstantHandle] Func<TErr> fallback)
        {
            if (fallback is null) { throw new ArgumentNullException(nameof(fallback)); }

            return tryValue.Match(
                none: () => fallback(),
                err: e => new Either<TErr, TOk>(e),
                ok: v => new Either<TErr, TOk>(v));
        }

        /// <summary>Joins one layer of <see cref="Try{TErr, TOk}"/> from a value.</summary>
        /// <typeparam name="TErr">
        /// The Err type of <paramref name="tryTryValue"/> and
        /// the Err type of the Err type of <paramref name="tryTryValue"/>.
        /// </typeparam>
        /// <typeparam name="TOk">The Ok type of the Ok type of <paramref name="tryTryValue"/>.</typeparam>
        /// <param name="tryTryValue">The value to join.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state whose Err value
        /// is the Err value of this instance if this instance is in the Err state,
        /// or the Ok value of this instance if this instance is in the OK state.
        /// </returns>
        public static Try<TErr, TOk> Join<TErr, TOk>(
            in this Try<TErr, Try<TErr, TOk>> tryTryValue) =>
            tryTryValue.Match(
                none: Try<TErr, TOk>.None,
                err: Try<TErr, TOk>.From,
                ok: tv => new Try<TErr, TOk>(tv));

        /// <summary>Joins one layer of <see cref="Try{TErr, TOk}"/> from a value.</summary>
        /// <typeparam name="TErr">The Err type of the Err type of <paramref name="tryTryValue"/>.</typeparam>
        /// <typeparam name="TOk">
        /// The Ok type of <paramref name="tryTryValue"/> and
        /// the Ok type of the Ok type of <paramref name="tryTryValue"/>.
        /// </typeparam>
        /// <param name="tryTryValue">The value to join.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value
        /// is the Ok value of this instance if this instance is in the Ok state,
        /// or the Err value of this instance if this instance is in the Err state.
        /// </returns>
        public static Try<TErr, TOk> Join<TErr, TOk>(
            in this Try<Try<TErr, TOk>, TOk> tryTryValue) =>
            tryTryValue.Match(
                none: Try<TErr, TOk>.None,
                err: tv => new Try<TErr, TOk>(tv),
                ok: Try<TErr, TOk>.From);

        /// <summary>
        /// Collapses a <see cref="Try{TErr, TOk}"/> whose Err and Ok types match
        /// into a value, based in its state.
        /// </summary>
        /// <typeparam name="TSame">The Err and Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <param name="tryValue">The value to collapse.</param>
        /// <param name="none">The value to return if <paramref name="tryValue"/> is in the None state.</param>
        /// <returns>
        /// <paramref name="none"/> if <paramref name="tryValue"/> is in the None state,
        /// the Err value of <paramref name="tryValue"/>
        /// if <paramref name="tryValue"/> is in the Err state,
        /// or the Ok value of <paramref name="tryValue"/>
        /// if <paramref name="tryValue"/> is in the Ok state.
        /// </returns>
        [NotNull, EditorBrowsable(Never)]
        public static TSame Collapse<TSame>(
            in this Try<TSame, TSame> tryValue,
            [NotNull] TSame none) => tryValue.Match(
                none: none,
                err: ev => ev,
                ok: ov => ov);

        /// <summary>
        /// Asserts that the provided try value is in the Ok state,
        /// throwing one of the provided exceptions if not.
        /// </summary>
        /// <typeparam name="TErr">The Err type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TNoneException">
        /// The type of the exception to throw if <paramref name="tryValue"/> is in the None state.
        /// </typeparam>
        /// <typeparam name="TErrException">
        /// The type of the exception to throw if <paramref name="tryValue"/> is in the Err state.
        /// </typeparam>
        /// <param name="tryValue">The value whose state to test.</param>
        /// <param name="none">
        /// A function producing an exception to throw if <paramref name="tryValue"/> is in the None state.
        /// </param>
        /// <param name="err">
        /// A function from <typeparamref name="TErr"/> producing an exception to throw
        /// if <paramref name="tryValue"/> is in the Err state.
        /// </param>
        /// <returns>The Ok value of <paramref name="tryValue"/>.</returns>
        /// <exception cref="Exception"><paramref name="tryValue"/> is in the None state.</exception>
        /// <exception cref="Exception"><paramref name="tryValue"/> is in the Err state.</exception>
        public static TOk Assert<TErr, TOk, TNoneException, TErrException>(
            in this Try<TErr, TOk> tryValue,
            [NotNull, InstantHandle] Func<TNoneException> none,
            [NotNull, InstantHandle] Func<TErr, TErrException> err)
            where TNoneException : Exception
            where TErrException : Exception
        {
            if (tryValue.IsNone)
            {
                throw none();
            }

            if (tryValue.IsErr)
            {
                throw err((TErr)tryValue);
            }

            return tryValue.Value;
        }

        /// <summary>
        /// Asserts that the provided try value is in the Ok state,
        /// throwing one of the provided exceptions if not.
        /// </summary>
        /// <typeparam name="TErr">The Err type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of <paramref name="tryValue"/>.</typeparam>
        /// <typeparam name="TNoneException">
        /// The type of the exception to throw if <paramref name="tryValue"/> is in the None state.
        /// </typeparam>
        /// <param name="tryValue">The value whose state to test.</param>
        /// <param name="none">
        /// A function producing an exception to throw if <paramref name="tryValue"/> is in the None state.
        /// </param>
        /// <returns>The Ok value of <paramref name="tryValue"/>.</returns>
        /// <exception cref="Exception"><paramref name="tryValue"/> is in the None state.</exception>
        /// <exception cref="Exception"><paramref name="tryValue"/> is in the Err state.</exception>
        public static TOk Assert<TErr, TOk, TNoneException>(
            in this Try<TErr, TOk> tryValue,
            [NotNull, InstantHandle] Func<TNoneException> none)
            where TErr : Exception
            where TNoneException : Exception
        {
            if (tryValue.IsNone)
            {
                throw none();
            }

            if (tryValue.IsErr)
            {
                throw (TErr)tryValue;
            }

            return tryValue.Value;
        }

        #endregion
    }
}
