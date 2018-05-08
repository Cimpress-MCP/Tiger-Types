// <copyright file="TryTaskExtensions.cs" company="Cimpress, Inc.">
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
    /// specialized for <see cref="Try{TErr, TOk}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class TryTaskExtensions
    {
        #region MatchT

        /// <summary>Transforms the result of <paramref name="tryTaskValue"/> based on its state.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(tv => tv.Match(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">A value to return in the None case.</param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>Transforms the result of <paramref name="tryTaskValue"/> based on its state.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(tv => tv.Match(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>Transforms the result of <paramref name="tryTaskValue"/> based on its state.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Transforms the result of <paramref name="tryTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>
        /// to perform in the Err case.
        /// </param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>
        /// to perform in the Ok case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/>, <paramref name="err"/>, or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public static Task<TOut> MatchTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err == null) { throw new ArgumentNullException(nameof(err)); }
            if (ok == null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MatchAsync(none: none, err: err, ok: ok));
        }

        #endregion

        #region MapT

        /// <summary>Maps the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="ok"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> with the transformation applied if the result of
        /// <paramref name="tryTaskValue"/> was in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TErr, TOut>> MapT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(tv => tv.Map(ok: ok));
        }

        /// <summary>Maps the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="err"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> with the transformation applied if the result of
        /// <paramref name="tryTaskValue"/> was in the Err state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TOut, TOk>> MapT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TErr, TOut> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Map(tv => tv.Map(err: err));
        }

        /// <summary>Maps the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="ok"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> with the transformation applied if the result of
        /// <paramref name="tryTaskValue"/> was in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TErr, TOut>> MapTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.MapAsync(ok: ok));
        }

        /// <summary>Maps the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="err"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> with the transformation applied if the result of
        /// <paramref name="tryTaskValue"/> was in the Err state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TOut, TOk>> MapTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Bind(tv => tv.MapAsync(err: err));
        }

        #endregion

        #region BindT

        /// <summary>Binds the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Err type of the resturn type of <paramref name="ok"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="ok">
        /// A transformation from <typeparamref name="TOk"/> to <see cref="Try{TErr, TOk}"/>.
        /// </param>
        /// <returns>
        /// A task which, when resolved, produces a <see cref="Try{TErr, TOk}"/> with the transformation applied
        /// if the result of <paramref name="tryTaskValue"/> was in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TErr, TOut>> BindT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOk, Try<TErr, TOut>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(tv => tv.Bind(ok: ok));
        }

        /// <summary>Binds the result of a <see cref="Task{TResult}"/> over a transformation.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Err type of the resturn type of <paramref name="err"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="err">
        /// A transformation from <typeparamref name="TErr"/> to <see cref="Try{TErr, TOk}"/>.
        /// </param>
        /// <returns>
        /// A task which, when resolved, produces a <see cref="Try{TErr, TOk}"/> with the transformation applied
        /// if the result of <paramref name="tryTaskValue"/> was in the Err state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TOut, TOk>> BindT<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TErr, Try<TOut, TOk>> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Map(tv => tv.Bind(err: err));
        }

        /// <summary>Binds the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Err type of the resturn type of <paramref name="ok"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="ok">
        /// An asynchronous transformation from <typeparamref name="TOk"/> to <see cref="Try{TErr, TOk}"/>.
        /// </param>
        /// <returns>
        /// A task which, when resolved, produces a <see cref="Try{TErr, TOk}"/> with the transformation applied
        /// if the result of <paramref name="tryTaskValue"/> was in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TErr, TOut>> BindTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOk, Task<Try<TErr, TOut>>> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.BindAsync(ok: ok));
        }

        /// <summary>Binds the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.</summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Err type of the resturn type of <paramref name="err"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="err">
        /// An asynchronous transformation from <typeparamref name="TErr"/> to <see cref="Try{TErr, TOk}"/>.
        /// </param>
        /// <returns>
        /// A task which, when resolved, produces a <see cref="Try{TErr, TOk}"/> with the transformation applied
        /// if the result of <paramref name="tryTaskValue"/> was in the Err state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Try<TOut, TOk>> BindTAsync<TErr, TOk, TOut>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TErr, Task<Try<TOut, TOk>>> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Bind(tv => tv.BindAsync(err: err));
        }

        #endregion

        #region TapT

        /// <summary>
        /// Performs an action on the Err value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="err">An action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapT<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action<TErr> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Map(ov => ov.Tap(err: err));
        }

        /// <summary>
        /// Performs an action on the Ok value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="ok">An action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapT<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(ov => ov.Tap(ok: ok));
        }

        /// <summary>
        /// Performs an action on the Err value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="err">
        /// An asynchronous action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapT<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TErr, Task> err)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return tryTaskValue.Bind(ov => ov.TapAsync(err: err));
        }

        /// <summary>
        /// Performs an action on the Ok value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="ok">
        /// An asynchronous action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapT<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(ov => ov.TapAsync(ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value of <paramref name="tryTaskValue"/>.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An action to perform.</param>
        /// <param name="err">An action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <param name="ok">An action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapT<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Map(tv => tv.Tap(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An action to perform.</param>
        /// <param name="err">An action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <param name="ok">
        /// An asynchronous action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An action to perform.</param>
        /// <param name="err">
        /// An asynchronous action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <param name="ok">An action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An action to perform.</param>
        /// <param name="err">
        /// An asynchronous action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <param name="ok">
        /// An asynchronous action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An asynchronous action to perform.</param>
        /// <param name="err">An action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <param name="ok">An action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An asynchronous action to perform.</param>
        /// <param name="err">An action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <param name="ok">
        /// An asynchronous action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An asynchronous action to perform.</param>
        /// <param name="err">
        /// An asynchronous action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <param name="ok">An action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        /// <summary>
        /// Performs an action based on the state of the Result value
        /// of <paramref name="tryTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TErr">The Err type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <typeparam name="TOk">The Ok type of the Result type of <paramref name="tryTaskValue"/>.</typeparam>
        /// <param name="tryTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="none">An asynchronous action to perform.</param>
        /// <param name="err">
        /// An asynchronous action to perform on the Err value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <param name="ok">
        /// An asynchronous action to perform on the Ok value of the Result value of <paramref name="tryTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="tryTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tryTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Try<TErr, TOk>> TapTAsync<TErr, TOk>(
            [NotNull] this Task<Try<TErr, TOk>> tryTaskValue,
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (tryTaskValue is null) { throw new ArgumentNullException(nameof(tryTaskValue)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return tryTaskValue.Bind(tv => tv.TapAsync(none: none, err: err, ok: ok));
        }

        #endregion
    }
}
