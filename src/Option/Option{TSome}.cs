// <copyright file="Option{TSome}.cs" company="Cimpress, Inc.">
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
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Represents the presence or absence of a value.</summary>
    /// <typeparam name="TSome">The Some type of the value that may be represented.</typeparam>
    [TypeConverter(typeof(OptionTypeConverter))]
    [DebuggerTypeProxy(typeof(Option<>.DebuggerTypeProxy))]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct Option<TSome>
    {
        /// <summary>A value representing no value.</summary>
        public static readonly Option<TSome> None;

        readonly TSome _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Option{TSome}"/> struct
        /// from the provided <see cref="Option{TSome}"/> struct.
        /// </summary>
        /// <param name="optionValue">The value to copy.</param>
        public Option(in Option<TSome> optionValue) => this = optionValue;

        /// <summary>Initializes a new instance of the <see cref="Option{TSome}"/> struct.</summary>
        /// <param name="value">The value to wrap.</param>
        internal Option([NotNull] TSome value)
        {
            Assume(value != null, OptionConstructNull);

            _value = value;
            IsSome = true;
        }

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsNone => !IsSome;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsSome { get; }

        /// <summary>Gets the Some value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw if this instance is in the None state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is in an invalid state.
        /// </exception>
        [NotNull]
        public TSome Value
        {
            get
            {
                if (!IsSome) { throw new InvalidOperationException(OptionIsNone); }

                return _value;
            }
        }

        #region Operators

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        [EditorBrowsable(Never)]
        public bool IsTrue => IsSome;

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        [EditorBrowsable(Never)]
        public bool IsFalse => !IsSome;

        /// <summary>Wraps a value in <see cref="Option{TSome}"/>.</summary>
        /// <param name="someValue">The value to wrap.</param>
        public static implicit operator Option<TSome>([CanBeNull] TSome someValue) => ToOption(someValue);

        /// <summary>Unwraps the Some value of this instance.</summary>
        /// <param name="optionValue">The value to unwrap.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Type parameters play poorly with this analysis.")]
        public static explicit operator TSome(Option<TSome> optionValue) => optionValue.Value;

        /// <summary>
        /// Implicitly converts an <see cref="OptionNone"/> to an
        /// <see cref="Option{TSome}"/> in the None state.
        /// </summary>
        /// <param name="none">The default value of <see cref="OptionNone"/>.</param>
        [SuppressMessage("Roslynator", "RCS1163", Justification = "Used only for the type inference.")]
        public static implicit operator Option<TSome>(OptionNone none) => None;

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Option<TSome> left, Option<TSome> right) => left.EqualsCore(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Option<TSome> left, Option<TSome> right) => !(left == right);

        /// <summary>Performs logical disjunction between two objects of the same type.</summary>
        /// <param name="left">An object to disjoin with <paramref name="right"/>.</param>
        /// <param name="right">An object to disjoin with <paramref name="left"/>.</param>
        /// <returns>
        /// The first value of <paramref name="left"/> and <paramref name="right"/>
        /// that is in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        public static Option<TSome> operator |(Option<TSome> left, Option<TSome> right) =>
            left.BitwiseOr(right); // note(cosborn) Also implements || (LogicalOr) operator, see below.

        /// <summary>Performs logical conjunction between two objects of the same type.</summary>
        /// <param name="left">An object to conjoin with <paramref name="right"/>.</param>
        /// <param name="right">An object to conjoin with <paramref name="left"/>.</param>
        /// <returns>
        /// The last value of <paramref name="left"/> and <paramref name="right"/>
        /// if they are both in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        public static Option<TSome> operator &(Option<TSome> left, Option<TSome> right) =>
            left.BitwiseAnd(right); // note(cosborn) Also implements && (LogicalAnd) operator, see below.

        // note(cosborn) Implementing true and false operators allows || and && operators to short-circuit.

        /// <summary>Tests whether <paramref name="value"/> is in the Some state.</summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the Some state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator true(Option<TSome> value) => value.IsTrue;

        /// <summary>Tests whether <paramref name="value"/> is in the None state.</summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator false(Option<TSome> value) => value.IsFalse;

        /// <summary>
        /// Tests the logical inverse of whether <paramref name="value"/>
        /// is in the Some state.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !(Option<TSome> value) => value.LogicalNot();

        /// <summary>Wraps a value in <see cref="Option{TSome}"/>.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns><paramref name="value"/>, wrapped in <see cref="Option{TSome}"/>.</returns>
        public static Option<TSome> ToOption([CanBeNull] TSome value) => Option.From(value);

        /// <summary>
        /// Performs logical disjunction between this instance
        /// and another object of the same type.
        /// </summary>
        /// <param name="other">An object to disjoin with this instance.</param>
        /// <returns>
        /// The first value of this instance and <paramref name="other"/>
        /// that is in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public Option<TSome> BitwiseOr(Option<TSome> other) =>
            IsSome ? this : other; // note(cosborn) Yes, BitwiseOr is the alternate name for the operator.

        /// <summary>
        /// Performs logical conjunction between this instance
        /// and another object of the same type.
        /// </summary>
        /// <param name="other">An object to conjoin with this instance.</param>
        /// <returns>
        /// The last value of this instance and <paramref name="other"/>
        /// if they are both in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public Option<TSome> BitwiseAnd(Option<TSome> other) =>
            IsSome ? other : None; // note(cosborn) Yes, BitwiseAnd is the alternate name for the operator.

        /// <summary>Tests the logical inverse of whether this instance is in the Some state.</summary>
        /// <returns>
        /// <see langword="true"/> if this instance is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public bool LogicalNot() => !IsSome;

        #endregion

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="some">
        /// A function to invoke with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : none;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">
        /// A value to return if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to invoke asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? await some(_value).ConfigureAwait(false)
                : none;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">
        /// A function to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to invoke with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : none();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">
        /// A function to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to invoke asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? await some(_value).ConfigureAwait(false)
                : none();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">
        /// A function to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to invoke with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : await none().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to produce.</typeparam>
        /// <param name="none">
        /// A function to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to invoke asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> MatchAsync<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? await some(_value).ConfigureAwait(false)
                : await none().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Void

        /// <summary>Performs an action with this instance by matching on its state.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to invoke with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }
            else
            {
                none();
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to invoke asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }
            else
            {
                none();
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to invoke with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }
            else
            {
                await none().ConfigureAwait(false);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to invoke asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to invoke asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> MatchAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }
            else
            {
                await none().ConfigureAwait(false);
            }

            return Unit.Value;
        }

        #endregion

        #endregion

        #region Map

        /// <summary>Maps a function over the Some value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="some">
        /// A function to invoke with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value set to
        /// the value of invoking <paramref name="some"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TOut> Map<TOut>([NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                var result = some(_value);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Option<TOut>(result);
            }

            return Option<TOut>.None;
        }

        /// <summary>Maps a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="some">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value set to
        /// the value of invoking <paramref name="some"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public async Task<Option<TOut>> MapAsync<TOut>([NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                var result = await some(_value).ConfigureAwait(false);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Option<TOut>(result);
            }

            return Option<TOut>.None;
        }

        #endregion

        #region Bind

        /// <summary>Binds a function over the Some value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="some">
        /// A function to invoke with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="some"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{TSome}"/> in the Some state with its
        /// Some value set to the value of invoking <paramref name="some"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TOut> Bind<TOut>([NotNull, InstantHandle] Func<TSome, Option<TOut>> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return Map(some).Join();
        }

        /// <summary>Binds a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="some">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="some"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{TSome}"/> in the Some state with its
        /// Some value set to the value of invoking <paramref name="some"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public Task<Option<TOut>> BindAsync<TOut>([NotNull, InstantHandle] Func<TSome, Task<Option<TOut>>> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            return MapAsync(some).Map(v => v.Join());
        }

        #endregion

        #region Filter

        /// <summary>Filters the Some value of this instance based on a provided condition.</summary>
        /// <param name="predicate">
        /// A function to invoke with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state if this instance is in the Some state
        /// and the result of invoking <paramref name="predicate"/> over the Some value of this instance
        /// is <see langword="true"/>; otherwise, an <see cref="Option{TSome}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TSome> Filter([NotNull, InstantHandle] Func<TSome, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return IsSome
                ? predicate(_value) ? this : None
                : None;
        }

        /// <summary>
        /// Filters the Some value of this instance based on a provided condition, asynchronously.
        /// </summary>
        /// <param name="predicate">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state if this instance is in the Some state
        /// and the result of invoking <paramref name="predicate"/> over the Some value of this instance
        /// is <see langword="true"/>; otherwise, an <see cref="Option{TSome}"/> in the None state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public async Task<Option<TSome>> FilterAsync([NotNull, InstantHandle] Func<TSome, Task<bool>> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return IsSome
                ? await predicate(_value).ConfigureAwait(false) ? this : None
                : None;
        }

        #endregion

        #region Fold

        /// <summary>Combines the provided seed state with the Some value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the Some value of this instance.</param>
        /// <param name="folder">
        /// A function to invoke with the seed value and the Some value of this instance as the arguments
        /// if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Some value of this instance
        /// if this instance is in the Some state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TSome, TState> folder)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            var result = IsSome
                ? folder(state, _value)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Combines the provided seed state with the Some value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to combine with the some value of this instance.</param>
        /// <param name="folder">
        /// A function to invoke with the seed value and the Some value of this instance as the arguments
        /// if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Some value of this instance
        /// if this instance is in the Some state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TState> FoldAsync<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TSome, Task<TState>> folder)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder is null) { throw new ArgumentNullException(nameof(folder)); }

            var result = IsSome
                ? await folder(state, _value).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Tap

        /// <summary>Performs an action if this instance is in the None state and returns this instance.</summary>
        /// <param name="none">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Option<TSome> Tap([NotNull, InstantHandle] Action none)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }

            if (!IsSome)
            {
                none();
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Some value of this instance, if present, and returns this instance.
        /// </summary>
        /// <param name="some">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Option<TSome> Tap([NotNull, InstantHandle] Action<TSome> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }

            return this;
        }

        /// <summary>
        /// Performs an action if this instance is in the None state and returns this instance, asynchronously.
        /// </summary>
        /// <param name="none">An action to perform asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Option<TSome>> TapAsync([NotNull, InstantHandle] Func<Task> none)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }

            if (!IsSome)
            {
                await none().ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Some value of this instance,
        /// if present, and returns the same value as this instance, asynchronously.
        /// </summary>
        /// <param name="some">An action to perform, asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Option<TSome>> TapAsync([NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>Performs an action based on the state of this instance.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="some">An action to perform if this instance is in the Some state.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public Option<TSome> Tap(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }
            else
            {
                none();
            }

            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state, asynchonously.</param>
        /// <param name="some">An action to perform if this instance is in the Some state.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public async Task<Option<TSome>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }
            else
            {
                await none().ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state.</param>
        /// <param name="some">An action to perform if this instance is in the Some state, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public async Task<Option<TSome>> TapAsync(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }
            else
            {
                none();
            }

            return this;
        }

        /// <summary>Performs an action based on the state of this instance, asynchronously.</summary>
        /// <param name="none">An action to perform if this instance is in the None state, asynchronously.</param>
        /// <param name="some">An action to perform if this instance is in the Some state, asynchronously.</param>
        /// <returns>A task which, when resolved, produces this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public async Task<Option<TSome>> TapAsync(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none is null) { throw new ArgumentNullException(nameof(none)); }
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }
            else
            {
                await none().ConfigureAwait(false);
            }

            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Some value of this instance.</summary>
        /// <param name="some">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TSome> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Some value of this instance, asynchronously.</summary>
        /// <param name="some">An action to perform asynchronously.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Unit> LetAsync([NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (some is null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }

            return Unit.Value;
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in the case that this instance is in the None state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> in the
        /// Some state state whose Some value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TSome> Recover([NotNull] TSome recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            return IsSome
                ? this
                : new Option<TSome>(recoverer);
        }

        /// <summary>Provides an alternate value in the case that this instance is in the None state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> in the
        /// Some state whose Some value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TSome> Recover([NotNull, InstantHandle] Func<TSome> recoverer)
        {
            if (recoverer is null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsSome) { return this; }

            var result = recoverer();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Option<TSome>(result);
        }

        /// <summary>Provides an alternate value in the case that this instance is in the None state.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> in the
        /// Some state whose Some value is the result of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public async Task<Option<TSome>> RecoverAsync([NotNull] Func<Task<TSome>> recoverer)
        {
            if (recoverer is null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsSome) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Option<TSome>(result);
        }

        #endregion

        #region Replace

        /// <summary>Replaces the Some value of this instance with the provided value.</summary>
        /// <typeparam name="TOut">The type of the replacement value.</typeparam>
        /// <param name="replacement">The value to use as a replacement.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value set to
        /// the value of <paramref name="replacement"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="replacement"/> is <see langword="null"/>.</exception>
        public Option<TOut> Replace<TOut>([NotNull] TOut replacement)
        {
            if (replacement == null) { throw new ArgumentNullException(nameof(replacement)); }

            return IsSome
                ? Option.From(replacement)
                : Option<TOut>.None;
        }

        #endregion

        #region Value

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, the default value of <typeparamref name="TSome"/>.
        /// </returns>
        /// <remarks>
        /// <para>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TSome"/> satisfies <see langword="class"/>.</para>
        /// </remarks>
        [CanBeNull, Pure]
        public TSome GetValueOrDefault() => _value;

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TSome GetValueOrDefault([NotNull] TSome other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return IsSome
                ? _value
                : other;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">A function producing an alternative value.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, the result of invoking <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TSome GetValueOrDefault([NotNull, InstantHandle] Func<TSome> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsSome
                ? _value
                : other();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Unwraps this instance asynchronously with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, the result of invoking <paramref name="other"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public async Task<TSome> GetValueOrDefaultAsync([NotNull, InstantHandle] Func<Task<TSome>> other)
        {
            if (other is null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsSome
                ? _value
                : await other().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        /// <summary>Returns an enumerator that iterates through the <see cref="Option{TSome}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Option{TSome}"/>.</returns>
        [Pure, EditorBrowsable(Never)]
        public IEnumerator<TSome> GetEnumerator()
        {
            if (IsSome)
            {
                yield return _value;
            }
        }

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => IsNone
            ? "None"
            : $"Some({_value})";

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => IsNone
            ? 0
            : EqualityComparer<TSome>.Default.GetHashCode(_value);

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Option<TSome> option && EqualsCore(option);

        /// <summary>Compares this instance with another instance for equality.</summary>
        /// <param name="other">Another instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance and <paramref name="other"/> are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        internal bool EqualsCore(in Option<TSome> other)
        {
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return _value.Equals(other._value);
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            none: new { State = "None" },
            some: v => new { State = "Some", Value = v });

        #endregion

        #endregion

        [UsedImplicitly(WithMembers)]
        sealed class DebuggerTypeProxy
        {
            readonly Option<TSome> _value;

            /// <summary>Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.</summary>
            /// <param name="value">The optional value to proxy.</param>
            public DebuggerTypeProxy(in Option<TSome> value)
            {
                _value = value;
            }

            /// <summary>Gets the internal value of the <see cref="Option{TSome}"/>.</summary>
            [NotNull]
            public TSome Value => _value._value;

            /// <summary>Gets the internal state of the <see cref="Option{TSome}"/>.</summary>
            [NotNull]
            public string State => _value.IsSome ? "Some" : "None";
        }
    }
}
