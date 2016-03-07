using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values.</summary>
    /// <typeparam name="TLeft">The Left type of the value that may be represented.</typeparam>
    /// <typeparam name="TRight">The Right type of the value that may be represented.</typeparam>
    [SuppressMessage("ReSharper", "ExceptionNotThrown", Justification = "R# doesn't understand Code Contracts.")]
    public struct Either<TLeft, TRight>
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="leftValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        [Pure]
        public static Either<TLeft, TRight> FromLeft([NotNull] TLeft leftValue)
        {
            Requires<ArgumentNullException>(leftValue != null);

            return new Either<TLeft, TRight>(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="rightValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        [Pure]
        public static Either<TLeft, TRight> FromRight([NotNull] TRight rightValue)
        {
            Requires<ArgumentNullException>(rightValue != null);

            return new Either<TLeft, TRight>(rightValue);
        }

        /// <summary>Gets a value indicating whether this instance is in the Left state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsLeft => _state == EitherState.Left;

        /// <summary>Gets a value indicating whether this instance is in the Right state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsRight => _state == EitherState.Right;

        readonly EitherState _state;
        readonly TLeft _leftValue;
        readonly TRight _rightValue;

        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TLeft leftValue)
            : this()
        {
            Requires<ArgumentNullException>(leftValue != null);

            _leftValue = leftValue;
            _state = EitherState.Left;
        }

        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TRight rightValue)
            : this()
        {
            Requires<ArgumentNullException>(rightValue != null);

            _rightValue = rightValue;
            _state = EitherState.Right;
        }

        #region Match

        #region Value

        /// <summary>Produces a value from this instance by matching on its state.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="left">
        /// A function to be invoked with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to be invoked with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);
            Ensures(Result<TOut>() != null);

            var result = IsLeft
                ? left(_leftValue)
                : right(_rightValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="left">
        /// A function to be invoked with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to be invoked asynchronously with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsLeft
                ? left(_leftValue)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="left">
        /// A function to be invoked asynchronously with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to be invoked with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : right(_rightValue);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        /// <summary>Produces a value from this instance by matching on its state, asynchronously.</summary>
        /// <typeparam name="TOut">The type of the value to be produced.</typeparam>
        /// <param name="left">
        /// A function to be invoked asynchronously with the Left value of this instance as
        /// the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to be invoked asynchronously with the Right value of this instance as
        /// the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A value produced by <paramref name="left"/> or <paramref name="right"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<TOut>() != null);
            Ensures(Result<Task<TOut>>() != null);

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, Resources.ResultIsNull);
            return result;
        }

        #endregion

        #region Void

        /// <summary>Performs an action on this instance by matching on its state.</summary>
        /// <param name="left">
        /// An action to be invoked with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public void Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                right(_rightValue);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task>() != null);

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                await right(_rightValue).ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task>() != null);

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }
            else
            {
                right(_rightValue);
            }
        }

        /// <summary>Performs an action on this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task>() != null);

            if (IsLeft)
            {
                await left(_leftValue);
            }

            await right(_rightValue);
        }

        #endregion

        #endregion

        #region Map

        /// <summary>Maps a function over the Left value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOut, TRight> Map<TOut>([NotNull, InstantHandle] Func<TLeft, TOut> left)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TOut, TRight>.FromLeft(leftResult);
            }

            // note(cosborn) Invariants don't understand sum types; can't link `!IsLeft` and `_rightValue != null`.
            Assume(_rightValue != null);
            return Either<TOut, TRight>.FromRight(_rightValue);
        }

        /// <summary>Maps a function over the Left value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TOut, TRight>> Map<TOut>([NotNull, InstantHandle] Func<TLeft, Task<TOut>> left)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TOut, TRight>>>() != null);

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TOut, TRight>.FromLeft(leftResult);
            }

            return Either<TOut, TRight>.FromRight(_rightValue);
        }

        /// <summary>Maps a function over the Right value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value set to
        /// the value of invoking <paramref name="right"/> over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeft, TOut> Map<TOut>([NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            if (IsLeft)
            { // note(cosborn) Invariants don't understand sum types; can't link `IsLeft` and `_leftValue != null`.
                Assume(_leftValue != null);
                return Either<TLeft, TOut>.FromLeft(_leftValue);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, Resources.ResultIsNull);
            return Either<TLeft, TOut>.FromRight(rightResult);
        }

        /// <summary>Maps a function over the Right value of this instance, if present, asynchronously.</summary>
        /// <typeparam name="TOut">The type to which to map.</typeparam>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value set to
        /// the value of invoking <paramref name="right"/> over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeft, TOut>> Map<TOut>([NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TLeft, TOut>>>() != null);

            if (IsLeft) { return Either<TLeft, TOut>.FromLeft(_leftValue); }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, Resources.ResultIsNull);
            return Either<TLeft, TOut>.FromRight(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance, whichever is present.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeftOut, TRightOut> Map<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, TLeftOut> left,
            [NotNull, InstantHandle] Func<TRight, TRightOut> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, Resources.ResultIsNull);
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> Map<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, TLeftOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TRightOut>> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TLeftOut, TRightOut>>>() != null);

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, Resources.ResultIsNull);
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> Map<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TLeftOut>> left,
            [NotNull, InstantHandle] Func<TRight, TRightOut> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TLeftOut, TRightOut>>>() != null);

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, Resources.ResultIsNull);
            return new Either<TLeftOut, TRightOut>(rightResult);
        }

        /// <summary>
        /// Maps a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TLeftOut">The type to which to map the Left type.</typeparam>
        /// <typeparam name="TRightOut">The type to which to map the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value set to
        /// the value of invoking <paramref name="left"/> over the Left value of this instance
        /// if this instance is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to the value of invoking <paramref name="right"/>
        /// over the Right value of this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeftOut, TRightOut>> Map<TLeftOut, TRightOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TLeftOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TRightOut>> right)
        {
            Requires<ArgumentNullException>(left != null);
            Requires<ArgumentNullException>(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TLeftOut, TRightOut>>>() != null);

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, Resources.ResultIsNull);
            return Either<TLeftOut, TRightOut>.FromRight(rightResult);
        }

        #endregion

        #region Bind

        /// <summary>Binds a function over the Left value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// or if the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its
        /// Left value set to the value of invoking <paramref name="left"/> over the Left value
        /// of this instance.
        /// </returns>
        [Pure]
        public Either<TOut, TRight> Bind<TOut>([NotNull, InstantHandle] Func<TLeft, Either<TOut, TRight>> left)
        {
            Requires(left != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            return IsLeft
                ? left(_leftValue)
                : _rightValue;
        }

        /// <summary>
        /// Binds a function over the Left value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// or if the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Left state with its
        /// Left value set to the value of invoking <paramref name="left"/> over the Left value
        /// of this instance.
        /// </returns>
        [NotNull, Pure]
        public async Task<Either<TOut, TRight>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOut, TRight>>> left)
        {
            Requires(left != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TOut, TRight>>>() != null);

            return IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : _rightValue;
        }

        /// <summary>Binds a function over the Right value of this instance, if present.</summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// or if the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its
        /// Right value set to the value of invoking <paramref name="right"/> over the Right value
        /// of this instance.
        /// </returns>
        [Pure]
        public Either<TLeft, TOut> Bind<TOut>([NotNull, InstantHandle] Func<TRight, Either<TLeft, TOut>> right)
        {
            Requires(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            return IsLeft
                ? _leftValue
                : right(_rightValue);
        }

        /// <summary>
        /// Binds a function over the Right value of this instance, if present, asynchronously.
        /// </summary>
        /// <typeparam name="TOut">The type to which to bind.</typeparam>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// or if the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Left state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its
        /// Right value set to the value of invoking <paramref name="right"/> over the Right value
        /// of this instance.
        /// </returns>
        [NotNull, Pure]
        public async Task<Either<TLeft, TOut>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TRight, Task<Either<TLeft, TOut>>> right)
        {
            Requires(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TLeft, TOut>>>() != null);

            return IsLeft
                ? _leftValue
                : await right(_rightValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance, whichever is present.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state;
        /// an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        [Pure]
        public Either<TOutLeft, TOutRight> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            Requires(left != null);
            Requires(right != null);
            Requires<InvalidOperationException>(IsLeft || IsRight, Resources.EitherIsBottom);

            return IsLeft
                ? left(_leftValue)
                : right(_rightValue);
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state;
        /// an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            Requires(left != null);
            Requires(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TOutLeft, TOutRight>>>() != null);

            return IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : right(_rightValue);
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state;
        /// an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            Requires(left != null);
            Requires(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TOutLeft, TOutRight>>>() != null);

            return IsLeft
                ? left(_leftValue)
                : await right(_rightValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Binds a function over the Left or Right value of this instance,
        /// whichever is present, asynchronously.
        /// </summary>
        /// <typeparam name="TOutLeft">The type to which to bind the Left type.</typeparam>
        /// <typeparam name="TOutRight">The type to which to bind the Right type.</typeparam>
        /// <param name="left">
        /// A function to invoke asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// A function to invoke asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state if this instance is in the Left state
        /// and the result of invoking <paramref name="left"/> over the Left value of this instance
        /// is in the Left state or if this instance is in the Right state and the result of invoking
        /// <paramref name="right"/> over the Right value of this instance is in the Left state;
        /// an <see cref="Either{TLeft,TRight}"/> in the Right state if this instance is in the Right state
        /// and the result of invoking <paramref name="right"/> over the Right value of this instance
        /// is in the Right state or if this instance is in the Left state and the result of invoking
        /// <paramref name="left"/> over the Left value of this instance is in the Right state.
        /// </returns>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            Requires(left != null);
            Requires(right != null);
            Requires<InvalidOperationException>(this != default(Either<TLeft, TRight>), Resources.EitherIsBottom);
            Ensures(Result<Task<Either<TOutLeft, TOutRight>>>() != null);

            return IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : await right(_rightValue).ConfigureAwait(false);
        }

        #endregion

        #region Overrides

        /// <summary>Converts this instance to a string.</summary>
        /// <returns>A <see cref="string"/> containing the value of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override string ToString()
        {
            switch (_state)
            {
                case EitherState.Left:
                    return string.Format(CultureInfo.InvariantCulture, "Left({0})", _leftValue);
                case EitherState.Right:
                    return string.Format(CultureInfo.InvariantCulture, "Right({0})", _rightValue);
                default:
                    return "Bottom";
            }
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance
        /// are the same type and represent the same value; otherwise, <see langword="false"/>. 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Either<TLeft, TRight> && Equals((Either<TLeft, TRight>)obj);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override int GetHashCode() => IsLeft
            ? _leftValue.GetHashCode()
            : _rightValue.GetHashCode();

        #endregion

        #region Implementations

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [Pure]
        public bool Equals(Either<TLeft, TRight> other)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (_state == EitherState.Bottom && other._state == EitherState.Bottom) { return true; }

            if (IsLeft && other.IsLeft)
            {
                return EqualityComparer<TLeft>.Default.Equals(_leftValue, other._leftValue);
            }

            if (IsRight && other.IsRight)
            {
                return EqualityComparer<TRight>.Default.Equals(_rightValue, other._rightValue);
            }

            // note(cosborn) Implicitly `_state != other._state`.
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Either{TLeft,TRight}"/>.</returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<TRight> GetEnumerator()
        {
            Ensures(Result<IEnumerator<TRight>>() != null);

            if (IsRight)
            {
                yield return _rightValue;
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
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.Equals(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            !(left == right);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="leftValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TLeft leftValue) => leftValue == null
            ? default(Either<TLeft, TRight>)
            : FromLeft(leftValue);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="rightValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TRight rightValue) => rightValue == null
            ? default(Either<TLeft, TRight>)
            : FromRight(rightValue);

        #endregion
    }
}
