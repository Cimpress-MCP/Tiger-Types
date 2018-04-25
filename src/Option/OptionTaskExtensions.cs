// <copyright file="OptionTaskExtensions.cs" company="Cimpress, Inc.">
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
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of <see cref="Task{TResult}"/>,
    /// specialized for <see cref="Option{TSome}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class OptionTaskExtensions
    {
        #region MatchT

        /// <summary>Transforms the result of <paramref name="optionTaskValue"/> based on its state.</summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Map(v => v.Match(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to
        /// <typeparamref name="TOut"/> to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.MatchAsync(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Map(v => v.Match(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/> to perform
        /// in the None case.
        /// </param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to
        /// <typeparamref name="TOut"/> to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.MatchAsync(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.MatchAsync(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.MatchAsync(none: none, some: some));
        }

        #endregion

        #region MapT

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="some"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> MapT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Map(ov => ov.Map(some));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Result type of the return type of <paramref name="some"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> MapTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(ov => ov.MapAsync(some));
        }

        #endregion

        #region BindT

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Some type of the return type of <paramref name="some"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <see cref="Option{TSome}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> BindT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Option<TOut>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Map(ov => ov.Bind(some));
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Some type of the Result type of the return type of <paramref name="some"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <see cref="Option{TSome}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> BindTAsync<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<Option<TOut>>> some)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(ov => ov.BindAsync(some));
        }

        #endregion

        #region TapT

        /// <summary>
        /// Acts upon the Some value of the Result value of <paramref name="optionTaskValue"/>, if it is present.
        /// </summary>
        /// <typeparam name="T">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="tapper">
        /// An action to perform on the Some value of the Result value of <paramref name="optionTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="optionTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Option<T>> TapT<T>(
            [NotNull] this Task<Option<T>> optionTaskValue,
            [NotNull, InstantHandle] Action<T> tapper)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (tapper is null) { throw new ArgumentNullException(nameof(tapper)); }

            return optionTaskValue.Map(ov => ov.Tap(tapper));
        }

        /// <summary>
        /// Acts upon the Some value of the Result value of <paramref name="optionTaskValue"/>,
        /// if it is present, asynchronously.
        /// </summary>
        /// <typeparam name="T">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="tapper">
        /// An asynchronous action to perform on the Some value of the Result value
        /// of <paramref name="optionTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="optionTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Option<T>> TapTAsync<T>(
            [NotNull] this Task<Option<T>> optionTaskValue,
            [NotNull, InstantHandle] Func<T, Task> tapper)
        {
            if (optionTaskValue is null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (tapper is null) { throw new ArgumentNullException(nameof(tapper)); }

            return optionTaskValue.Bind(ov => ov.TapAsync(tapper));
        }

        #endregion
    }
}
