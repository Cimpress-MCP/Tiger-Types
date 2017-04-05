using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
    public partial struct Option<TSome>
    {
        /// <summary>Gets a value representing no value.</summary>
        public static Option<TSome> None = default(Option<TSome>);

        readonly TSome _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Option{TSome}"/> struct.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        internal Option([NotNull] TSome value)
        {
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
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TSome, TOut> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : none;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            [NotNull] TOut none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? await some(_value).ConfigureAwait(false)
                : none;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : none();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
        [ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? await some(_value).ConfigureAwait(false)
                : none();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            var result = IsSome
                ? some(_value)
                : await none().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

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
        /// An action to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

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
        /// An action to be invoked with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Action none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
            }
            else
            {
                none();
            }
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Action<TSome> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }
            else
            {
                await none().ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="none">
        /// An action to be invoked asynchronously with no arguments if this instance is in the None state.
        /// </param>
        /// <param name="some">
        /// An action to be invoked asynchronously with the Some value of this instance as
        /// the argument if this instance is in the Some state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Match(
            [NotNull, InstantHandle] Func<Task> none,
            [NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
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
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

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
        public async Task<Option<TOut>> Map<TOut>([NotNull, InstantHandle] Func<TSome, Task<TOut>> some)
        {
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

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
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return Map(some).Pipe(Option.Join);
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
        public Task<Option<TOut>> Bind<TOut>([NotNull, InstantHandle] Func<TSome, Task<Option<TOut>>> some)
        {
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return Map(some).Map(Option.Join);
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
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

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
        public async Task<Option<TSome>> Filter([NotNull, InstantHandle] Func<TSome, Task<bool>> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            return IsSome
                ? await predicate(_value).ConfigureAwait(false) ? this : None
                : None;
        }

        #endregion

        #region Fold

        /// <summary>Combines the provided seed state with the Some value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Some value of this instance.</param>
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
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }

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
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }

            var result = IsSome
                ? await folder(state, _value).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action on the Some value of this instance,
        /// if present, and returns the same value as this instance.
        /// </summary>
        /// <param name="some">An action to perform.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public Option<TSome> Tap([NotNull, InstantHandle] Action<TSome> some)
        {
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                some(_value);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Some value of this instance asynchronously,
        /// if present, and returns the same value as this instance.
        /// </summary>
        /// <param name="some">An action to perform asynchronously.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Option<TSome>> Tap([NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value).ConfigureAwait(false);
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
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

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
        public async Task Let([NotNull, InstantHandle] Func<TSome, Task> some)
        {
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            if (IsSome)
            {
                await some(_value);
            }
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
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

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
        public async Task<Option<TSome>> Recover([NotNull] Func<Task<TSome>> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsSome) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Option<TSome>(result);
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
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

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
        public async Task<TSome> GetValueOrDefault([NotNull, InstantHandle] Func<Task<TSome>> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsSome
                ? _value
                : await other().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion
    }
}
