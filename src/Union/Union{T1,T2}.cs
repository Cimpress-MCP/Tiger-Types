// <copyright file="Union{T1,T2}.cs" company="Cimpress, Inc.">
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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Represents a value that is a composite of two values.</summary>
    /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
    /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
    [DebuggerTypeProxy(typeof(Union<,>.DebuggerTypeProxy))]
    public partial class Union<T1, T2>
    {
        readonly T1 _value1;
        readonly T2 _value2;
        readonly int _state;

        /// <summary>Initializes a new instance of the <see cref="Union{T1, T2}"/> class.</summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        Union([NotNull] T1 value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            _value1 = value;
            _state = 1;
        }

        /// <summary>Initializes a new instance of the <see cref="Union{T1, T2}"/> class.</summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        Union([NotNull] T2 value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            _value2 = value;
            _state = 2;
        }

        /// <summary>Gets a value indicating whether this instance is in the first state.</summary>
        public bool IsState1 => _state == 1;

        /// <summary>Gets a value indicating whether this instance is in the second state.</summary>
        public bool IsState2 => _state == 2;

        /// <summary>Gets the first value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw
        /// if this instance is not in the first state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is not in the specified state.
        /// </exception>
        [NotNull]
        public T1 Value1
        {
            get
            {
                if (!IsState1) { throw new InvalidOperationException(UnionDoesNotMatch); }

                return _value1;
            }
        }

        /// <summary>Gets the second value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw
        /// if this instance is not in the second state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is not in the specified state.
        /// </exception>
        [NotNull]
        public T2 Value2
        {
            get
            {
                if (!IsState2) { throw new InvalidOperationException(UnionDoesNotMatch); }

                return _value2;
            }
        }

        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Union{T1,T2}"/> in the first state.</returns>
        [NotNull]
        public static Union<T1, T2> From([NotNull] T1 value) => new Union<T1, T2>(value);

        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Union{T1,T2}"/> in the second state.</returns>
        [NotNull]
        public static Union<T1, T2> From([NotNull] T2 value) => new Union<T1, T2>(value);

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="one">
        /// A function to be invoked with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// A function to be invoked with the second value of this instance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>A value produced by <paramref name="one"/> or <paramref name="two"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [CanBeNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<T1, TOut> one,
            [NotNull, InstantHandle] Func<T2, TOut> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    return one(_value1);
                case 2:
                    return two(_value2);
                default: // because(cosborn) Hush, ReSharper.
                    return default;
            }
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="one">
        /// A function to be invoked asynchronously with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// A function to be invoked with the second value of this instance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>A value produced by <paramref name="one"/> or <paramref name="two"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<T1, Task<TOut>> one,
            [NotNull, InstantHandle] Func<T2, TOut> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    return await one(_value1).ConfigureAwait(false);
                case 2:
                    return two(_value2);
                default: // because(cosborn) Hush, ReSharper.
                    return default;
            }
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="one">
        /// A function to be invoked with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// A function to be invoked asynchronously with the second value of this instance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>A value produced by <paramref name="one"/> or <paramref name="two"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<T1, TOut> one,
            [NotNull, InstantHandle] Func<T2, Task<TOut>> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    return one(_value1);
                case 2:
                    return await two(_value2).ConfigureAwait(false);
                default: // because(cosborn) Hush, ReSharper.
                    return default;
            }
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="one">
        /// A function to be invoked asynchronously with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// A function to be invoked asynchronously with the second value of this instance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>A value produced by <paramref name="one"/> or <paramref name="two"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<T1, Task<TOut>> one,
            [NotNull, InstantHandle] Func<T2, Task<TOut>> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    return one(_value1);
                case 2:
                    return two(_value2);
                default: // because(cosborn) Hush, ReSharper.
                    return Task.FromResult(default(TOut));
            }
        }

        #endregion

        #region Void

        /// <summary>Performs an action on with this instance by matching on its state.</summary>
        /// <param name="one">
        /// An action to be invoked with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// An action to be invoked with the second value of this isntance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action<T1> one,
            [NotNull, InstantHandle] Action<T2> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    one(_value1);
                    break;
                case 2:
                    two(_value2);
                    break;
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on with this instance by matching on its state, asynchronously.</summary>
        /// <param name="one">
        /// An action to be invoked asynchronously with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// An action to be invoked with the second value of this isntance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Func<T1, Task> one,
            [NotNull, InstantHandle] Action<T2> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    await one(_value1).ConfigureAwait(false);
                    return;
                case 2:
                    two(_value2);
                    return;
            }
        }

        /// <summary>Performs an action on with this instance by matching on its state, asynchronously.</summary>
        /// <param name="one">
        /// An action to be invoked with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// An action to be invoked asynchronously with the second value of this isntance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Action<T1> one,
            [NotNull, InstantHandle] Func<T2, Task> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    one(_value1);
                    return;
                case 2:
                    await two(_value2).ConfigureAwait(false);
                    return;
            }
        }

        /// <summary>Performs an action on with this instance by matching on its state, asynchronously.</summary>
        /// <param name="one">
        /// An action to be invoked asynchronously with the first value of this instance
        /// as the argument if this instance is in the first state.
        /// </param>
        /// <param name="two">
        /// An action to be invoked asynchronously with the second value of this isntance
        /// as the argument if this instance is in the second state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Func<T1, Task> one,
            [NotNull, InstantHandle] Func<T2, Task> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (_state)
            {
                case 1:
                    await one(_value1).ConfigureAwait(false);
                    return;
                case 2:
                    await two(_value2).ConfigureAwait(false);
                    return;
            }
        }

        #endregion

        #endregion
    }
}
