using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Represents the presence or absence of a value.</summary>
    /// <typeparam name="TSome">The Some type of the value that may be represented.</typeparam>
    [SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "R# doesn't understand Code Contracts.")]
    public struct Option<TSome>
        : IEquatable<Option<TSome>>
    {
        /// <summary>A value representing no value.</summary>
        public static readonly Option<TSome> None = default(Option<TSome>);

        /// <summary>Creates an <see cref="Option{TSome}"/> from the provided value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if <paramref name="value"/>
        /// is equal to <see langword="null"/>; otherwise, an <see cref="Option{TSome}"/>
        /// in the Some state.
        /// </returns>
        /// <remarks>Passing a nullable struct into this method is likely to confuse
        /// both the type system and the programmer.</remarks>
        [Pure]
        public static Option<TSome> From([CanBeNull] TSome value) => new Option<TSome>(value);

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsNone => _state == OptionState.None;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsSome => _state == OptionState.Some;

        readonly OptionState _state;
        readonly TSome _someValue;

        internal Option([CanBeNull] TSome someValue)
            : this()
        {
            if (someValue == null) { return; } // note(cosborn) Defaults all fields.

            _someValue = someValue;
            _state = OptionState.Some;
        }

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="none">A value to return if this instance is in the None state.</param>
        /// <param name="some">
        /// A function to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A value produced by <paramref name="none"/> or <paramref name="some"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);

            var result = IsNone
                ? none
                : some(_someValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] TOut none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsNone
                ? none
                : await some(_someValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);

            var result = IsNone
                ? none()
                : some(_someValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsNone
                ? none()
                : await some(_someValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsNone
                ? await none().ConfigureAwait(false)
                : some(_someValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsNone
                ? await none().ConfigureAwait(false)
                : await some(_someValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
            [NotNull, InstantHandle] Action<TSome> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);

            if (IsNone)
            {
                none();
            }
            else
            {
                some(_someValue);
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
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<Task>() != null);

            if (IsNone)
            {
                none();
            }
            else
            {
                await some(_someValue).ConfigureAwait(false);
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
            [NotNull, InstantHandle] Action<TSome> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<Task>() != null);

            if (IsNone)
            {
                await none().ConfigureAwait(false);
            }
            else
            {
                some(_someValue);
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
        public async Task Match(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            Requires<ArgumentNullException>(none != null);
            Requires<ArgumentNullException>(some != null);
            Ensures(Result<Task>() != null);

            if (IsNone)
            {
                await none().ConfigureAwait(false);
            }
            else
            {
                await some(_someValue).ConfigureAwait(false);
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
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value set to
        /// the value of invoking <paramref name="mapper"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TOut> Map<TOut>([NotNull, InstantHandle] Func<TSome, TOut> mapper)
        {
            Requires<ArgumentNullException>(mapper != null);

            if (IsNone) { return Option<TOut>.None; }
            
            var result = mapper(_someValue);
            Assume(result != null, Resources.ResultIsNull);
            return new Option<TOut>(result);
        }

        /// <summary>Maps a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="mapper">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state;
        /// otherwise, an <see cref="Option{TSome}"/> in the Some state with its Some value set to
        /// the value of invoking <paramref name="mapper"/> over the Some value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public async Task<Option<TOut>> Map<TOut>([NotNull, InstantHandle] Func<TSome, Task<TOut>> mapper)
        {
            Requires<ArgumentNullException>(mapper != null);
            Ensures(Result<Task<Option<TOut>>>() != null);

            if (IsNone) { return Option.None; }

            var result = await mapper(_someValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return new Option<TOut>(result);
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
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="binder"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{TSome}"/> in the Some state with its
        /// Some value set to the value of invoking <paramref name="binder"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [Pure]
        public Option<TOut> Bind<TOut>([NotNull, InstantHandle] Func<TSome, Option<TOut>> binder)
        {
            Requires<ArgumentNullException>(binder != null);

            return IsNone
                ? Option<TOut>.None
                : binder(_someValue);
        }

        /// <summary>Binds a function over the Some value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="binder">
        /// A function to invoke asynchronously with the Some value of this instance
        /// as the argument if this instance is in the Some state.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the None state if this instance is in the None state
        /// or if the result of invoking <paramref name="binder"/> over the Some value of this instance
        /// is in the None state; otherwise, an <see cref="Option{TSome}"/> in the Some state with its
        /// Some value set to the value of invoking <paramref name="binder"/> over the Some value
        /// of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public async Task<Option<TOut>> Bind<TOut>([NotNull, InstantHandle] Func<TSome, Task<Option<TOut>>> binder)
        {
            Requires<ArgumentNullException>(binder != null);
            Ensures(Result<Task<Option<TOut>>>() != null);

            return IsNone
                ? Option<TOut>.None
                : await binder(_someValue).ConfigureAwait(false);
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
            Requires<ArgumentNullException>(predicate != null);

            return IsNone
                ? None
                : predicate(_someValue) ? this : None;
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
        public async Task<Option<TSome>> Filter([NotNull, InstantHandle] Func<TSome, Task<bool>> predicate)
        {
            Requires<ArgumentNullException>(predicate != null);
            Ensures(Result<Task<Option<TSome>>>() != null);

            return IsNone
                ? None
                : await predicate(_someValue).ConfigureAwait(false) ? this : None;
        }

        #endregion

        #region Fold

        /// <summary>Combines the provided seed state with the Some value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the some value of this instance.</param>
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
            Requires<ArgumentNullException>(state != null);
            Requires<ArgumentNullException>(folder != null);
            Ensures(Result<TState>() != null);

            var result = IsNone
                ? state
                : folder(state, _someValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        /// <summary>
        /// Combines the provided seed state with the Some value of this instance, asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the some value of this instance.</param>
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
        public async Task<TState> Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TSome, Task<TState>> folder)
        {
            Requires<ArgumentNullException>(state != null);
            Requires<ArgumentNullException>(folder != null);
            Ensures(Result<Task<TState>>() != null);

            var result = IsNone
                ? state
                : await folder(state, _someValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        public Option<TSome> Tap([NotNull] Action<TSome> tapper)
        {
            Requires<ArgumentNullException>(tapper != null);

            if (IsSome)
            {
                tapper(_someValue);
            }
            return this;
        }

        /// <summary>
        /// Performs an action on the Some value of this instance asynchronously,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="tapper">An action to perform asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task<Option<TSome>> Tap([NotNull] Func<TSome, Task> tapper)
        {
            Requires<ArgumentNullException>(tapper != null);
            Ensures(Result<Task<Option<TSome>>>() != null);

            if (IsSome)
            {
                await tapper(_someValue).ConfigureAwait(false);
            }
            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Some value of this instance.</summary>
        /// <param name="action">An action to perform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public void Let([NotNull, InstantHandle] Action<TSome> action)
        {
            Requires<ArgumentNullException>(action != null);

            if (IsSome)
            {
                action(_someValue);
            }
        }

        /// <summary>Performs an action on the Some value of this instance, asynchronously.</summary>
        /// <param name="action">An action to perform asynchronously.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Let([NotNull, InstantHandle] Func<TSome, Task> action)
        {
            Requires<ArgumentNullException>(action != null);
            Ensures(Result<Task>() != null);

            if (IsSome)
            {
                await action(_someValue);
            }
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in that case that this instance is in the None state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> state whose
        /// Some value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        public Option<TSome> Recover([NotNull] TSome recoverer)
        {
            Requires<ArgumentNullException>(recoverer != null);

            return IsNone
                ? new Option<TSome>(recoverer)
                : this;
        }

        /// <summary>Provides an alternate value in that case that this instance is in the None state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> state whose
        /// Some value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        public Option<TSome> Recover([NotNull, InstantHandle] Func<TSome> recoverer)
        {
            Requires<ArgumentNullException>(recoverer != null);

            if (IsNone)
            {
                var result = recoverer();
                Assume(result != null, Resources.ResultIsNull);
                return new Option<TSome>(result);
            }

            return this;
        }

        /// <summary>Provides an alternate value in that case that this instance is in the None state.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state whose Some value is the original Some value
        /// if this instance is in the Some state; otherwise, an <see cref="Option{TSome}"/> state whose
        /// Some value is the result of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        public async Task<Option<TSome>> Recover([NotNull] Func<Task<TSome>> recoverer)
        {
            Requires<ArgumentNullException>(recoverer != null);
            Ensures(Result<Task<Option<TSome>>>() != null);

            if (IsNone)
            {
                var result = await recoverer().ConfigureAwait(false);
                Assume(result != null, Resources.ResultIsNull);
                return new Option<TSome>(result);
            }

            return this;
        }

        #endregion

        #region Value

        /// <summary>Gets the Some value of this instance.</summary>
        /// <remarks>This property is unsafe, as it can throw if this instance is in the None state.</remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is in an invalid state.
        /// </exception>
        public TSome Value
        {
            get
            {
                Requires<InvalidOperationException>(IsSome, Resources.OptionIsNone);
                Ensures(Result<TSome>() != null);

                // note(cosborn) Invariants don't understand sum types; can't link `IsSome` and `_somevalue != null`.
                Assume(_someValue != null);
                return _someValue;
            }
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is in the None state.
        /// </summary>
        /// <returns>
        /// The Some value of this instance if this instance is in the Some state;
        /// otherwise, the default value of <typeparamref name="TSome"/>.
        /// </returns>
        /// <remarks>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TSome"/> satisfies <see langword="class"/>.</remarks>
        [CanBeNull, Pure]
        public TSome GetValueOrDefault() => IsNone
            ? default(TSome)
            : _someValue;

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
            Requires<ArgumentNullException>(other != null);
            Ensures(Result<TSome>() != null);

            if (IsNone) { return other; }

            // note(cosborn) Invariants don't understand sum types; can't link `!IsNone` and `_somevalue != null`.
            Assume(_someValue != null);
            return _someValue;
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
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TSome GetValueOrDefault([NotNull, InstantHandle] Func<TSome> other)
        {
            Requires<ArgumentNullException>(other != null);
            Ensures(Result<TSome>() != null);

            var result = IsNone
                ? other()
                : _someValue;
            Assume(result != null, Resources.ResultIsNull);
            return result;
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
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public async Task<TSome> GetValueOrDefault([NotNull, InstantHandle] Func<Task<TSome>> other)
        {
            Requires<ArgumentNullException>(other != null);
            Ensures(Result<Task<TSome>>() != null);

            var result = IsNone
                ? await other().ConfigureAwait(false)
                : _someValue;
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        #endregion

        #region Overrides

        /// <summary>Converts this instance to a string.</summary>
        /// <returns>A <see cref="string"/> containing the value of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override string ToString() => IsNone
            ? "None"
            : string.Format(CultureInfo.InvariantCulture, "Some({0})", _someValue);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance
        /// are the same type and represent the same value; otherwise, <see langword="false"/>. 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override bool Equals(object obj) => obj is Option<TSome> && Equals((Option<TSome>)obj);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override int GetHashCode() => IsNone
            ? 0
            : _someValue.GetHashCode();

        #endregion

        #region Implementations

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool Equals(Option<TSome> other)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return EqualityComparer<TSome>.Default.Equals(_someValue, other._someValue);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool Equals(Option<TSome> other, [CanBeNull] IEqualityComparer<TSome> comparer)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return (comparer ?? EqualityComparer<TSome>.Default).Equals(_someValue, other._someValue);
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="Option{TSome}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Option{TSome}"/>.</returns>
        [NotNull, Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<TSome> GetEnumerator()
        {
            Ensures(Result<IEnumerator<TSome>>() != null);

            if (IsSome)
            {
                yield return _someValue;
            }
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
        public static bool operator ==(Option<TSome> left, Option<TSome> right) => left.Equals(right);

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
        // note(cosborn) Also implements || (LogicalOr) operator, see below.
        public static Option<TSome> operator |(Option<TSome> left, Option<TSome> right) => left.BitwiseOr(right);

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
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public Option<TSome> BitwiseOr(Option<TSome> other) => IsNone
            ? other
            : this;

        /// <summary>Performs logical conjunction between two objects of the same type.</summary>
        /// <param name="left">An object to conjoin with <paramref name="right"/>.</param>
        /// <param name="right">An object to conjoin with <paramref name="left"/>.</param>
        /// <returns>
        /// The last value of <paramref name="left"/> and <paramref name="right"/>
        /// if they are both in the Some state; otherwise, <see cref="None"/>.
        /// </returns>
        // note(cosborn) Also implements && (LogicalAnd) operator, see below.
        public static Option<TSome> operator &(Option<TSome> left, Option<TSome> right) => left.BitwiseAnd(right);

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
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public Option<TSome> BitwiseAnd(Option<TSome> other) => IsNone
            ? None
            : other;

        // note(cosborn) Implementing true and false operators allows || and && operators to short-circuit.

        /// <summary>Tests whether <paramref name="value"/> is in the Some state.</summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the Some state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator true(Option<TSome> value) => value.IsTrue;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsTrue => IsSome;

        /// <summary>Tests whether <paramref name="value"/> is in the None state.</summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator false(Option<TSome> value) => value.IsFalse;

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsFalse => IsNone;

        /// <summary>
        /// Tests the logical inverse of whether <paramref name="value"/>
        /// is in the Some state.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !(Option<TSome> value) => value.LogicalNot();

        /// <summary>
        /// Tests the logical inverse of whether this instance
        /// is in the Some state.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if this instance is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public bool LogicalNot() => IsNone;

        /// <summary>Wraps a value in <see cref="Option{TSome}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        public static implicit operator Option<TSome>([CanBeNull] TSome value) => new Option<TSome>(value);

        /// <summary>Unwraps the Some value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        public static explicit operator TSome(Option<TSome> value)
        {
            Requires<InvalidOperationException>(value.IsSome, Resources.OptionIsNone);

            return value._someValue;
        }

        // ReSharper disable once UnusedParameter.Global note(cosborn) Used only for the type inference.
        /// <summary>
        /// Implicitly converts a <see cref="OptionNone"/> to an
        /// <see cref="Option{TSome}"/> in the None state.
        /// </summary>
        /// <param name="none">The default value of <see cref="OptionNone"/>.</param>
        public static implicit operator Option<TSome>(OptionNone none) => None;

        #endregion
    }
}
