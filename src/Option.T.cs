using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Represents the presence or absence of a value.</summary>
    /// <typeparam name="T">The Some type of the value that may be represented.</typeparam>
    [PublicAPI]
    public struct Option<T>
        : IEquatable<Option<T>>
    {
        /// <summary>A value representing no value.</summary>
        public static readonly Option<T> None = default(Option<T>);

        /// <summary>Creates an <see cref="Option{T}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{T}"/>
        /// in the Some state.
        /// </returns>
        /// <remarks>
        /// Passing a nullable struct into this method is likely to confuse
        /// both the type system and the programmer.
        /// </remarks>
        public static Option<T> From([CanBeNull] T value) => new Option<T>(value);

        enum OptionState
            : byte // todo(cosborn) Does this save anything?
        {
            None, // note(cosborn) None must be the 0 value in case of default(Option<T>).
            Some
        }

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsNone => _state == OptionState.None;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsSome => _state == OptionState.Some;

        readonly OptionState _state;
        readonly T _value;

        Option([CanBeNull] T value)
            : this()
        {
            if (value == null) { return; } // note(cosborn) Defaults all fields.

            _value = value;
            _state = OptionState.Some;
        }

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A value to return if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<T, TOut> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none
                : some(_value);
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A value to return if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<T, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none
                : await some(_value).ConfigureAwait(false);
        }

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A function to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<T, TOut> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none()
                : some(_value);
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A function to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<T, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none()
                : await some(_value).ConfigureAwait(false);
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A function to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<T, TOut> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? await none().ConfigureAwait(false)
                : some(_value);
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">
        /// A function to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// A function to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<T, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none()
                : some(_value);
        }

        #endregion

        #region Void

        /// <summary>Performs an action on this instance by matching on its state.</summary>
        /// <param name="none">
        /// An action to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public void Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<T> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsNone)
            {
                none();
            }
            else
            {
                some(_value);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<T, Task> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsNone)
            {
                none();
            }
            else
            {
                await some(_value).ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<T> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsNone)
            {
                await none().ConfigureAwait(false);
            }
            else
            {
                some(_value);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task Match(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<T, Task> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? none()
                : some(_value);
        }

        #endregion

        #endregion

        #region Map

        /// <summary>Maps a function over the Some value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="mapper">
        /// A function to invoke with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{T}"/> in the Some state with its Some value set to
        /// the value of applying <paramref name="mapper"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        public Option<TOut> Map<TOut>([NotNull, InstantHandle] Func<T, TOut> mapper)
        {
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return Match(
                none: Option<TOut>.None,
                some: v => mapper(v));
        }

        /// <summary>Maps a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="mapper">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{T}"/> in the Some state with its Some value set to
        /// the value of applying <paramref name="mapper"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull]
        public Task<Option<TOut>> Map<TOut>([NotNull, InstantHandle] Func<T, Task<TOut>> mapper)
        {
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return Match(
                none: Option<TOut>.None,
                some: v => mapper(v).Map(Option.From));
        }

        #endregion

        #region Bind

        /// <summary>Binds a function over the Some value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="binder">
        /// A function to invoke with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if this instance is in the None state
        /// or if the result of applying <paramref name="binder"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{T}"/> in the Some state with its
        /// Some value set to the value of applying <paramref name="binder"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        public Option<TOut> Bind<TOut>([NotNull, InstantHandle] Func<T, Option<TOut>> binder)
        {
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            return Match(
                none: Option<TOut>.None,
                some: binder);
        }

        /// <summary>Binds a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="binder">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if this instance is in the None state
        /// or if the result of applying <paramref name="binder"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{T}"/> in the Some state with its
        /// Some value set to the value of applying <paramref name="binder"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        public Task<Option<TOut>> Bind<TOut>([NotNull, InstantHandle] Func<T, Task<Option<TOut>>> binder)
        {
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            return Match(
                none: Option<TOut>.None,
                some: binder);
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action on the Some value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="tapper">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        public Option<T> Tap([NotNull] Action<T> tapper)
        {
            if (tapper == null) { throw new ArgumentNullException(nameof(tapper)); }

            IfSome(tapper);
            return this;
        }

        /// <summary>
        /// Performs an action on the Some value of this instance asynchronously,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="tapper">An action to perform asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        public async Task<Option<T>> Tap([NotNull] Func<T, Task> tapper)
        {
            if (tapper == null) { throw new ArgumentNullException(nameof(tapper)); }

            await IfSome(tapper).ConfigureAwait(false);
            return this;
        }

        #endregion

        #region Other Useful Methods

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <remarks>
        /// This is very similar to the null-coalescence operator (??)
        /// or <see cref="Nullable{T}.GetValueOrDefault()"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull]
        public T IfNone([NotNull] T other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return Match(
                none: other,
                some: v => v);
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">A function producing an alternative value.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <remarks>
        /// This is very similar to the null-coalescence operator (??)
        /// or <see cref="Nullable{T}.GetValueOrDefault()"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull]
        public T IfNone([NotNull, InstantHandle] Func<T> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return Match(
                none: other,
                some: v => v);
        }

        /// <summary>
        /// Unwraps this instance asynchronously with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <remarks>
        /// This is very similar to the null-coalescence operator (??)
        /// or <see cref="Nullable{T}.GetValueOrDefault()"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        public Task<T> IfNone([NotNull, InstantHandle] Func<Task<T>> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return Match(
                none: other,
                some: v => v);
        }

        /// <summary>Performs an action on the Some value of this instance.</summary>
        /// <param name="action">An action to perform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public void IfSome([NotNull, InstantHandle] Action<T> action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            if (IsSome)
            {
                action(_value);
            }
        }

        /// <summary>Performs an action on the Some value of this instance, asynchronously.</summary>
        /// <param name="action">An action to perform asynchronously.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task IfSome([NotNull, InstantHandle] Func<T, Task> action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            if (IsSome)
            {
                await action(_value).ConfigureAwait(false);
            }
        }

        #endregion

        #region Overrides

        /// <summary>Converts this instance to a string.</summary>
        /// <returns>A <see cref="string"/> containing the value of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() =>
            Map(v => string.Format(CultureInfo.InvariantCulture, "Some({0})", v)).IfNone("None");

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance
        /// are the same type and represent the same value; otherwise, <see langword="false"/>. 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj) => obj is Option<T> && Equals((Option<T>)obj);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => Map(v => v.GetHashCode()).IfNone(0);

        #endregion

        #region Implementations

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Option<T> other)
        { // note(cosborn) Eh, this gets gnarly using Match.
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        #endregion

        #region Operators and Named Alternates

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

        /// <summary>Performs logical disjunction between two objects of the same type.</summary>
        /// <param name="left">An object to disjoin with <paramref name="right"/>.</param>
        /// <param name="right">An object to disjoin with <paramref name="left"/>.</param>
        /// <returns>
        /// The first value of <paramref name="left"/> and <paramref name="right"/>
        /// that is in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        // note(cosborn) Also implements || (LogicalOr) operator, see below.
        public static Option<T> operator |(Option<T> left, Option<T> right) => left.BitwiseOr(right);

        /// <summary>
        /// Performs logical disjunction between this instance
        /// and another object of the same type.
        /// </summary>
        /// <param name="other">An object to disjoin with this instance.</param>
        /// <returns>
        /// The first value of this instance and <paramref name="other"/>
        /// that is in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        // note(cosborn) Yes, BitwiseOr is the alternate name for the operator.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Option<T> BitwiseOr(Option<T> other) => Match(
            none: other,
            some: v => v);

        /// <summary>Performs logical conjunction between two objects of the same type.</summary>
        /// <param name="left">An object to conjoin with <paramref name="right"/>.</param>
        /// <param name="right">An object to conjoin with <paramref name="left"/>.</param>
        /// <returns>
        /// The last value of <paramref name="left"/> and <paramref name="right"/>
        /// if they are both in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        // note(cosborn) Also implements && (LogicalAnd) operator, see below.
        public static Option<T> operator &(Option<T> left, Option<T> right) => left.BitwiseAnd(right);

        /// <summary>
        /// Performs logical conjunction between this instance
        /// and another object of the same type.
        /// </summary>
        /// <param name="other">An object to conjoin with this instance.</param>
        /// <returns>
        /// The last value of this instance and <paramref name="other"/>
        /// if they are both in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        // note(cosborn) Yes, BitwiseAnd is the alternate name for the operator.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Option<T> BitwiseAnd(Option<T> other) => Match(
            none: None,
            some: _ => other);

        // note(cosborn) Implementing true and false operators allows || and && operators to short-circuit.

        /// <summary>Tests whether <paramref name="value"/> is in the Some state.</summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the Some state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator true(Option<T> value) => value.IsTrue;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsTrue => IsSome;

        /// <summary>Tests whether <paramref name="value"/> is in the None state.</summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator false(Option<T> value) => value.IsFalse;

        /// <summary>Gets a value indicating whether the current object is in the None state.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsFalse => IsNone;

        /// <summary>Wraps a value in <see cref="Option{T}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        // note(cosborn) Can be [NotNull] because C# checks for value/reference before implicit conversions.
        public static implicit operator Option<T>([NotNull] T value) => Option.From(value);

        /// <summary>
        /// Implicitly converts a <see cref="OptionNone"/> to an
        /// <see cref="Option{T}"/> in the None state.
        /// </summary>
        /// <param name="none">The default value of <see cref="OptionNone"/>.</param>
        public static implicit operator Option<T>(OptionNone none) => None;

        #endregion
    }
}
