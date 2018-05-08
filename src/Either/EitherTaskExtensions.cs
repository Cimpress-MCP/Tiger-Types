// <copyright file="EitherTaskExtensions.cs" company="Cimpress, Inc.">
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
    /// specialized for <see cref="Either{TLeft,TRight}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class EitherTaskExtensions
    {
        #region MatchT

        /// <summary>
        /// Transforms the result of <paramref name="eitherTaskValue"/> based on its state.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="left">A transformation from <typeparamref name="TLeft"/> to <typeparamref name="TOut"/> to perform in the Left case.</param>
        /// <param name="right">A transformation from <typeparamref name="TRight"/> to <typeparamref name="TOut"/> to perform in the Right case.</param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of <paramref name="left"/> or <paramref name="right"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TLeft, TRight, TOut>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Map(ev => ev.Match(left: left, right: right));
        }

        /// <summary>
        /// Transforms the result of <paramref name="eitherTaskValue"/> based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="left">
        /// A transformation from <typeparamref name="TLeft"/> to <typeparamref name="TOut"/> to perform in the Left case.
        /// </param>
        /// <param name="right">
        /// An asynchronous transformation from <typeparamref name="TRight"/> to <typeparamref name="TOut"/> to perform in the Right case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of <paramref name="left"/> or <paramref name="right"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TLeft, TRight, TOut>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.MatchAsync(left: left, right: right));
        }

        /// <summary>
        /// Transforms the result of <paramref name="eitherTaskValue"/> asynchronously based on its state.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="left">
        /// An asynchronous transformation from <typeparamref name="TLeft"/> to <typeparamref name="TOut"/> to perform in the Left case.
        /// </param>
        /// <param name="right">
        /// A transformation from <typeparamref name="TRight"/> to <typeparamref name="TOut"/> to perform in the Right case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of <paramref name="left"/> or <paramref name="right"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TLeft, TRight, TOut>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.MatchAsync(left: left, right: right));
        }

        /// <summary>
        /// Transforms the result of <paramref name="eitherTaskValue"/> asynchronously based on its state.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to match.</param>
        /// <param name="left">
        /// An asynchronous transformation from <typeparamref name="TLeft"/> to <typeparamref name="TOut"/> to perform in the Left case.
        /// </param>
        /// <param name="right">
        /// An asynchronous transformation from <typeparamref name="TRight"/> to <typeparamref name="TOut"/> to perform in the Right case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of <paramref name="left"/> or <paramref name="right"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchTAsync<TLeft, TRight, TOut>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.MatchAsync(left: left, right: right));
        }

        #endregion

        #region MapT

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="right"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="right">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TLeft, TOut>> MapT<TLeft, TIn, TOut>(
            [NotNull] this Task<Either<TLeft, TIn>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Map(ev => ev.Map(right: right));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="right"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="right">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TLeft, TOut>> MapTAsync<TLeft, TIn, TOut>(
            [NotNull] this Task<Either<TLeft, TIn>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.MapAsync(right: right));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="left"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="left">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Left state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TOut, TRight>> MapT<TRight, TIn, TOut>(
            [NotNull] this Task<Either<TIn, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Map(ev => ev.Map(left: left));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.
        /// </summary>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Result type of the return type of <paramref name="left"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to map.</param>
        /// <param name="left">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TOut, TRight>> MapTAsync<TRight, TIn, TOut>(
            [NotNull] this Task<Either<TIn, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Bind(ev => ev.MapAsync(left: left));
        }

        #endregion

        #region BindT

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Right type of the return type of <paramref name="right"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="right">
        /// A transformation from <typeparamref name="TIn"/> to <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TLeft, TOut>> BindT<TLeft, TIn, TOut>(
            [NotNull] this Task<Either<TLeft, TIn>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Either<TLeft, TOut>> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Map(ev => ev.Bind(right: right));
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Right type of the Result type of the return type of <paramref name="right"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="right">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TLeft, TOut>> BindTAsync<TLeft, TIn, TOut>(
            [NotNull] this Task<Either<TLeft, TIn>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<Either<TLeft, TOut>>> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.BindAsync(right: right));
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Left type of the return type of <paramref name="left"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="left">
        /// A transformation from <typeparamref name="TIn"/> to <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TOut, TRight>> BindT<TRight, TIn, TOut>(
            [NotNull] this Task<Either<TIn, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Either<TOut, TRight>> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Map(ev => ev.Bind(left: left));
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation, asynchronously.
        /// </summary>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TIn">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Left type of the Result type of the return type of <paramref name="left"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to bind.</param>
        /// <param name="left">An asynchronous transformation from <typeparamref name="TIn"/> to <see cref="Either{TLeft,TRight}"/>.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> with the transformation applied if the result of
        /// <paramref name="eitherTaskValue"/> was in the Right state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="eitherTaskValue"/> produced a result that has not been initialized.</exception>
        [NotNull, Pure]
        public static Task<Either<TOut, TRight>> BindTAsync<TRight, TIn, TOut>(
            [NotNull] this Task<Either<TIn, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<Either<TOut, TRight>>> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Bind(ev => ev.BindAsync(left: left));
        }

        #endregion

        #region TapT

        /// <summary>
        /// Performs an action on the Left value of <paramref name="eitherTaskValue"/>,
        /// if present, and returns <paramref name="eitherTaskValue"/>.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapT<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Action<TLeft> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Map(ev => ev.Tap(left: left));
        }

        /// <summary>
        /// Performs an action on the Left value of <paramref name="eitherTaskValue"/>,
        /// if present, and returns <paramref name="eitherTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">
        /// An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.
        /// </param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapTAsync<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            return eitherTaskValue.Bind(ev => ev.TapAsync(left: left));
        }

        /// <summary>
        /// Performs an action on the Right value of <paramref name="eitherTaskValue"/>,
        /// if present, and returns <paramref name="eitherTaskValue"/>.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="right">An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapT<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Map(ev => ev.Tap(right: right));
        }

        /// <summary>
        /// Performs an action on the Right value of <paramref name="eitherTaskValue"/>,
        /// if present, and returns <paramref name="eitherTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="right">
        /// An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.
        /// </param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapTAsync<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.TapAsync(right: right));
        }

        /// <summary>
        /// Performs an action on the Left or Right value of the Result value
        /// of <paramref name="eitherTaskValue"/>, whichever is present, and returns <paramref name="eitherTaskValue"/>.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <param name="right">An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapT<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Map(ev => ev.Tap(left: left, right: right));
        }

        /// <summary>
        /// Performs an action on the Left or Right value of the Result value
        /// of <paramref name="eitherTaskValue"/>, whichever is present, and returns <paramref name="eitherTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.</param>
        /// <param name="right">An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapTAsync<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.TapAsync(left: left, right: right));
        }

        /// <summary>
        /// Performs an action on the Left or Right value of the Result value
        /// of <paramref name="eitherTaskValue"/>, whichever is present, and returns <paramref name="eitherTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>.</param>
        /// <param name="right">An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapTAsync<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.TapAsync(left: left, right: right));
        }

        /// <summary>
        /// Performs an action on the Left or Right value of the Result value
        /// of <paramref name="eitherTaskValue"/>, whichever is present, and returns <paramref name="eitherTaskValue"/>, asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of the Result type of <paramref name="eitherTaskValue"/>.</typeparam>
        /// <param name="eitherTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="left">An action to perform on the Left value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.</param>
        /// <param name="right">An action to perform on the Right value of the Result value of <paramref name="eitherTaskValue"/>, asynchronously.</param>
        /// <returns>The original value of <paramref name="eitherTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Either<TLeft, TRight>> TapTAsync<TLeft, TRight>(
            [NotNull] this Task<Either<TLeft, TRight>> eitherTaskValue,
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (eitherTaskValue is null) { throw new ArgumentNullException(nameof(eitherTaskValue)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            return eitherTaskValue.Bind(ev => ev.TapAsync(left: left, right: right));
        }

        #endregion
    }
}
