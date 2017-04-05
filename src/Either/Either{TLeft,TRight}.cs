using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;
using static System.Runtime.InteropServices.LayoutKind;
using static Tiger.Types.EitherState;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values.</summary>
    /// <typeparam name="TLeft">The Left type of the value that may be represented.</typeparam>
    /// <typeparam name="TRight">The Right type of the value that may be represented.</typeparam>
    [DebuggerTypeProxy(typeof(Either<,>.DebuggerTypeProxy))]
    [StructLayout(Auto)]
    public partial struct Either<TLeft, TRight>
    {
        readonly TLeft _leftValue;
        readonly TRight _rightValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct.
        /// </summary>
        /// <param name="leftValue">The value to use as the Left value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        internal Either([NotNull] TLeft leftValue)
            : this()
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            _leftValue = leftValue;
            State = Left;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct.
        /// </summary>
        /// <param name="rightValue">The value to use as the Right value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        internal Either([NotNull] TRight rightValue)
            : this()
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            _rightValue = rightValue;
            State = Right;
        }

        /// <summary>Gets a value indicating whether this instance is in the Left state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsLeft => State == Left;

        /// <summary>Gets a value indicating whether this instance is in the Right state.</summary>
        /// <remarks><para>There are usually better ways to do this.</para></remarks>
        public bool IsRight => State == Right;

        /// <summary>Gets the Right value of this instance.</summary>
        /// <remarks>
        /// <para>This property is unsafe, as it can throw if this instance is not in the Right state.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException" accessor="get">This instance is in an invalid state.</exception>
        [NotNull]
        public TRight Value
        {
            get
            {
                if (!IsRight) { throw new InvalidOperationException(EitherIsNotRight); }

                return _rightValue;
            }
        }

        /// <summary>Gets the current state of this instance.</summary>
        internal EitherState State { get; }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> FromLeft([NotNull] TLeft value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return new Either<TLeft, TRight>(value);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> FromRight([NotNull] TRight value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return new Either<TLeft, TRight>(value);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(_leftValue)
                : right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(_leftValue)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(_leftValue).ConfigureAwait(false)
                : await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Void

        /// <summary>Performs an action with this instance by matching on its state.</summary>
        /// <param name="left">
        /// An action to be invoked with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public Unit Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                right(_rightValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Action<TLeft> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                left(_leftValue);
            }
            else
            {
                await right(_rightValue).ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Action<TRight> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }
            else
            {
                right(_rightValue);
            }
        }

        /// <summary>Performs an action with this instance by matching on its state, asynchronously.</summary>
        /// <param name="left">
        /// An action to be invoked asynchronously with the Left value of this instance
        /// as the argument if this instance is in the Left state.
        /// </param>
        /// <param name="right">
        /// An action to be invoked asynchronously with the Right value of this instance
        /// as the argument if this instance is in the Right state.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public async Task Match(
            [NotNull, InstantHandle] Func<TLeft, Task> left,
            [NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

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
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var result = left(_leftValue);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TOut, TRight>(result);
            }

            return new Either<TOut, TRight>(_rightValue);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var result = await left(_leftValue).ConfigureAwait(false);
                Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TOut, TRight>(result);
            }

            return new Either<TOut, TRight>(_rightValue);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft) { return new Either<TLeft, TOut>(_leftValue); }

            var result = right(_rightValue);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TOut>(result);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft) { return new Either<TLeft, TOut>(_leftValue); }

            var result = await right(_rightValue).ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TOut>(result);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = left(_leftValue);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = right(_rightValue);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute  // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            if (IsLeft)
            {
                var leftResult = await left(_leftValue).ConfigureAwait(false);
                Assume(leftResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
                return new Either<TLeftOut, TRightOut>(leftResult);
            }

            var rightResult = await right(_rightValue).ConfigureAwait(false);
            Assume(rightResult != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeftOut, TRightOut>(rightResult);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left).Pipe(Either.Join);
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
        public Task<Either<TOut, TRight>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOut, TRight>>> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left).Map(Either.Join);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(right).Pipe(Either.Join);
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
        public Task<Either<TLeft, TOut>> Bind<TOut>(
            [NotNull, InstantHandle] Func<TRight, Task<Either<TLeft, TOut>>> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(right).Map(Either.Join);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left, right).Pipe(Either.Collapse);
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
        public Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Either<TOutLeft, TOutRight>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left, right).Map(Either.Collapse);
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
        public Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Either<TOutLeft, TOutRight>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left, right).Map(Either.Collapse);
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
        public Task<Either<TOutLeft, TOutRight>> Bind<TOutLeft, TOutRight>(
            [NotNull, InstantHandle] Func<TLeft, Task<Either<TOutLeft, TOutRight>>> left,
            [NotNull, InstantHandle] Func<TRight, Task<Either<TOutLeft, TOutRight>>> right)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }
            if (right == null) { throw new ArgumentNullException(nameof(right)); }
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return Map(left, right).Map(Either.Collapse);
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? left(state, _leftValue)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsRight
                ? right(state, _rightValue)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Combines the provided seed state with the Left value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Left value of this instance.</param>
        /// <param name="left">
        /// A function to invoke asynchronously with the seed value and the Left value of this instance
        /// as the arguments if this instance is in the Left state.
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsLeft
                ? await left(state, _leftValue).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>Combines the provided seed state with the Right value of this instance, asynchronously.</summary>
        /// <typeparam name="TState">The type of the seed value.</typeparam>
        /// <param name="state">The seed value to be combined with the Right value of this instance.</param>
        /// <param name="right">
        /// A function to invoke asynchronously with the seed value and the Right value of this instance
        /// as the arguments if this instance is in the Right state.
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
            if (State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = IsRight
                ? await right(state, _rightValue).ConfigureAwait(false)
                : state;
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Tap

        /// <summary>
        /// Performs an action on the Left value of this instance,
        /// if present, and returns the same value as this instance.
        /// </summary>
        /// <param name="left">An action to perform.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null" />.</exception>
        [MustUseReturnValue]
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(_leftValue);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns the same value as this instance.
        /// </summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null" />.</exception>
        [MustUseReturnValue]
        public Either<TLeft, TRight> Tap([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            { // note(cosborn) Remember, `IsLeft` and `IsRight` can both be false.
                right(_rightValue);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Left value of this instance,
        /// if present, and returns the same value as this instance, asynchronously.
        /// </summary>
        /// <param name="left">An action to perform asynchronously.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null" />.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> Tap([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }

            return this;
        }

        /// <summary>
        /// Performs an action on the Right value of this instance,
        /// if present, and returns the same value as this instance, asynchronously.
        /// </summary>
        /// <param name="right">An action to perform asynchronously.</param>
        /// <returns>The same value as this instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null" />.</exception>
        [NotNull, MustUseReturnValue]
        public async Task<Either<TLeft, TRight>> Tap([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            { // note(cosborn) Remember, `IsLeft` and `IsRight` can both be false.
                await right(_rightValue).ConfigureAwait(false);
            }

            return this;
        }

        #endregion

        #region Let

        /// <summary>Performs an action on the Left value of this instance.</summary>
        /// <param name="left">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TLeft> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                left(_leftValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        public Unit Let([NotNull, InstantHandle] Action<TRight> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                right(_rightValue);
            }

            return Unit.Value;
        }

        /// <summary>Performs an action on the Left value of this instance, asynchronously.</summary>
        /// <param name="left">An action to perform asynchronously.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Let([NotNull, InstantHandle] Func<TLeft, Task> left)
        {
            if (left == null) { throw new ArgumentNullException(nameof(left)); }

            if (IsLeft)
            {
                await left(_leftValue).ConfigureAwait(false);
            }
        }

        /// <summary>Performs an action on the Right value of this instance.</summary>
        /// <param name="right">An action to perform.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [NotNull]
        public async Task Let([NotNull, InstantHandle] Func<TRight, Task> right)
        {
            if (right == null) { throw new ArgumentNullException(nameof(right)); }

            if (IsRight)
            {
                await right(_rightValue).ConfigureAwait(false);
            }
        }

        #endregion

        #region Recover

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">An alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Either<TLeft, TRight> Recover([NotNull] TRight recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            return IsRight
                ? this
                : new Either<TLeft, TRight>(recoverer);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state.</summary>
        /// <param name="recoverer">A function producing an alternate value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is the result of invoking <paramref name="recoverer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure]
        public Either<TLeft, TRight> Recover([NotNull, InstantHandle] Func<TRight> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = recoverer();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return new Either<TLeft, TRight>(result);
        }

        /// <summary>Provides an alternate value in the case that this instance is not in the Right state, asynchronously.</summary>
        /// <param name="recoverer">A function producing an alternate value, asynchronously.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value is the original Right value
        /// of this instance if this instance is in the Right state; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state whose Right value is the value of invoking <paramref name="recoverer"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="recoverer"/> is <see langword="null"/>.</exception>
        [Pure, NotNull]
        public async Task<Either<TLeft, TRight>> Recover([NotNull, InstantHandle] Func<Task<TRight>> recoverer)
        {
            if (recoverer == null) { throw new ArgumentNullException(nameof(recoverer)); }

            if (IsRight) { return this; }

            var result = await recoverer().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #region Value

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the default value of <typeparamref name="TRight"/>.
        /// </returns>
        /// <remarks>
        /// <para>This method is unsafe, as it can return <see langword="null"/>
        /// if <typeparamref name="TRight"/> satisfies <see langword="class"/>.</para>
        /// </remarks>
        [CanBeNull, Pure]
        public TRight GetValueOrDefault() => _rightValue;

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state.
        /// </summary>
        /// <param name="other">An alternative value.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, <paramref name="other"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, Pure]
        public TRight GetValueOrDefault([NotNull] TRight other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return IsRight
                ? _rightValue
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
        [NotNull, Pure]
        public TRight GetValueOrDefault([NotNull, InstantHandle] Func<TRight> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? _rightValue
                : other();
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        /// <summary>
        /// Unwraps this instance with an alternative value
        /// if this instance is not in the Right state, asynchronously.
        /// </summary>
        /// <param name="other">A function producing an alternative value asynchronously.</param>
        /// <returns>
        /// The Right value of this instance if this instance is in the Right state;
        /// otherwise, the result of invoking <paramref name="other"/> asynchronously.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull, Pure]
        public async Task<TRight> GetValueOrDefault([NotNull, InstantHandle] Func<Task<TRight>> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var result = IsRight
                ? _rightValue
                : await other().ConfigureAwait(false);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion
    }
}
