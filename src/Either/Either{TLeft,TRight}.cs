// <copyright file="Either{TLeft,TRight}.cs" company="Cimpress, Inc.">
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static JetBrains.Annotations.ImplicitUseTargetFlags;
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;
using static System.Runtime.InteropServices.LayoutKind;
using static Tiger.Types.EitherState;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values.</summary>
    /// <typeparam name="TLeft">The Left type of the value that may be represented.</typeparam>
    /// <typeparam name="TRight">The Right type of the value that may be represented.</typeparam>
    [DebuggerTypeProxy(typeof(Either<,>.DebuggerTypeProxy))]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct Either<TLeft, TRight>
    {
        readonly TLeft _leftValue;
        readonly TRight _rightValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft, TRight}"/> struct
        /// from the provided <see cref="Either{TLeft, TRight}"/> struct.
        /// </summary>
        /// <param name="eitherValue">The value to copy.</param>
        public Either(in Either<TLeft, TRight> eitherValue) => this = eitherValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct.
        /// </summary>
        /// <param name="leftValue">The value to use as the Left value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TLeft leftValue)
            : this()
        {
            Assume(leftValue != null, EitherConstructNull);

            _leftValue = leftValue;
            State = Left;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct.
        /// </summary>
        /// <param name="rightValue">The value to use as the Right value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TRight rightValue)
            : this()
        {
            Assume(rightValue != null, EitherConstructNull);

            _rightValue = rightValue;
            State = Right;
        }

        /// <summary>Gets a value indicating whether this instance is in the Left state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsLeft => State == Left;

        /// <summary>Gets a value indicating whether this instance is in the Right state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsRight => State == Right;

        /// <summary>Gets the Right value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw if this instance is not in the Right state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException" accessor="get">This instance is in an invalid state.</exception>
        [NotNull]
        public TRight Value
        {
            get
            {
                if (!IsRight) { throw new InvalidOperationException(EitherIsNotRight); }

                return _rightValue;
            }
        }

        /// <summary>Gets the current state of this instance.</summary>
        internal EitherState State { get; }

        #region Operators

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="leftValue">The value to wrap.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TLeft leftValue) => leftValue == null
            ? default
            : new Either<TLeft, TRight>(leftValue);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="rightValue">The value to wrap.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TRight rightValue) => rightValue == null
            ? default
            : new Either<TLeft, TRight>(rightValue);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="leftValue">The value to wrap.</param>
        public static implicit operator Either<TLeft, TRight>(EitherLeft<TLeft> leftValue) =>
            new Either<TLeft, TRight>(leftValue.Value);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="rightValue">The value to wrap.</param>
        public static implicit operator Either<TLeft, TRight>(EitherRight<TRight> rightValue) =>
            new Either<TLeft, TRight>(rightValue.Value);

        /// <summary>Unwraps the Right value of <paramref name="eitherValue"/>.</summary>
        /// <param name="eitherValue">The value to unwrap.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static explicit operator TRight(Either<TLeft, TRight> eitherValue)
        {
            if (!eitherValue.IsRight) { throw new InvalidOperationException(EitherIsNotRight); }

            return eitherValue._rightValue;
        }

        /// <summary>Unwraps the Left value of <paramref name="eitherValue"/>.</summary>
        /// <param name="eitherValue">The value to unwrap.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static explicit operator TLeft(Either<TLeft, TRight> eitherValue)
        {
            if (!eitherValue.IsLeft) { throw new InvalidOperationException(EitherIsNotLeft); }

            return eitherValue._leftValue;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => left.EqualsCore(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !(left == right);

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        [EditorBrowsable(Never)]
        public static Either<TLeft, TRight> ToEither([NotNull] TLeft value) => From(value);

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        [EditorBrowsable(Never)]
        public static Either<TLeft, TRight> ToEither([NotNull] TRight value) => From(value);

        #endregion

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="left">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> From([NotNull] TLeft left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            return new Either<TLeft, TRight>(left);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="right">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> From([NotNull] TRight right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            return new Either<TLeft, TRight>(right);
        }

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(_leftValue)
                : right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(_leftValue)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Void

        /// <summary>Performs an action with this instance by matching on its state.</summary>
        /// <param name="left">
        /// An action to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                right(_rightValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                await right(_rightValue).ConfigureAwait(false);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }
            else
            {
                right(_rightValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }
            else
            {
                await right(_rightValue).ConfigureAwait(false);
            }

            return Unit.Value;
        }

        #endregion

        #endregion

        #region Map

        /// <summary>Maps a function over the Left value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOut, TRight> Map<TOut>([NotNull, InstantHandle] Func<TLeft, TOut> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var result = left(_leftValue);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TOut, TRight>(result);
            }

            return new Either<TOut, TRight>(_rightValue);
        }

        /// <summary>Maps a function over the Left value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TOut, TRight>> MapAsync<TOut>([NotNull, InstantHandle] Func<TLeft, Task<TOut>> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var result = await left(_leftValue).ConfigureAwait(false);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TOut, TRight>(result);
            }

            return new Either<TOut, TRight>(_rightValue);
        }

        /// <summary>Maps a function over the Right value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value set to
        /// the value of invoking <paramref name="right"/> over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeft, TOut> Map<TOut>([NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft) { return new Either<TLeft, TOut>(_leftValue); }

            var result = right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TOut>(result);
        }

        /// <summary>Maps a function over the Right value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value set to
        /// the value of invoking <paramref name="right"/> over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeft, TOut>> MapAsync<TOut>([NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft) { return new Either<TLeft, TOut>(_leftValue); }

            var result = await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TOut>(result);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance, whichever is present.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeftOut, TRightOut> Map<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, TLeftOut> left,
            [NotNull, InstantHandle] Func<TRight, TRightOut> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> MapAsync<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, TLeftOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TRightOut>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> MapAsync<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TLeftOut>> left,
            [NotNull, InstantHandle] Func<TRight, TRightOut> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute  // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> MapAsync<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TLeftOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TRightOut>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        #endregion

        #region Bind

        /// <summary>Binds a function over the Left value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// or if the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its
        /// Left value set to the value of invoking <paramref name="left"/> over the Left value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOut, TRight> Bind<TOut>([NotNull, InstantHandle] Func<TLeft, Either<TOut, TRight>> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left: left).Pipe(v => v.Join());
        }

        /// <summary>
        /// Binds a function over the Left value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// or if the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its
        /// Left value set to the value of invoking <paramref name="left"/> over the Left value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public Task<Either<TOut, TRight>> BindAsync<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOut, TRight>>> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return MapAsync(left: left).Map(v => v.Join());
        }

        /// <summary>Binds a function over the Right value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// or if the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its
        /// Right value set to the value of invoking <paramref name="right"/> over the Right value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeft, TOut> Bind<TOut>([NotNull, InstantHandle] Func<TRight, Either<TLeft, TOut>> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(right: right).Join();
        }

        /// <summary>
        /// Binds a function over the Right value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// or if the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its
        /// Right value set to the value of invoking <paramref name="right"/> over the Right value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public Task<Either<TLeft, TOut>> BindAsync<TOut>(
            [NotNull, InstantHandle] Func<TRight, Task<Either<TLeft, TOut>>> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return MapAsync(right: right).Map(v => v.Join());
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance, whichever is present.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state,
        /// or an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOutLeft, TOutRight> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left: left, right: right).Collapse();
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state,
        /// or an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public Task<Either<TOutLeft, TOutRight>> BindAsync<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return MapAsync(left: left, right: right).Map(v => v.Collapse());
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state,
        /// or an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public Task<Either<TOutLeft, TOutRight>> BindAsync<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return MapAsync(left: left, right: right).Map(v => v.Collapse());
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state,
        /// or an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public Task<Either<TOutLeft, TOutRight>> BindAsync<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return MapAsync(left: left, right: right).Map(v => v.Collapse());
        }

        #endregion

        #region Fold

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with the Left value of this instance.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="left">
        /// A function to invoke with the seed value and the Left value of this instance as the arguments
        /// if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public TState Fold<TState>([NotNull, InstantHandle] Func<TState, TLeft, TState> left)
            where TState : struct
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return IsLeft
                ? left(default, _leftValue)
                : default;
        }

        /// <summary>Combines the provided seed state with the Left value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Left value of this instance.</param>
        /// <param name="left">
        /// A function to invoke with the seed value and the Left value of this instance as the arguments
        /// if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TLeft, TState> left)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(state, _leftValue)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with the Right value of this instance.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="right">
        /// A function to invoke with the seed value and the Right value of this instance as the arguments
        /// if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public TState Fold<TState>([NotNull, InstantHandle] Func<TState, TRight, TState> right)
            where TState : struct
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return IsRight
                ? right(default, _rightValue)
                : default;
        }

        /// <summary>Combines the provided seed state with the Right value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Right value of this instance.</param>
        /// <param name="right">
        /// A function to invoke with the seed value and the Right value of this instance as the arguments
        /// if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TRight, TState> right)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsRight
                ? right(state, _rightValue)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/>
        /// with the Left value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the seed value and the Left value of this instance
        /// as the arguments if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> FoldAsync<TState>([NotNull, InstantHandle] Func<TState, TLeft, Task<TState>> left)
            where TState : struct
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return IsLeft
                ? await left(default, _leftValue).ConfigureAwait(false)
                : default;
        }

        /// <summary>Combines the provided seed state with the Left value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Left value of this instance.</param>
        /// <param name="left">
        /// A function to invoke asynchronously with the seed value and the Left value of this instance
        /// as the arguments if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> FoldAsync<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TLeft, Task<TState>> left)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(state, _leftValue).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/>
        /// with the Right value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="right">
        /// A function to invoke asynchronously with the seed value and the Right value of this instance
        /// as the arguments if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> FoldAsync<TState>([NotNull, InstantHandle] Func<TState, TRight, Task<TState>> right)
            where TState : struct
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return IsRight
                ? await right(default, _rightValue).ConfigureAwait(false)
                : default;
        }

        /// <summary>Combines the provided seed state with the Right value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Right value of this instance.</param>
        /// <param name="right">
        /// A function to invoke asynchronously with the seed value and the Right value of this instance
        /// as the arguments if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> FoldAsync<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TRight, Task<TState>> right)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsRight
                ? await right(state, _rightValue).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action on the Left value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(_leftValue);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> TapAsync([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="right">An action to perform on the Right value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                right(_rightValue);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="right">An action to perform on the Right value of this instance, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> TapAsync([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                await right(_rightValue).ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left or Right value of this instance,
        /// whichever is present, and returns this instance.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance.</param>
        /// <param name="right">An action to perform on the Right value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Either<TLeft, TRight> Tap(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsLeft)
            {
                left(_leftValue);
                return this;
            }

            if (IsRight)
            {
                right(_rightValue);
                return this;
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left or Right value of this instance,
        /// whichever is present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance, asynchronously.</param>
        /// <param name="right">An action to perform on the Right value of this instance.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        public async Task<Either<TLeft, TRight>> TapAsync(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
                return this;
            }

            if (IsRight)
            {
                right(_rightValue);
                return this;
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left or Right value of this instance,
        /// whichever is present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance.</param>
        /// <param name="right">An action to perform on the Right value of this instance, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> TapAsync(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsLeft)
            {
                left(_leftValue);
                return this;
            }

            if (IsRight)
            {
                await right(_rightValue).ConfigureAwait(false);
                return this;
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left or Right value of this instance,
        /// whichever is present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="left">An action to perform on the Left value of this instance, asynchronously.</param>
        /// <param name="right">An action to perform on the Right value of this instance, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> TapAsync(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
                return this;
            }

            if (IsRight)
            {
                await right(_rightValue).ConfigureAwait(false);
                return this;
            }

            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Left value of this instance.</summary>
        /// <param name="left">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(_leftValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                right(_rightValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Left value of this instance, asynchronously.</summary>
        /// <param name="left">An action to perform asynchronously.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> LetAsync([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left is null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> LetAsync([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right is null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                await right(_rightValue).ConfigureAwait(false);
            }

            return Unit.Value;
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Either<TLeft, TRight> Recover([NotNull] TRight recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            return IsRight
                ? this
                : new Either<TLeft, TRight>(recoverer);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Either<TLeft, TRight> Recover([NotNull, InstantHandle] Func<TRight> recoverer)
        {
            if (recoverer is null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = recoverer();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TRight>(result);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state, asynchronously.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is the value of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure, NotNull]
        public async Task<Either<TLeft, TRight>> RecoverAsync([NotNull, InstantHandle] Func<Task<TRight>> recoverer)
        {
            if (recoverer is null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Replace

        /// <summary>Replaces the Right value of this instance with the provided value.</summary>
        /// <typeparam name="TOut">The type of the replacement value.</typeparam>
        /// <param name="replacement">The value to use as a replacement.</param>
        /// <returns>
        /// An <see cref="Either{TLeft, TRight}"/> in the Right state whose Right value is the value
        /// of <paramref name="replacement"/> if this instance is in the Right state; otherwise,
        /// an <see cref="Either{TLeft, TRight}"/> in the Left state whose Left value is the
        /// Left value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="replacement"/> is <see langword="null"/>.</exception>
        public Either<TLeft, TOut> Replace<TOut>([NotNull] TOut replacement)
        {
            if (replacement == null) { throw new ArgumentNullException(nameof(replacement)); }

            return IsLeft
                ? new Either<TLeft, TOut>(_leftValue)
                : new Either<TLeft, TOut>(replacement);
        }

        #endregion

        #region Value

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the default value of <typeparamref name="TRight"/>.
        /// </returns>
        /// <remarks>
        /// <para>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TRight"/> satisfies <see langword="class"/>.</para>
        /// </remarks>
        [CanBeNull, Pure]
        public TRight GetValueOrDefault() => _rightValue;

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TRight GetValueOrDefault([NotNull] TRight other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return IsRight
                ? _rightValue
                : other;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">A function producing an alternative value.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the result of invoking <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TRight GetValueOrDefault([NotNull, InstantHandle] Func<TRight> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? _rightValue
                : other();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state, asynchronously.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the result of invoking <paramref name="other"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TRight> GetValueOrDefaultAsync([NotNull, InstantHandle] Func<Task<TRight>> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? _rightValue
                : await other().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Either{TLeft,TRight}"/>.</returns>
        [Pure, EditorBrowsable(Never)]
        public IEnumerator<TRight> GetEnumerator()
        { // note(cosborn) OK, it's kind of an implementation.
            if (IsRight)
            {
                yield return _rightValue;
            }
        }

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString()
        {
            switch (State)
            {
                case Left:
                    return $"Left({_leftValue})";
                case Right:
                    return $"Right({_rightValue})";
                default: // note(cosborn) Why would you change this enum???
                    return "Bottom";
            }
        }

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Either<TLeft, TRight> either && EqualsCore(either);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode()
        {
            switch (State)
            {
                case Left:
                    return _leftValue.GetHashCode();
                case Right:
                    return _rightValue.GetHashCode();
                default: // note(cosborn) Why would you change this enum???
                    return 0;
            }
        }

        /// <summary>Compares this instance with another instance for equality.</summary>
        /// <param name="other">Another instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance and <paramref name="other"/> are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        internal bool EqualsCore(in Either<TLeft, TRight> other)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (State == Bottom && other.State == Bottom)
            {
                return true;
            }

            if (IsLeft && other.IsLeft)
            {
                return EqualityComparer<TLeft>.Default.Equals(_leftValue, other._leftValue);
            }

            if (IsRight && other.IsRight)
            {
                return EqualityComparer<TRight>.Default.Equals(_rightValue, other._rightValue);
            }

            // note(cosborn) Implicitly `_state != other._state`.
            return false;
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            left: l => new { State = Left, Value = l },
            right: r => new { State = Right, Value = r });

        #endregion

        #endregion

        [UsedImplicitly(WithMembers)]
        sealed class DebuggerTypeProxy
        {
            readonly Either<TLeft, TRight> _value;

            /// <summary>Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.</summary>
            /// <param name="value">The either value to proxy.</param>
            public DebuggerTypeProxy(in Either<TLeft, TRight> value)
            {
                _value = value;
            }

            /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
            [CanBeNull]
            public TLeft LeftValue => _value._leftValue;

            /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
            [CanBeNull]
            public TRight RightValue => _value._rightValue;

            /// <summary>Gets the internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
            public string State => _value.State.ToString();
        }
    }
}
