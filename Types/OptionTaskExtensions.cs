using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// Extensions to the functionality of <see cref="Task{TResult}"/>,
    /// specialized for <see cref="Option{TSome}"/>.
    /// </summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class OptionTaskExtensions
    {
        #region MatchT

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be matched.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Map(v => v.Match(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be matched.</param>
        /// <param name="none">
        /// A transformation from nothing to <typeparamref name="TOut"/> to perform
        /// in the None case.
        /// </param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to
        /// <typeparamref name="TOut"/> to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TOut> none,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.Match(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be matched.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TIn, TOut> some)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.Match(none: none, some: some));
        }

        /// <summary>
        /// Transforms the result of <paramref name="optionTaskValue"/>
        /// based on its state, asynchronously.
        /// </summary>
        /// <typeparam name="TIn">
        /// The Some type of the Result type of <paramref name="optionTaskValue"/>.
        /// </typeparam>
        /// <typeparam name="TOut">The type to which to transform.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be matched.</param>
        /// <param name="none">
        /// An asynchronous transformation from nothing to <typeparamref name="TOut"/>
        /// to perform in the None case.
        /// </param>
        /// <param name="some">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// to perform in the Some case.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="TOut"/> created from one of
        /// <paramref name="none"/> or <paramref name="some"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="none"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="some"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<TOut> MatchT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<Task<TOut>> none,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> some)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (none == null) { throw new ArgumentNullException(nameof(none)); }
            if (some == null) { throw new ArgumentNullException(nameof(some)); }

            return optionTaskValue.Bind(v => v.Match(none: none, some: some));
        }

        #endregion

        #region MapT

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be mapped.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> MapT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, TOut> mapper)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return optionTaskValue.Map(ov => ov.Map(mapper));
        }

        /// <summary>
        /// Maps the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Result type of the return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be mapped.</param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> MapT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<TOut>> mapper)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (mapper == null) { throw new ArgumentNullException(nameof(mapper)); }

            return optionTaskValue.Bind(ov => ov.Map(mapper));
        }

        #endregion

        #region BindT

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Some type of the return type of <paramref name="binder"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be bound.</param>
        /// <param name="binder">
        /// A transformation from <typeparamref name="TIn"/> to <see cref="Option{TSome}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> BindT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Option<TOut>> binder)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            return optionTaskValue.Map(ov => ov.Bind(binder));
        }

        /// <summary>
        /// Binds the result of a <see cref="Task{TResult}"/> asynchronously over a transformation.
        /// </summary>
        /// <typeparam name="TIn">The Some type of the Result type of <paramref name="optionTaskValue"/>.</typeparam>
        /// <typeparam name="TOut">The Some type of the Result type of the return type of <paramref name="binder"/>.</typeparam>
        /// <param name="optionTaskValue">The <see cref="Task{TResult}"/> to be bound.</param>
        /// <param name="binder">
        /// An asynchronous transformation from <typeparamref name="TIn"/> to <see cref="Option{TSome}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Option{TSome}"/>, with the transformation applied if the result of
        /// <paramref name="optionTaskValue"/> was in the Some state, asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="optionTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public static Task<Option<TOut>> BindT<TIn, TOut>(
            [NotNull] this Task<Option<TIn>> optionTaskValue,
            [NotNull, InstantHandle] Func<TIn, Task<Option<TOut>>> binder)
        {
            if (optionTaskValue == null) { throw new ArgumentNullException(nameof(optionTaskValue)); }
            if (binder == null) { throw new ArgumentNullException(nameof(binder)); }

            return optionTaskValue.Bind(ov => ov.Bind(binder));
        }

        #endregion

        #region TapT

        /// <summary>
        /// Acts upon the Some value of the Result value of <paramref name="resultTaskValue"/>,
        /// if it is present.
        /// </summary>
        /// <typeparam name="T">The Some type of the Result type of <paramref name="resultTaskValue"/>.</typeparam>
        /// <param name="resultTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="tapper">
        /// An action to perform on the Some value of the Result value
        /// of <paramref name="resultTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="resultTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resultTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Option<T>> TapT<T>(
            [NotNull] this Task<Option<T>> resultTaskValue,
            [NotNull, InstantHandle] Action<T> tapper)
        {
            if (resultTaskValue == null) { throw new ArgumentNullException(nameof(resultTaskValue)); }
            if (tapper == null) { throw new ArgumentNullException(nameof(tapper)); }

            return resultTaskValue.Map(ov => ov.Tap(tapper));
        }

        /// <summary>
        /// Acts upon the Some value of the Result value of <paramref name="resultTaskValue"/>,
        /// if it is present, asynchronously.
        /// </summary>
        /// <typeparam name="T">The Some type of the Result type of <paramref name="resultTaskValue"/>.</typeparam>
        /// <param name="resultTaskValue">The <see cref="Task{TResult}"/> to tap.</param>
        /// <param name="tapper">
        /// An asynchronous action to perform on the Some value of the Result value
        /// of <paramref name="resultTaskValue"/>.
        /// </param>
        /// <returns>The original value of <paramref name="resultTaskValue"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resultTaskValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tapper"/> is <see langword="null"/>.</exception>
        [NotNull, MustUseReturnValue]
        public static Task<Option<T>> TapT<T>(
            [NotNull] this Task<Option<T>> resultTaskValue,
            [NotNull, InstantHandle] Func<T, Task> tapper)
        {
            if (resultTaskValue == null) { throw new ArgumentNullException(nameof(resultTaskValue)); }
            if (tapper == null) { throw new ArgumentNullException(nameof(tapper)); }

            return resultTaskValue.Bind(ov => ov.Tap(tapper));
        }

        #endregion
    }
}
