using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using LINQPad;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Represents the presence or absence of a value.</summary>
    /// <typeparam name="TSome">The Some type of the value that may be represented.</typeparam>
    [SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "R# doesn't understand Code Contracts.")]
    [TypeConverter(typeof(OptionTypeConverter))]
    [JsonConverter(typeof(OptionJsonConverter))] // todo(cosborn) Library.
    [DebuggerTypeProxy(typeof(OptionDebuggerTypeProxy<>))]
    public struct Option<TSome>
        : ICustomMemberProvider
    {
        /// <summary>Gets a value representing no value.</summary>
        public static Option<TSome> None
        {
            get
            {
                Ensures(Result<Option<TSome>>().IsNone);
                Ensures(!Result<Option<TSome>>().IsSome);

                return default(Option<TSome>);
            }
        }

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
        public static Option<TSome> From([CanBeNull] TSome value) =>
            value == null ? None : new Option<TSome>(value);

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsNone => !_isSome;

        /// <summary>Gets a value indicating whether this instance is in the Some state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsSome => _isSome;

        [ContractPublicPropertyName(nameof(IsSome))]
        readonly bool _isSome;
        internal readonly TSome SomeValue;

        internal Option([NotNull] TSome someValue)
            : this()
        {
            Requires(someValue != null);
            Ensures(SomeValue != null);
            Ensures(IsSome);

            SomeValue = someValue;
            _isSome = true;
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

            var result = IsSome
                ? some(SomeValue)
                : none;
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
            Ensures(Result<Task<TOut>>() != null);

            var result = _isSome
                ? await some(SomeValue).ConfigureAwait(false)
                : none;
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

            var result = _isSome
                ? some(SomeValue)
                : none();
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

            var result = _isSome
                ? await some(SomeValue).ConfigureAwait(false)
                : none();
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

            var result = _isSome
                ? some(SomeValue)
                : await none().ConfigureAwait(false);
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

            var result = _isSome
                ? await some(SomeValue).ConfigureAwait(false)
                : await none().ConfigureAwait(false);
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

            if (_isSome)
            {
                some(SomeValue);
            }
            else
            {
                none();
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

            if (_isSome)
            {
                await some(SomeValue).ConfigureAwait(false);
            }
            else
            {
                none();
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

            if (_isSome)
            {
                some(SomeValue);
            }
            else
            {
                await none().ConfigureAwait(false);
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

            if (_isSome)
            {
                await some(SomeValue).ConfigureAwait(false);
            }
            else
            {
                await none().ConfigureAwait(false);
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
            Ensures(Result<Option<TOut>>().IsNone == IsNone);
            Ensures(Result<Option<TOut>>().IsSome == IsSome);

            if (_isSome)
            {
                var result = mapper(SomeValue);
                Assume(result != null, Resources.ResultIsNull);
                return new Option<TOut>(result);
            }

            return Option<TOut>.None;
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

            if (_isSome)
            {
                var result = await mapper(SomeValue).ConfigureAwait(false);
                Assume(result != null, Resources.ResultIsNull);
                return new Option<TOut>(result);
            }

            return Option<TOut>.None;
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

            return _isSome
                ? binder(SomeValue)
                : Option<TOut>.None;
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

            return _isSome
                ? await binder(SomeValue).ConfigureAwait(false)
                : Option<TOut>.None;
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

            return _isSome
                ? predicate(SomeValue) ? this : None
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
        public async Task<Option<TSome>> Filter([NotNull, InstantHandle] Func<TSome, Task<bool>> predicate)
        {
            Requires<ArgumentNullException>(predicate != null);
            Ensures(Result<Task<Option<TSome>>>() != null);

            return _isSome
                ? await predicate(SomeValue).ConfigureAwait(false) ? this : None
                : None;
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
            
            var result = _isSome
                ? folder(state, SomeValue)
                : state;
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

            var result = _isSome
                ? await folder(state, SomeValue).ConfigureAwait(false)
                : state;
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
            Ensures(Result<Option<TSome>>().IsNone == IsNone);
            Ensures(Result<Option<TSome>>().IsSome == IsSome);

            if (_isSome)
            {
                tapper(SomeValue);
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

            if (_isSome)
            {
                await tapper(SomeValue).ConfigureAwait(false);
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

            if (_isSome)
            {
                action(SomeValue);
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

            if (_isSome)
            {
                await action(SomeValue);
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
            Ensures(!Result<Option<TSome>>().IsNone);
            Ensures(Result<Option<TSome>>().IsSome);

            return _isSome
                ? this
                : new Option<TSome>(recoverer);
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
            Ensures(!Result<Option<TSome>>().IsNone);
            Ensures(Result<Option<TSome>>().IsSome);

            if (_isSome) { return this; }

            var result = recoverer();
            Assume(result != null, Resources.ResultIsNull);
            return new Option<TSome>(result);
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

            if (_isSome) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return new Option<TSome>(result);
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
                Requires<InvalidOperationException>(_isSome, Resources.OptionIsNone);
                Ensures(Result<TSome>() != null);

                // note(cosborn) Invariants don't understand sum types; can't link `IsSome` and `SomeValue != null`.
                Assume(SomeValue != null);
                return SomeValue;
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
        public TSome GetValueOrDefault() => SomeValue;

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

            if (_isSome)
            { // note(cosborn) Invariants don't understand sum types; can't link `IsSome` and `SomeValue != null`.
                Assume(SomeValue != null);
                return SomeValue;
            }

            return other;
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

            var result = _isSome
                ? SomeValue
                : other();
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

            var result = _isSome
                ? SomeValue
                : await other().ConfigureAwait(false);
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
            : string.Format(CultureInfo.InvariantCulture, "Some({0})", SomeValue);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance
        /// are the same type and represent the same value; otherwise, <see langword="false"/>. 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is Option<TSome> && EqualsCore((Option<TSome>)obj);
        }

        [Pure]
        bool EqualsCore(Option<TSome> other)
        {
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return SomeValue.Equals(other.SomeValue);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override int GetHashCode() => IsNone
            ? 0
            : SomeValue.GetHashCode();

        #endregion

        #region Implementations

        //note(cosborn) OK, it's kind of an implementation.
        /// <summary>Returns an enumerator that iterates through the <see cref="Option{TSome}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Option{TSome}"/>.</returns>
        [NotNull, Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<TSome> GetEnumerator()
        {
            Ensures(Result<IEnumerator<TSome>>() != null);

            if (IsSome)
            {
                yield return SomeValue;
            }
        }

        IEnumerable<string> ICustomMemberProvider.GetNames()
        {
            yield return string.Empty;
        }

        IEnumerable<Type> ICustomMemberProvider.GetTypes()
        {
            yield return typeof(string);
        }

        IEnumerable<object> ICustomMemberProvider.GetValues()
        {
            yield return ToString();
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
        public Option<TSome> BitwiseOr(Option<TSome> other) => _isSome ? this : other;

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
        public Option<TSome> BitwiseAnd(Option<TSome> other) => _isSome ? other : None;

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
        public bool IsTrue => _isSome;

        /// <summary>Tests whether <paramref name="value"/> is in the None state.</summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is in the None state;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator false(Option<TSome> value) => value.IsFalse;

        /// <summary>Gets a value indicating whether this instance is in the None state.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsFalse => !_isSome;

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
        public bool LogicalNot() => !_isSome;

        /// <summary>Wraps a value in <see cref="Option{TSome}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        public static implicit operator Option<TSome>([CanBeNull] TSome value) => value == null
            ? None
            : new Option<TSome>(value);

        /// <summary>Unwraps the Some value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        public static explicit operator TSome(Option<TSome> value)
        {
            Requires<InvalidOperationException>(value._isSome, Resources.OptionIsNone);
            Ensures(Result<TSome>() != null);

            Assume(value.SomeValue != null); // note(cosborn) I just can't prove this!
            return value.SomeValue;
        }

        // ReSharper disable once UnusedParameter.Global note(cosborn) Used only for the type inference.
        /// <summary>
        /// Implicitly converts a <see cref="OptionNone"/> to an
        /// <see cref="Option{TSome}"/> in the None state.
        /// </summary>
        /// <param name="none">The default value of <see cref="OptionNone"/>.</param>
        public static implicit operator Option<TSome>(OptionNone none)
        {
            Ensures(Result<Option<TSome>>().IsNone);
            Ensures(!Result<Option<TSome>>().IsSome);

            return None;
        }

        #endregion
    }
}
