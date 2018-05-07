// <copyright file="Try{TErr,TOk}.cs" company="Cimpress, Inc.">
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
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;
using static System.Runtime.InteropServices.LayoutKind;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values, or none.</summary>
    /// <typeparam name="TErr">The Error type of the value that may be represented.</typeparam>
    /// <typeparam name="TOk">The OK type of the value that may be represented.</typeparam>
    [DebuggerTypeProxy(typeof(Try<,>.DebuggerTypeProxy))]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct Try<TErr, TOk>
    {
        /// <summary>A value representing no value.</summary>
        public static readonly Try<TErr, TOk> None;

        readonly Option<Either<TErr, TOk>> _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Try{TErr, TOk}"/> struct.
        /// </summary>
        /// <param name="tryValue">The value to copy.</param>
        public Try(in Try<TErr, TOk> tryValue) => this = tryValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Try{TErr, TOk}"/> struct.
        /// </summary>
        /// <param name="optionEitherValue">The value to use as the inner value.</param>
        internal Try(in Option<Either<TErr, TOk>> optionEitherValue)
        {
            _value = optionEitherValue;
        }

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsNone => _value.IsNone;

        /// <summary>Gets a value indicating whether this instance is in the Err state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsErr => _value.Map(e => e.IsLeft).GetValueOrDefault(false);

        /// <summary>Gets a value indicating whether this instance is in the Ok state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsOk => _value.Map(e => e.IsRight).GetValueOrDefault(false);

        /// <summary>Gets the Ok value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw if this instance is not in the Ok state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        public TOk Value
        {
            get
            {
                if (!IsOk) { throw new InvalidOperationException(TryIsNotOk); }

                // note(cosborn) ಠ_ಠ
                return _value.Value.Value;
            }
        }

        #region Operators

        /// <summary>Wraps a value in <see cref="Try{TErr, TOk}"/>.</summary>
        /// <param name="errValue">The value to wrap.</param>
        public static implicit operator Try<TErr, TOk>([CanBeNull] TErr errValue) => ToTry(errValue);

        /// <summary>Wraps a value in <see cref="Try{TErr, TOk}"/>.</summary>
        /// <param name="okValue">The value to wrap.</param>
        public static implicit operator Try<TErr, TOk>([CanBeNull] TOk okValue) => ToTry(okValue);

        /// <summary>Unwraps the Err value of <paramref name="tryValue"/>.</summary>
        /// <param name="tryValue">The value to unwrap.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static explicit operator TErr(Try<TErr, TOk> tryValue) => (TErr)tryValue._value.Value;

        /// <summary>Unwraps the Ok value of <paramref name="tryValue"/>.</summary>
        /// <param name="tryValue">The value to unwrap.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static explicit operator TOk(Try<TErr, TOk> tryValue) => tryValue.Value;

        /// <summary>
        /// Implicitly converts a <see cref="TryNone"/> to a
        /// <see cref="Try{TErr, TOk}"/> in the None state.
        /// </summary>
        /// <param name="none">The default value of <see cref="TryNone"/>.</param>
        [SuppressMessage("Roslynator", "RCS1163", Justification = "Used only for the type inference.")]
        public static implicit operator Try<TErr, TOk>(TryNone none) => None;

        /// <summary>
        /// Implicitly converts a <see cref="TryErr{TErr}"/> to a
        /// <see cref="Try{TErr, TOk}"/> in the Err state.
        /// </summary>
        /// <param name="err">The partially applied try value.</param>
        public static implicit operator Try<TErr, TOk>(TryErr<TErr> err) => From(err.Value);

        /// <summary>
        /// Implicitly converts a <see cref="TryOk{TOk}"/> to a
        /// <see cref="Try{TErr, TOk}"/> in the Ok state.
        /// </summary>
        /// <param name="ok">The partially applied try value.</param>
        public static implicit operator Try<TErr, TOk>(TryOk<TOk> ok) => From(ok.Value);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Try<TErr, TOk> left, Try<TErr, TOk> right) => left.EqualsCore(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Try<TErr, TOk> left, Try<TErr, TOk> right) => !(left == right);

        /// <summary>Wraps a value in <see cref="Try{TErr, TOk}"/>.</summary>
        /// <param name="errValue">The value to wrap.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if <paramref name="errValue"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Err state whose Err value is the value of <paramref name="errValue"/>.
        /// </returns>
        [Pure]
        [EditorBrowsable(Never)]
        public static Try<TErr, TOk> ToTry([CanBeNull] TErr errValue) => From(errValue);

        /// <summary>Wraps a value in <see cref="Try{TErr, TOk}"/>.</summary>
        /// <param name="okValue">The value to wrap.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if <paramref name="okValue"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the value of <paramref name="okValue"/>.
        /// </returns>
        [Pure]
        [EditorBrowsable(Never)]
        public static Try<TErr, TOk> ToTry([CanBeNull] TOk okValue) => From(okValue);

        #endregion

        /// <summary>
        /// Creates a <see cref="Try{TErr, TOk}"/> in the Err or None state
        /// from the provided value.
        /// </summary>
        /// <param name="err">The value to wrap.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if
        /// <paramref name="err"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Err state.
        /// </returns>
        public static Try<TErr, TOk> From([CanBeNull] TErr err) =>
            new Try<TErr, TOk>(Option.From(err).Map(Either.From<TErr, TOk>));

        /// <summary>
        /// Creates a <see cref="Try{TErr, TOk}"/> in the Ok or None state
        /// from the provided value.
        /// </summary>
        /// <param name="ok">The value to wrap.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if
        /// <paramref name="ok"/> is <see langword="null"/>;
        /// otherwise, a <see cref="Try{TErr, TOk}"/> in the Ok state.
        /// </returns>
        public static Try<TErr, TOk> From([CanBeNull] TOk ok) =>
            new Try<TErr, TOk>(Option.From(ok).Map(Either.From<TErr, TOk>));

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public TOut Match<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Match(
                none: none,
                some: e => e.Match(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Match(
                none: none,
                some: e => e.Match(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke asynchronously if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.Match(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke asynchronously if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, TOut> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke asynchronously if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A function to invoke asynchronously if this instance is in the None state.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A value produced by <paramref name="none"/>, <paramref name="err"/>,
        /// or <paramref name="ok"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TErr, Task<TOut>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        #endregion

        #region Void

        /// <summary>Performs an action with this instance by matching on its state.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Match(
                none: none,
                some: e => e.Match(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke asynchronously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.Match(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke asynchonously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="err">
        /// An action to invoke asynchonously with the Err value of this instance as
        /// the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// An action to invoke asynchronously with the Ok value of this instance as
        /// the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MatchAsync(
                none: none,
                some: e => e.MatchAsync(
                    left: err,
                    right: ok));
        }

        #endregion

        #endregion

        #region Map

        /// <summary>Maps a function over the Err value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Ok state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        public Try<TOut, TOk> Map<TOut>([NotNull, InstantHandle] Func<TErr, TOut> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return new Try<TOut, TOk>(_value.Map(ev => ev.Map(left: err)));
        }

        /// <summary>Maps a function over the Err value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Ok state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TOut, TOk>> MapAsync<TOut>([NotNull, InstantHandle] Func<TErr, Task<TOut>> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.MapAsync(ev => ev.MapAsync(left: err)).Map(oev => new Try<TOut, TOk>(oev));
        }

        /// <summary>Maps a function over the Ok value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Try<TErr, TOut> Map<TOut>([NotNull, InstantHandle] Func<TOk, TOut> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return new Try<TErr, TOut>(_value.Map(ev => ev.Map(right: ok)));
        }

        /// <summary>Maps a function over the Ok value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TErr, TOut>> MapAsync<TOut>([NotNull, InstantHandle] Func<TOk, Task<TOut>> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.MapAsync(right: ok)).Map(oev => new Try<TErr, TOut>(oev));
        }

        /// <summary>
        /// Maps a function over the Err or Ok value of this instance, if present.
        /// </summary>
        /// <typeparam name="TOutErr">The type to which to map the Err value of this instance.</typeparam>
        /// <typeparam name="TOutOk">The type to which to map the Ok value of this instance.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the Non state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Try<TOutErr, TOutOk> Map<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, TOutErr> err,
            [NotNull, InstantHandle] Func<TOk, TOutOk> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return new Try<TOutErr, TOutOk>(_value.Map(ev => ev.Map(left: err, right: ok)));
        }

        /// <summary>
        /// Maps a function over the Err or Ok value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutErr">The type to which to map the Err value of this instance.</typeparam>
        /// <typeparam name="TOutOk">The type to which to map the Ok value of this instance.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the Non state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TOutErr, TOutOk>> MapAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, TOutErr> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOutOk>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.MapAsync(left: err, right: ok)).Map(oev => new Try<TOutErr, TOutOk>(oev));
        }

        /// <summary>
        /// Maps a function over the Err or Ok value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutErr">The type to which to map the Err value of this instance.</typeparam>
        /// <typeparam name="TOutOk">The type to which to map the Ok value of this instance.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the Non state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TOutErr, TOutOk>> MapAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Task<TOutErr>> err,
            [NotNull, InstantHandle] Func<TOk, TOutOk> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.MapAsync(left: err, right: ok)).Map(oev => new Try<TOutErr, TOutOk>(oev));
        }

        /// <summary>
        /// Maps a function over the Err or Ok value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutErr">The type to which to map the Err value of this instance.</typeparam>
        /// <typeparam name="TOutOk">The type to which to map the Ok value of this instance.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the Non state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value set to
        /// the value of invoking <paramref name="err"/> over the Err value of this instance,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value set to
        /// the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TOutErr, TOutOk>> MapAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Task<TOutErr>> err,
            [NotNull, InstantHandle] Func<TOk, Task<TOutOk>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.MapAsync(left: err, right: ok)).Map(oev => new Try<TOutErr, TOutOk>(oev));
        }

        #endregion

        #region Bind

        /// <summary>Binds a function over the Err value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Some state
        /// or if the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is in the Ok state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value
        /// set to the value of invoking <paramref name="err"/> over the Err value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        public Try<TOut, TOk> Bind<TOut>([NotNull, InstantHandle] Func<TErr, Try<TOut, TOk>> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return Map(err: err).Join();
        }

        /// <summary>Binds a function over the Err value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Some state
        /// or if the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is in the Ok state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Err state with its Err value
        /// set to the value of invoking <paramref name="err"/> over the Err value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TOut, TOk>> BindAsync<TOut>([NotNull, InstantHandle] Func<TErr, Task<Try<TOut, TOk>>> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return MapAsync(err: err).Map(tv => tv.Join());
        }

        /// <summary>Binds a function over the Ok value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="ok"/> over the Ok value of this instance
        /// is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state
        /// or if the result of invoking <paramref name="ok"/> over the Ok value of this instance
        /// is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value
        /// set to the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Try<TErr, TOut> Bind<TOut>([NotNull, InstantHandle] Func<TOk, Try<TErr, TOut>> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return Map(ok: ok).Join();
        }

        /// <summary>Binds a function over the Ok value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="ok"/> over the Ok value of this instance
        /// is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state
        /// or if the result of invoking <paramref name="ok"/> over the Ok value of this instance
        /// is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state with its Ok value
        /// set to the value of invoking <paramref name="ok"/> over the Ok value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Try<TErr, TOut>> BindAsync<TOut>([NotNull, InstantHandle] Func<TOk, Task<Try<TErr, TOut>>> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return MapAsync(ok: ok).Map(tv => tv.Join());
        }

        /// <summary>Binds a function over the Err or Ok value of this instance, if present.</summary>
        /// <typeparam name="TOutErr">The type to which to bind the Err type.</typeparam>
        /// <typeparam name="TOutOk">The type to which to bind the Ok type.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state
        /// if this instance is in the None state
        /// or if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the None state
        /// or if this instance is in the Ok state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Err state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Ok state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instane is in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Try<TOutErr, TOutOk> Bind<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Try<TOutErr, TOutOk>> err,
            [NotNull, InstantHandle] Func<TOk, Try<TOutErr, TOutOk>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return Map(err: err, ok: ok).Collapse(none: Try<TOutErr, TOutOk>.None);
        }

        /// <summary>Binds a function over the Err or Ok value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOutErr">The type to which to bind the Err type.</typeparam>
        /// <typeparam name="TOutOk">The type to which to bind the Ok type.</typeparam>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state
        /// if this instance is in the None state
        /// or if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the None state
        /// or if this instance is in the Ok state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Err state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Ok state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instane is in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Task<Try<TOutErr, TOutOk>> BindAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Try<TOutErr, TOutOk>> err,
            [NotNull, InstantHandle] Func<TOk, Task<Try<TOutErr, TOutOk>>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return MapAsync(err: err, ok: ok).Map(ttv => ttv.Collapse(none: Try<TOutErr, TOutOk>.None));
        }

        /// <summary>Binds a function over the Err or Ok value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOutErr">The type to which to bind the Err type.</typeparam>
        /// <typeparam name="TOutOk">The type to which to bind the Ok type.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state
        /// if this instance is in the None state
        /// or if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the None state
        /// or if this instance is in the Ok state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Err state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Ok state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instane is in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Task<Try<TOutErr, TOutOk>> BindAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Task<Try<TOutErr, TOutOk>>> err,
            [NotNull, InstantHandle] Func<TOk, Try<TOutErr, TOutOk>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return MapAsync(err: err, ok: ok).Map(ttv => ttv.Collapse(none: Try<TOutErr, TOutOk>.None));
        }

        /// <summary>Binds a function over the Err or Ok value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOutErr">The type to which to bind the Err type.</typeparam>
        /// <typeparam name="TOutOk">The type to which to bind the Ok type.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the None state
        /// if this instance is in the None state
        /// or if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the None state
        /// or if this instance is in the Ok state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the None state,
        /// a <see cref="Try{TErr, TOk}"/> in the Err state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Err state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instance is in the Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the Ok state
        /// if this instance is in the Err state and the result of invoking <paramref name="err"/> over the Err value of this instance is in the Ok state
        /// or if this instance is in the OK state and the result of invoking <paramref name="ok"/> over the Ok value of this instane is in the Ok state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Task<Try<TOutErr, TOutOk>> BindAsync<TOutErr, TOutOk>(
            [NotNull, InstantHandle] Func<TErr, Task<Try<TOutErr, TOutOk>>> err,
            [NotNull, InstantHandle] Func<TOk, Task<Try<TOutErr, TOutOk>>> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return MapAsync(err: err, ok: ok).Map(ttv => ttv.Collapse(none: Try<TOutErr, TOutOk>.None));
        }

        #endregion

        #region Filter

        /// <summary>Filters the Err value of this instance based on a provided condition.</summary>
        /// <param name="err">
        /// A function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state
        /// ans the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is <see langword="true"/>; otherwise, a <see cref="Try{TErr, TOk}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [Pure]
        public Try<TErr, TOk> Filter([NotNull, InstantHandle] Func<TErr, bool> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return new Try<TErr, TOk>(_value.Filter(ev => ev.IsLeft && err((TErr)ev)));
        }

        /// <summary>
        /// Filters the Err value of this instance based on a provided condition, asynchronously.
        /// </summary>
        /// <param name="err">
        /// An asynchronous function to invoke with the Err value of this instance
        /// as the argument if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Err state if this instance is in the Err state
        /// ans the result of invoking <paramref name="err"/> over the Err value of this instance
        /// is <see langword="true"/>; otherwise, a <see cref="Try{TErr, TOk}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<Try<TErr, TOk>> FilterAsync([NotNull, InstantHandle] Func<TErr, Task<bool>> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.FilterAsync(async ev => ev.IsLeft && await err((TErr)ev).ConfigureAwait(false))
                .Map(oev => new Try<TErr, TOk>(oev));
        }

        /// <summary>Filters the Ok value of this instance based on a provided condition.</summary>
        /// <param name="ok">
        /// A function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Err state
        /// ans the result of invoking <paramref name="ok"/> over the Err value of this instance
        /// is <see langword="true"/>; otherwise, a <see cref="Try{TErr, TOk}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [Pure]
        public Try<TErr, TOk> Filter([NotNull, InstantHandle] Func<TOk, bool> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return new Try<TErr, TOk>(_value.Filter(ev => ev.IsRight && ok(ev.Value)));
        }

        /// <summary>
        /// Filters the Ok value of this instance based on a provided condition, asynchronously.
        /// </summary>
        /// <param name="ok">
        /// An asynchronous function to invoke with the Ok value of this instance
        /// as the argument if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state if this instance is in the Ok state
        /// ans the result of invoking <paramref name="ok"/> over the Err value of this instance
        /// is <see langword="true"/>; otherwise, a <see cref="Try{TErr, TOk}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<Try<TErr, TOk>> FilterAsync([NotNull, InstantHandle] Func<TOk, Task<bool>> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.FilterAsync(async ev => ev.IsLeft && await ok(ev.Value).ConfigureAwait(false))
                .Map(oev => new Try<TErr, TOk>(oev));
        }

        #endregion

        #region Fold

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with the Err value of this instance.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="err">
        /// A function to invoke with the seed value and the Err value of this instance as the arguments
        /// if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// The result of combining the seed value with the Err value of this instance
        /// if this instance is in the Err state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [Pure]
        public TState Fold<TState>([NotNull, InstantHandle] Func<TState, TErr, TState> err)
            where TState : struct
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.Map(ev => ev.Fold(left: err)).GetValueOrDefault(default(TState));
        }

        /// <summary>Combines the provided seed state with the Err value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Err value of this instance.</param>
        /// <param name="err">
        /// A function to invoke with the seed value and the Err value of this instance as the arguments
        /// if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Err value of this instance
        /// if this instance is in the Err state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TErr, TState> err)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.Map(ev => ev.Fold(state, left: err)).GetValueOrDefault(state);
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/> with the Ok value of this instance.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="ok">
        /// A function to invoke with the seed value and the Ok value of this instance as the arguments
        /// if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// The result of combining the seed value with the Ok value of this instance
        /// if this instance is in the Ok state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [Pure]
        public TState Fold<TState>(
            [NotNull, InstantHandle] Func<TState, TOk, TState> ok)
            where TState : struct
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Map(ev => ev.Fold(right: ok)).GetValueOrDefault(default(TState));
        }

        /// <summary>Combines the provided seed state with the Ok value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Ok value of this instance.</param>
        /// <param name="ok">
        /// A function to invoke with the seed value and the Ok value of this instance as the arguments
        /// if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Ok value of this instance
        /// if this instance is in the Ok state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TOk, TState> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Map(ev => ev.Fold(state, right: ok)).GetValueOrDefault(state);
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/>
        /// with the Err value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="err">
        /// A function to invoke asynchronously with the seed value and the Err value of this instance
        /// as the arguments if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Err value of this instance
        /// if this instance is in the Err state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        public Task<TState> FoldAsync<TState>([NotNull, InstantHandle] Func<TState, TErr, Task<TState>> err)
            where TState : struct
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.MapAsync(ev => ev.FoldAsync(left: err)).Map(ov => ov.GetValueOrDefault(default(TState)));
        }

        /// <summary>Combines the provided seed state with the Err value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Err value of this instance.</param>
        /// <param name="err">
        /// A function to invoke asynchronously with the seed value and the Err value of this instance
        /// as the arguments if this instance is in the Err state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Err value of this instance
        /// if this instance is in the Err state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<TState> FoldAsync<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TErr, Task<TState>> err)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.MapAsync(ev => ev.FoldAsync(state, left: err)).Map(ov => ov.GetValueOrDefault(state));
        }

        /// <summary>
        /// Combines the default value of <typeparamref name="TState"/>
        /// with the Ok value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="ok">
        /// A function to invoke asynchronously with the seed value and the Ok value of this instance
        /// as the arguments if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Ok state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<TState> FoldAsync<TState>([NotNull, InstantHandle] Func<TState, TOk, Task<TState>> ok)
            where TState : struct
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.FoldAsync(right: ok)).Map(ov => ov.GetValueOrDefault(default(TState)));
        }

        /// <summary>Combines the provided seed state with the Ok value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Ok value of this instance.</param>
        /// <param name="ok">
        /// A function to invoke asynchronously with the seed value and the Ok value of this instance
        /// as the arguments if this instance is in the Ok state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Ok value of this instance
        /// if this instance is in the Ok state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<TState> FoldAsync<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TOk, Task<TState>> ok)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.MapAsync(ev => ev.FoldAsync(state, right: ok)).Map(ov => ov.GetValueOrDefault(state));
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action if this instance is in the None state and returns this instance.
        /// </summary>
        /// <param name="none">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap([NotNull, InstantHandle] Action none)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }

            _value.Tap(none: none);
            return this;
        }

        /// <summary>
        /// Performs an action if this instance is in the None state and returns this instance, asynchronously.
        /// </summary>
        /// <param name="none">An action to perform asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync([NotNull, InstantHandle] Func<Task> none)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }

            await _value.TapAsync(none: none).ConfigureAwait(false);
            return this;
        }

        /// <summary>
        /// Performs an action on the Err value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap([NotNull, InstantHandle] Action<TErr> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            _value.Tap(ev => ev.Tap(left: err));
            return this;
        }

        /// <summary>
        /// Performs an action on the Err value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        public async Task<Try<TErr, TOk>> TapAsync([NotNull, InstantHandle] Func<TErr, Task> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            await _value.TapAsync(ev => ev.TapAsync(left: err)).ConfigureAwait(false);
            return this;
        }

        /// <summary>
        /// Performs an action on the Ok value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap([NotNull, InstantHandle] Action<TOk> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            _value.Tap(ev => ev.Tap(right: ok));
            return this;
        }

        /// <summary>
        /// Performs an action on the Ok value of this instance,
        /// if present, and returns this instance, asynchronously.
        /// </summary>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public async Task<Try<TErr, TOk>> TapAsync([NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(ev => ev.TapAsync(right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            _value.Tap(none: none, some: ev => ev.Tap(left: err));
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            await _value.TapAsync(none: none, some: ev => ev.Tap(left: err)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            _value.Tap(none: none, some: ev => ev.Tap(right: ok));
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.Tap(right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance.</summary>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap(
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            _value.Tap(some: ev => ev.Tap(left: err, right: ok));
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Try<TErr, TOk> Tap(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            _value.Tap(none: none, some: ev => ev.Tap(left: err, right: ok));
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.Tap(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TErr> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Action<TOk> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform asynchronously if this instance is in the None state.</param>
        /// <param name="err">An action to perform asynchronously on the Err value of this instance.</param>
        /// <param name="ok">An action to perform asynchronously on the Ok value of this instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Try<TErr, TOk>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TErr, Task> err,
            [NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (err is null) { throw new ArgumentNullException(nameof(err)); }
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            await _value.TapAsync(none: none, some: ev => ev.TapAsync(left: err, right: ok)).ConfigureAwait(false);
            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Err value of this instance.</summary>
        /// <param name="err">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TErr> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.Let(ev => ev.Let(left: err));
        }

        /// <summary>Performs an action on the Ok value of this instance.</summary>
        /// <param name="ok">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TOk> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.Let(ev => ev.Let(right: ok));
        }

        /// <summary>Performs an action on the Err value of this instance, asynchronously.</summary>
        /// <param name="err">An action to perform asynchronously.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="err"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task LetAsync([NotNull, InstantHandle] Func<TErr, Task> err)
        {
            if (err is null) { throw new ArgumentNullException(nameof(err)); }

            return _value.LetAsync(ev => ev.LetAsync(left: err));
        }

        /// <summary>Performs an action on the Ok value of this instance, asynchronously.</summary>
        /// <param name="ok">An action to perform asynchronously.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ok"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Unit> LetAsync([NotNull, InstantHandle] Func<TOk, Task> ok)
        {
            if (ok is null) { throw new ArgumentNullException(nameof(ok)); }

            return _value.LetAsync(ev => ev.LetAsync(right: ok));
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in the case that this instance is not in the Ok state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the original Ok value
        /// of this instance if this instance is in the Ok state; otherwise, a <see cref="Try{TErr, TOk}"/>
        /// in the Ok state whose Ok value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Try<TErr, TOk> Recover([NotNull] TOk recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            return IsOk
                ? this
                : From(recoverer);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Ok state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the original Ok value
        /// of this instance if this instance is in the Ok state; otherwise, a <see cref="Try{TErr, TOk}"/>
        /// in the Ok state whose Ok value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Try<TErr, TOk> Recover([NotNull, InstantHandle] Func<TOk> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsOk) { return this; }

            var result = recoverer();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return From(result);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Ok state, asynchronously.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the original Ok value
        /// of this instance if this instance is in the Ok state; otherwise, a <see cref="Try{TErr, TOk}"/>
        /// in the Ok state whose Ok value is the result of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        [Pure, NotNull]
        public async Task<Try<TErr, TOk>> RecoverAsync([NotNull, InstantHandle] Func<Task<TOk>> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsOk) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return From(result);
        }

        #endregion

        #region Replace

        /// <summary>Replaces the Ok value of this instance with the provided value.</summary>
        /// <typeparam name="TOut">The type of the replacement value.</typeparam>
        /// <param name="replacement">The value to use as a replacement.</param>
        /// <returns>
        /// A <see cref="Try{TErr, TOk}"/> in the Ok state whose Ok value is the value
        /// of <paramref name="replacement"/> if this instance is in the Ok state,
        /// A <see cref="Try{TErr, TOk}"/> in the Err state whose Err value is the Err value
        /// of this instance if this instance is in te Err state,
        /// or a <see cref="Try{TErr, TOk}"/> in the None state if this instance is in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="replacement"/> is <see langword="null"/>.</exception>
        public Try<TErr, TOut> Replace<TOut>([NotNull] TOut replacement)
        {
            if (replacement == null) { throw new ArgumentNullException(nameof(replacement)); }

            return Map(ok: _ => replacement);
        }

        #endregion

        #region Value

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Ok state.
        /// </summary>
        /// <returns>
        /// The Ok value of this instance if this instance is in the Ok state;
        /// otherwise, the default value of <typeparamref name="TOk"/>.
        /// </returns>
        /// <remarks>
        /// <para>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TOk"/> satisfies <see langword="class"/>.</para>
        /// </remarks>
        [CanBeNull, Pure]
        public TOk GetValueOrDefault()
        {
            if (_value.IsNone) { return default; }
            if (_value.Value.IsLeft) { return default; }

            return Value;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Ok state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Ok value of this instance if this instance is in the Ok state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TOk GetValueOrDefault([NotNull] TOk other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return _value.Map(ev => ev.GetValueOrDefault(other)).GetValueOrDefault(other);
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Ok state.
        /// </summary>
        /// <param name="other">A function producing an alternative value.</param>
        /// <returns>
        /// The Ok value of this instance if this instance is in the Ok state;
        /// otherwise, the result of invoking <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TOk GetValueOrDefault([NotNull, InstantHandle] Func<TOk> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            return _value.Map(ev => ev.GetValueOrDefault(other)).GetValueOrDefault(other);
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Ok state, asynchronously.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Ok value of this instance if this instance is in the Ok state;
        /// otherwise, the result of invoking <paramref name="other"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public Task<TOk> GetValueOrDefaultAsync([NotNull, InstantHandle] Func<Task<TOk>> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            return _value.MapAsync(ev => ev.GetValueOrDefaultAsync(other)).Bind(ov => ov.GetValueOrDefaultAsync(other));
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Try{TErr, TOk}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Try{TErr, TOk}"/>.</returns>
        [NotNull, EditorBrowsable(Never)]
        public IEnumerator<TOk> GetEnumerator()
        { // note(cosborn) OK, it's kind of an implementation.
            if (IsOk)
            {
                yield return _value.Value.Value;
            }
        }

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => Match(
            none: "None",
            err: e => $"Err({e})",
            ok: v => $"Ok({v})");

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Try<TErr, TOk> @try && EqualsCore(@try);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>Compares this instance with another instance for equality.</summary>
        /// <param name="other">Another instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance and <paramref name="other"/> are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        internal bool EqualsCore(in Try<TErr, TOk> other) => _value.EqualsCore(other._value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            none: new { State = "None" },
            err: e => new { State = "Err", Value = e },
            ok: v => new { State = "Ok", Value = v });

        #endregion

        #endregion

        sealed class DebuggerTypeProxy
        {
            readonly Try<TErr, TOk> _value;

            /// <summary>Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.</summary>
            /// <param name="value">The try value to proxy.</param>
            public DebuggerTypeProxy(in Try<TErr, TOk> value)
            {
                _value = value;
            }

            /// <summary>Gets an internal value of the <see cref="Try{TErr, TOk}"/>.</summary>
            [CanBeNull]
            public TErr ErrValue => (TErr)_value._value.Value;

            /// <summary>Gets an internal value of the <see cref="Try{TErr, TOk}"/>.</summary>
            [CanBeNull]
            public TOk OkValue => _value.Value;

            /// <summary>Gets the internal state of the <see cref="Try{TErr, TOk}"/>.</summary>
            [NotNull]
            public string State => _value.Match(
                none: "None",
                err: _ => "Err",
                ok: _ => "Ok");
        }
    }
}
