using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values.</summary>
    /// <typeparam name="TLeft">The Left type of the value that may be represented.</typeparam>
    /// <typeparam name="TRight">The Right type of the value that may be represented.</typeparam>
    public struct Either<TLeft, TRight>
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="leftValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <remarks>
        /// Passing a nullable struct into this method is likely to confuse
        /// both the type system and the programmer.
        /// </remarks>
        public static Either<TLeft, TRight> Left([NotNull] TLeft leftValue) =>
            new Either<TLeft, TRight>(leftValue);

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="rightValue">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <remarks>
        /// Passing a nullable struct into this method is likely to confuse
        /// both the type system and the programmer.
        /// </remarks>
        public static Either<TLeft, TRight> Right([NotNull] TRight rightValue) =>
            new Either<TLeft, TRight>(rightValue);

        enum EitherState
            : byte // todo(cosborn) Does this save anything?
        {
            Bottom, // note(cosborn) Must be 0 in the case of default(Option<TLeft, TRight).
            Left,
            Right
        }

        bool IsBottom => _state == EitherState.Bottom;

        /// <summary>Gets a value indicating whether this instance is in the Left state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsLeft => _state == EitherState.Left;

        /// <summary>Gets a value indicating whether this instance is in the Right state.</summary>
        /// <remarks>There are usually better ways to do this.</remarks>
        public bool IsRight => _state == EitherState.Right;

        readonly EitherState _state;
        readonly TLeft _leftValue;
        readonly TRight _rightValue;

        Either([NotNull] TLeft leftValue)
            : this()
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue));}

            _leftValue = leftValue;
            _state = EitherState.Left;
        }

        Either([NotNull] TRight rightValue)
            : this()
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        [NotNull]
        public TOut Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized.");}

            return IsLeft
                ? left(_leftValue)
                : right(_rightValue);
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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, TOut> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

            return IsLeft
                ? left(_leftValue)
                : await right(_rightValue).ConfigureAwait(false);
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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull]
        public async Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, TOut> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

            return IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : right(_rightValue);
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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        [NotNull, ItemNotNull]
        public Task<TOut> Match<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<TOut>> left,
            [NotNull, InstantHandle] Func<TRight, Task<TOut>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

            return IsLeft
                ? left(_leftValue)
                : right(_rightValue);
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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        public void Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

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
        /// <exception cref="BottomException">This instance has not been initialized.</exception>
        public Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (IsBottom) { throw new BottomException("This instance has not been initialized."); }

            return IsLeft
                ? left(_leftValue)
                : right(_rightValue);
        }

        #endregion

        #endregion
    }
}
