using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using LINQPad;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values.</summary>
    /// <typeparam name="TLeft">The Left type of the value that may be represented.</typeparam>
    /// <typeparam name="TRight">The Right type of the value that may be represented.</typeparam>
    public struct Either<TLeft, TRight>
        : ICustomMemberProvider
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="leftValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> FromLeft([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new Either<TLeft, TRight>(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="rightValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> FromRight([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new Either<TLeft, TRight>(rightValue);
        }

        /// <summary>Gets a value indicating whether this instance is in the Left state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsLeft => State == EitherState.Left;

        /// <summary>Gets a value indicating whether this instance is in the Right state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsRight => State == EitherState.Right;

        internal readonly EitherState State;
        internal readonly TLeft LeftValue;
        internal readonly TRight RightValue;

        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TLeft leftValue)
            : this()
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            LeftValue = leftValue;
            State = EitherState.Left;
        }

        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        internal Either([NotNull] TRight rightValue)
            : this()
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            RightValue = rightValue;
            State = EitherState.Right;
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? left(LeftValue)
                : right(RightValue);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? left(LeftValue)
                : await right(RightValue).ConfigureAwait(false);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? await left(LeftValue).ConfigureAwait(false)
                : right(RightValue);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? await left(LeftValue).ConfigureAwait(false)
                : await right(RightValue).ConfigureAwait(false);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                left(LeftValue);
            }
            else
            {
                right(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                left(LeftValue);
            }
            else
            {
                await right(RightValue).ConfigureAwait(false);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                await left(LeftValue).ConfigureAwait(false);
            }
            else
            {
                right(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                await left(LeftValue);
            }

            await right(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(LeftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TOut, TRight>.FromLeft(leftResult);
            }

            return Either<TOut, TRight>.FromRight(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(LeftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TOut, TRight>.FromLeft(leftResult);
            }

            return Either<TOut, TRight>.FromRight(RightValue);
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
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            { // note(cosborn) Invariants don't understand sum types; can't link `IsLeft` and `_leftValue != null`.
                Assume(LeftValue != null);
                return Either<TLeft, TOut>.FromLeft(LeftValue);
            }

            var rightResult = right(RightValue);
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
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft) { return Either<TLeft, TOut>.FromLeft(LeftValue); }

            var rightResult = await right(RightValue).ConfigureAwait(false);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(LeftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = right(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(LeftValue);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = await right(RightValue).ConfigureAwait(false);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(LeftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = right(RightValue);
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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(LeftValue).ConfigureAwait(false);
                Assume(leftResult != null, Resources.ResultIsNull);
                return Either<TLeftOut, TRightOut>.FromLeft(leftResult);
            }

            var rightResult = await right(RightValue).ConfigureAwait(false);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOut, TRight> Bind<TOut>([NotNull, InstantHandle] Func<TLeft, Either<TOut, TRight>> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                return left(LeftValue);
            }

            Assume(RightValue != null);
            return Either<TOut, TRight>.FromRight(RightValue);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TOut, TRight>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOut, TRight>>> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            if (IsLeft)
            {
                return await left(LeftValue).ConfigureAwait(false);
            }

            Assume(RightValue != null);
            return Either<TOut, TRight>.FromRight(RightValue);
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
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TLeft, TOut> Bind<TOut>([NotNull, InstantHandle] Func<TRight, Either<TLeft, TOut>> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? Either<TLeft, TOut>.FromLeft(LeftValue)
                : right(RightValue);
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
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<Either<TLeft, TOut>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TRight, Task<Either<TLeft, TOut>>> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? Either<TLeft, TOut>.FromLeft(LeftValue)
                : await right(RightValue).ConfigureAwait(false);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public Either<TOutLeft, TOutRight> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? left(LeftValue)
                : right(RightValue);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? await left(LeftValue).ConfigureAwait(false)
                : right(RightValue);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? left(LeftValue)
                : await right(RightValue).ConfigureAwait(false);
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
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull]
        public async Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            return IsLeft
                ? await left(LeftValue).ConfigureAwait(false)
                : await right(RightValue).ConfigureAwait(false);
        }

        #endregion

        #region Fold

        /// <summary>Combines the provided seed state with the Left value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Left value of this instance.</param>
        /// <param name="left">
        /// A function to invoke with the seed value and the Left value of this instance as the arguments
        /// if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TLeft, TState> left)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? left(state, LeftValue)
                : state;
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        /// <summary>Combines the provided seed state with the Right value of this instance.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Right value of this instance.</param>
        /// <param name="right">
        /// A function to invoke with the seed value and the Right value of this instance as the arguments
        /// if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public TState Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TRight, TState> right)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsRight
                ? right(state, RightValue)
                : state;
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        /// <summary>Combines the provided seed state with the Left value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Left value of this instance.</param>
        /// <param name="left">
        /// A function to invoke with the seed value and the Left value of this instance as the arguments
        /// if this instance is in the Left state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Left value of this instance
        /// if this instance is in the Left state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TLeft, Task<TState>> left)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsLeft
                ? await left(state, LeftValue).ConfigureAwait(false)
                : state;
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        /// <summary>Combines the provided seed state with the Right value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Right value of this instance.</param>
        /// <param name="right">
        /// A function to invoke with the seed value and the Right value of this instance as the arguments
        /// if this instance is in the Right state.
        /// </param>
        /// <returns>
        /// The result of combining the provided seed value with the Right value of this instance
        /// if this instance is in the Right state; otherwise, the seed value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull, Pure]
        public async Task<TState> Fold<TState>(
            [NotNull] TState state,
            [NotNull, InstantHandle] Func<TState, TRight, Task<TState>> right)
        {
            if (state == null) { throw new ArgumentNullException(nameof(state)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == EitherState.Bottom) { throw new InvalidOperationException(Resources.EitherIsBottom); }

            var result = IsRight
                ? await right(state, RightValue).ConfigureAwait(false)
                : state;
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action on the Left value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="left">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null" />.</exception>
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(LeftValue);
            }
            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null" />.</exception>
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            { // note(cosborn) Remember, `IsLeft` and `IsRight` can both be false.
                right(RightValue);
            }
            return this;
        }

        /// <summary>
        /// Performs an action on the Left value of this instance asynchronously,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="left">An action to perform asynchronously.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null" />.</exception>
        [NotNull]
        public async Task<Either<TLeft, TRight>> Tap([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(LeftValue).ConfigureAwait(false);
            }
            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns this instance.
        /// </summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null" />.</exception>
        [NotNull]
        public async Task<Either<TLeft, TRight>> Tap([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            { // note(cosborn) Remember, `IsLeft` and `IsRight` can both be false.
                await right(RightValue).ConfigureAwait(false);
            }
            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Left value of this instance.</summary>
        /// <param name="left">An action to perform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        public void Let([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(LeftValue);
            }
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        public void Let([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                right(RightValue);
            }
        }

        /// <summary>Performs an action on the Left value of this instance, asynchronously.</summary>
        /// <param name="left">An action to perform asynchronously.</param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Let([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(LeftValue).ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Let([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                await right(RightValue).ConfigureAwait(false);
            }
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// if this instance is in the right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Right state whose Some value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        public Either<TLeft, TRight> Recover([NotNull] TRight recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            return IsRight
                ? this
                : FromRight(recoverer);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// if this instance is in the right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Right state whose Some value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This result evaluated to <see langword="null"/>.</exception>
        public Either<TLeft, TRight> Recover([NotNull, InstantHandle] Func<TRight> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = recoverer();
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return FromRight(result);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state, asynchronously.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// if this instance is in the right state; otherwise, an <see cref="Either{TLeft,TRight}"/> in the
        /// Right state whose Some value is the value of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This result evaluated to <see langword="null"/>.</exception>
        public async Task<Either<TLeft, TRight>> Recover([NotNull, InstantHandle] Func<Task<TRight>> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        #endregion

        #region Value

        /// <summary>Gets the Right value of this instance.</summary>
        /// <remarks>This property is unsafe, as it can throw if this instance is not in the Right state.</remarks>
        /// <exception cref="InvalidOperationException" accessor="get">
        /// This instance is in an invalid state.
        /// </exception>
        public TRight Value
        {
            get
            {
                if (!IsRight) { throw new InvalidOperationException(Resources.EitherIsNotRight); }

                return RightValue;
            }
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the default value of <typeparamref name="TRight"/>.
        /// </returns>
        /// <remarks>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TRight"/> satisfies <see langword="class"/>.</remarks>
        [CanBeNull, Pure]
        public TRight GetValueOrDefault() => RightValue;

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the default value of <typeparamref name="TRight"/>.
        /// </returns>
        /// <remarks>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TRight"/> satisfies <see langword="class"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public TRight GetValueOrDefault([NotNull] TRight other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return IsRight
                ? RightValue
                : other;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">A function producing an alternative value.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the result of invoking <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This result evaluated to <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TRight GetValueOrDefault([NotNull, InstantHandle] Func<TRight> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? RightValue
                : other();
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        /// <summary>
        /// Unwraps this instance asynchronously with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the result of invoking <paramref name="other"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This result evaluated to <see langword="null"/>.</exception>
        [NotNull, ItemCanBeNull, Pure]
        public async Task<TRight> GetValueOrDefault([NotNull, InstantHandle] Func<Task<TRight>> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? RightValue
                : await other().ConfigureAwait(false);
            if (result == null) { throw new InvalidOperationException(Resources.ResultIsNull); }
            return result;
        }

        #endregion

        #region Overrides

        /// <summary>Converts this instance to a string.</summary>
        /// <returns>A <see cref="string"/> containing the value of this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override string ToString()
        {
            switch (State)
            {
                case EitherState.Left:
                    return string.Format(CultureInfo.InvariantCulture, @"Left({0})", LeftValue);
                case EitherState.Right:
                    return string.Format(CultureInfo.InvariantCulture, @"Right({0})", RightValue);
                case EitherState.Bottom:
                    return @"Bottom";
                default: // note(cosborn) Why would you change this enum???
                    return string.Empty;
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
            obj is Either<TLeft, TRight> && EqualsCore((Either<TLeft, TRight>)obj);

        [Pure]
        bool EqualsCore(Either<TLeft, TRight> other)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (State == EitherState.Bottom && other.State == EitherState.Bottom)
            {
                return true;
            }

            if (IsLeft && other.IsLeft)
            {
                return EqualityComparer<TLeft>.Default.Equals(LeftValue, other.LeftValue);
            }

            if (IsRight && other.IsRight)
            {
                return EqualityComparer<TRight>.Default.Equals(RightValue, other.RightValue);
            }

            // note(cosborn) Implicitly `_state != other._state`.
            return false;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        [Pure]
        public override int GetHashCode()
        {
            switch (State)
            {
                case EitherState.Left:
                    return LeftValue.GetHashCode();
                case EitherState.Right:
                    return RightValue.GetHashCode();
                case EitherState.Bottom:
                    return 0;
                default: // note(cosborn) Why would you change this enum???
                    return 0;
            }
        }

        #endregion

        #region Implementations

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Either{TLeft,TRight}"/>.</returns>
        [Pure, EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<TRight> GetEnumerator()
        {
            if (IsRight)
            {
                yield return RightValue;
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
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.EqualsCore(right);

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

        /// <summary>Unwraps the Right value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        public static explicit operator TRight(Either<TLeft, TRight> value)
        {
            if (!value.IsRight) { throw new InvalidOperationException(Resources.EitherIsNotRight); }

            return value.RightValue;
        }

        #endregion
    }
}
