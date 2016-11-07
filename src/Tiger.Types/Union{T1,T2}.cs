using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LINQPad;

namespace Tiger.Types
{
    /// <summary>
    /// Represents a value that is a composite of two values.
    /// </summary>
    /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
    /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
    [DebuggerTypeProxy(typeof(UnionDebuggerTypeProxy<,>))]
    public class Union<T1, T2>
        : ICustomMemberProvider
    {
        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Union{T1,T2}"/> in the first state.</returns>
        [NotNull]
        public static Union<T1, T2> From(T1 value) => new Union<T1, T2>(value);

        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Union{T1,T2}"/> in the second state.</returns>
        [NotNull]
        public static Union<T1, T2> From(T2 value) => new Union<T1, T2>(value);

        /// <summary>Gets a value indicating whether this instance is in the first state.</summary>
        public bool IsState1 => State == 1;

        /// <summary>Gets a value indicating whether this instance is in the second state.</summary>
        public bool IsState2 => State == 2;

        /// <summary>Gets the internal state of this instance.</summary>
        internal int State { get; }

        /// <summary>Gets the first value of this instance.</summary>
        /// <remarks>This property is unsafe, as it can throw
        /// if this instance is not in the first state.</remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is not in the specified state.
        /// </exception>
        public T1 Value1
        {
            get
            {
                if (!IsState1) { throw new InvalidOperationException(Resources.UnionDoesNotMatch); }

                return _value1;
            }
        }

        /// <summary>Gets the second value of this instance.</summary>
        /// <remarks>This property is unsafe, as it can throw
        /// if this instance is not in the second state.</remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is not in the specified state.
        /// </exception>
        public T2 Value2
        {
            get
            {
                if (!IsState2) { throw new InvalidOperationException(Resources.UnionDoesNotMatch); }

                return _value2;
            }
        }

        readonly T1 _value1;
        readonly T2 _value2;

        Union(T1 value)
        {
            _value1 = value;
            State = 1;
        }

        Union(T2 value)
        {
            _value2 = value;
            State = 2;
        }

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

            switch (State)
            {
                case 1:
                    return one(_value1);
                case 2:
                    return two(_value2);
                default: // because(cosborn) Hush, ReSharper.
                    return default(TOut);
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

            switch (State)
            {
                case 1:
                    return await one(_value1).ConfigureAwait(false);
                case 2:
                    return two(_value2);
                default: // because(cosborn) Hush, ReSharper.
                    return default(TOut);
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

            switch (State)
            {
                case 1:
                    return one(_value1);
                case 2:
                    return await two(_value2).ConfigureAwait(false);
                default: // because(cosborn) Hush, ReSharper.
                    return default(TOut);
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

            switch (State)
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
        /// <exception cref="ArgumentNullException"><paramref name="one"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="two"/> is <see langword="null"/>.</exception>
        public void Match(
            [NotNull, InstantHandle] Action<T1> one,
            [NotNull, InstantHandle] Action<T2> two)
        {
            if (one == null) { throw new ArgumentNullException(nameof(one)); }
            if (two == null) { throw new ArgumentNullException(nameof(two)); }

            switch (State)
            {
                case 1:
                    one(_value1);
                    return;
                case 2:
                    two(_value2);
                    return;
            }
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

            switch (State)
            {
                case 1:
                    await one(_value1);
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

            switch (State)
            {
                case 1:
                    one(_value1);
                    return;
                case 2:
                    await two(_value2);
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

            switch (State)
            {
                case 1:
                    await one(_value1);
                    return;
                case 2:
                    await two(_value2);
                    return;
            }
        }

        #endregion

        #endregion

        #region Overrides

        #region object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() => Match(
            one: o => string.Format(CultureInfo.InvariantCulture, "One({0})", o),
            two: t => string.Format(CultureInfo.InvariantCulture, "Two({0})", t)) ?? string.Empty;

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => Match(
            one: o => o.GetHashCode(),
            two: t => t.GetHashCode());

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <see langword="true"/> if the specified object  is equal to the current object;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var union = obj as Union<T1, T2>;
            return !ReferenceEquals(union, null) && EqualsCore(union);
        }

        bool EqualsCore([NotNull] Union<T1, T2> other)
        {
            if (State != other.State) { return false; }

            switch (State)
            {
                case 1:
                    return _value1.Equals(other._value1);
                case 2:
                    return _value2.Equals(other._value2);
                default: // because(cosborn) Hush, ReSharper.
                    return false;
            }
        }

        #endregion

        #endregion

        #region Implementations

        /// <inheritdoc />
        IEnumerable<string> ICustomMemberProvider.GetNames()
        {
            yield return string.Empty;
        }

        /// <inheritdoc />
        IEnumerable<Type> ICustomMemberProvider.GetTypes()
        {
            yield return typeof(string);
        }

        /// <inheritdoc />
        IEnumerable<object> ICustomMemberProvider.GetValues()
        {
            yield return ToString();
        }

        #endregion

        #region Operators and Named Alternatives

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Union<T1, T2> left, Union<T1, T2> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.EqualsCore(right);
        }

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Union<T1, T2> left, Union<T1, T2> right) =>
            !(left == right);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        public static implicit operator Union<T1, T2>(T1 value) => value == null
            ? null
            : new Union<T1, T2>(value);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        public static implicit operator Union<T1, T2>(T2 value) => value == null
            ? null
            : new Union<T1, T2>(value);

        /// <summary>Unwraps the first value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        public static explicit operator T1(Union<T1, T2> value) => value.Value1;

        /// <summary>Unwraps the second value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        public static explicit operator T2(Union<T1, T2> value) => value.Value2;

        #endregion
    }
}
