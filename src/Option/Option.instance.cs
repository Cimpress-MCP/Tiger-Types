using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Types
{
    /// <summary>Represents the presence or absence of a value.</summary>
    /// <typeparam name="T">The type of the value that may be represented.</typeparam>
    [PublicAPI]
    public struct Option<T>
    {
        /// <summary>A value representing no value.</summary>
        public static Option<T> None = default(Option<T>);

        /// <summary>Creates an <see cref="Option{T}"/> from the provided value.</summary>
        /// <param name="value"></param>
        /// <returns>
        /// An <see cref="Option{T}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{T}"/>
        /// in the Some state.
        /// </returns>
        public static Option<T> From(T value)
        {
            return new Option<T>(value);
        }

        enum OptionState
        {
            None = 0, // note(cosborn) None must be the 0 value in case of default(Option<T>).
            Some
        }

        /// <summary>Gets a value indicating whether this value is in the Some state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsSome => _state == OptionState.Some;

        /// <summary>Gets a value indicating whether this value is in the None state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsNone => _state == OptionState.None;

        readonly OptionState _state;
        readonly T _value;

        Option(T value)
        {
            if (ReferenceEquals(value, null))
            {
                _value = default(T); // note(cosborn) This value is unimportant; the state ensures it's never read.
                _state = OptionState.None;
            }
            else
            {
                _value = value;
                _state = OptionState.Some;
            }
        }

        #region Match

        #region Value

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
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<T, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return IsNone
                ? await none().ConfigureAwait(false)
                : await some(_value).ConfigureAwait(false);
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
        public void Match([NotNull] Action none, [NotNull] Action<T> some)
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
        public async Task Match([NotNull] Action none, [NotNull] Func<T, Task> some)
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
        public async Task Match([NotNull] Func<Task> none, [NotNull] Action<T> some)
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
        public async Task Match([NotNull] Func<Task> none, [NotNull] Func<T, Task> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsNone)
            {
                await none().ConfigureAwait(false);
            }
            else
            {
                await some(_value).ConfigureAwait(false);
            }
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
                none: () => Option<TOut>.None,
                some: v => mapper(v).Pipe(Option.From));
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
                none: () => Option<TOut>.None,
                some: v => mapper(v).Map(Option.From));
        }

        #endregion
    }
}
