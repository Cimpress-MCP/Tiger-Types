using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;
using static Tiger.Types.EitherState;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Either{TLeft,TRight}"/>.</summary>
    [PublicAPI]
    public static class Either
    {
        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Left state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Left state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new Either<TLeft, TRight>(leftValue);
        }

        /// <summary>Creates an <see cref="EitherLeft{TLeft}"/> from the provided value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="leftValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="EitherLeft{TLeft}"/> that can be converted to an <see cref="Either{TLeft,TRight}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="leftValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static EitherLeft<TLeft> Left<TLeft>([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            return new EitherLeft<TLeft>(leftValue);
        }

        /// <summary>
        /// Creates an <see cref="Either{TLeft,TRight}"/> in the Right state from the provided value.
        /// </summary>
        /// <typeparam name="TLeft">
        /// The Left type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> in the Right state.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new Either<TLeft, TRight>(rightValue);
        }

        /// <summary>Creates an <see cref="EitherRight{TRight}"/> from the provided value.</summary>
        /// <typeparam name="TRight">
        /// The Right type of the <see cref="Either{TLeft,TRight}"/> to be created.
        /// </typeparam>
        /// <param name="rightValue">
        /// The value from which to create the <see cref="Either{TLeft,TRight}"/>.
        /// </param>
        /// <returns>
        /// An <see cref="EitherRight{TRight}"/> that can be converted to an <see cref="Either{TLeft,TRight}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="rightValue"/> is <see langword="null"/>.</exception>
        [Pure]
        public static EitherRight<TRight> Right<TRight>([NotNull] TRight rightValue)
        {
            if (rightValue == null) { throw new ArgumentNullException(nameof(rightValue)); }

            return new EitherRight<TRight>(rightValue);
        }

        #region Extensions

        /// <summary>
        /// Converts an <see cref="Either{TLeft,TRight}"/> into an <see cref="Option{TSome}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The Left type of <paramref name="eitherValue"/>.</typeparam>
        /// <typeparam name="TRight">The Right type of <paramref name="eitherValue"/>.</typeparam>
        /// <param name="eitherValue">The value to be converted.</param>
        /// <returns>
        /// An <see cref="Option{TSome}"/> in the Some state with its Some value set to the
        /// Right value of <paramref name="eitherValue"/> if <paramref name="eitherValue"/> is in the
        /// Right state; otherwise, an <see cref="Option{TSome}"/> in the None state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Option<TRight> ToOption<TLeft, TRight>(this Either<TLeft, TRight> eitherValue)
        {
            if (eitherValue.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return eitherValue.Map(v => new Option<TRight>(v)).GetValueOrDefault();
        }

        /// <summary>Joins one layer of <see cref="Either{TLeft,TRight}"/> from a value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of <paramref name="eitherEitherValue"/> and
        /// the Left type of the Left type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of the Right type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <param name="eitherEitherValue">The value to be joined.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state whose Left value
        /// is the Left value of this instance if this instance is in the Left state,
        /// or the Right value of this instance if this instance is in the Right state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Either<TLeft, TRight> Join<TLeft, TRight>(
            this Either<TLeft, Either<TLeft, TRight>> eitherEitherValue) =>
            eitherEitherValue.Match(
                left: l => new Either<TLeft, TRight>(l),
                right: r => r);

        /// <summary>Joins one layer of <see cref="Either{TLeft,TRight}"/> from a value.</summary>
        /// <typeparam name="TLeft">
        /// The Left type of the Left type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <typeparam name="TRight">
        /// The Right type of <paramref name="eitherEitherValue"/> and
        /// the Right type of the Right type of <paramref name="eitherEitherValue"/>.
        /// </typeparam>
        /// <param name="eitherEitherValue">The value to be joined.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state whose Right value
        /// is the Right value of this instance if this instance is in the Right state,
        /// or the Left value of this instance if this instance is in the Left state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public static Either<TLeft, TRight> Join<TLeft, TRight>(
            this Either<Either<TLeft, TRight>, TRight> eitherEitherValue) =>
            eitherEitherValue.Match(
                left: l => l,
                right: r => new Either<TLeft, TRight>(r));

        /// <summary>
        /// Collapses an <see cref="Either{TLeft,TRight}"/> whose Left and Right types
        /// match into a value, based on its state.
        /// </summary>
        /// <typeparam name="TSame">The Left and Right type of <paramref name="eitherValue"/>.</typeparam>
        /// <param name="eitherValue">The value to be collapsed.</param>
        /// <returns>
        /// The Left value of this instance if this instance is in the Left state, or
        /// the Right value of this instance if this instance is in the Right state.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [NotNull]
        internal static TSame Collapse<TSame>(this Either<TSame, TSame> eitherValue) =>
            eitherValue.Match(
                left: l => l,
                right: r => r);

        #region LINQ

        /// <summary>Determines whether the either value contains a right value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Either{TLeft,TRight}"/> to check for a Right state.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a right value;
        /// otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Any<TLeftSource, TRightSource>(this Either<TLeftSource, TRightSource> source)
        {
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.IsRight;
        }

        /// <summary>
        /// Determines whether an either value contains a right value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Either{TLeft,TRight}"/> whose Right value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Right value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Right value if the source either value passes the test
        /// in the specified predicate; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Any<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, bool> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.IsRight && predicate(source.Value);
        }

        /// <summary>
        /// Determines whether an either value contains a right value that satisfies a condition.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The <see cref="Either{TLeft,TRight}"/> whose Right value to apply the predicate to.
        /// </param>
        /// <param name="predicate">A function to test the Right value for a condition.</param>
        /// <returns>
        /// <see langword="true"/> if the Right value of the source either value passes the test
        /// in the specified predicate, or if the source either is in the Left state;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool All<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, bool> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.IsLeft || source.IsRight && predicate(source.Value);
        }

        /// <summary>
        /// Determines whether an either value contains a specified right value
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value in which to locate a value.</param>
        /// <param name="value">The value to locate in the either value.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a Right value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Contains<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Any(v => EqualityComparer<TRightSource>.Default.Equals(v, value));
        }

        /// <summary>
        /// Determines whether an either value contains a specified right value
        /// by using a specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value in which to locate a value.</param>
        /// <param name="value">The value to locate in the either value.</param>
        /// <param name="equalityComparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <see langword="true"/> if the either value contains a Right value that has the specified value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure]
        public static bool Contains<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource value,
            [CanBeNull] IEqualityComparer<TRightSource> equalityComparer)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Any(v => (equalityComparer ?? EqualityComparer<TRightSource>.Default).Equals(v, value));
        }

        /// <summary>
        /// Returns the specified either value or the default value of <typeparamref name="TRightSource"/>
        /// as an either value if the either value is not in the Right state.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The either value to return a default value for if it is not in the Right state.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with the default value for
        /// the <typeparamref name="TRightSource"/> type as the Right value
        /// if <paramref name="source"/> is not in the Right state; otherwise, <paramref name="source"/>.
        /// </returns>
        [Pure, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> DefaultIfEmpty<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source)
            where TRightSource : struct => source.Recover(default(TRightSource));

        /// <summary>
        /// Returns the specified either value or the specified value as an either value
        /// if the either value is not in the Right state.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The either value to return a default value for if it is not in the Right state.
        /// </param>
        /// <param name="defaultValue">The value to return if the either value is not in the Right state.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Right state with <paramref name="defaultValue"/>
        /// as the Right value if <paramref name="source"/> is not in the Right state;
        /// otherwise, <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultValue"/> is <see langword="null"/>.</exception>
        [Pure, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> DefaultIfEmpty<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull] TRightSource defaultValue)
        {
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            return source.Recover(defaultValue);
        }

        /// <summary>Invokes an action on the Right value of an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>The original value, exhibiting the specified side effects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TRightSource> Do<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Action<TRightSource> onNext)
        {
            if (onNext == null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Tap(onNext);
        }

        /// <summary>Invokes an action on the Right value of an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <param name="source">An either value on which to perform an action.</param>
        /// <param name="onNext">An action to invoke.</param>
        /// <returns>A unit.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <see langword="null"/>.</exception>
        [EditorBrowsable(Never)]
        public static Unit ForEach<TLeftSource, TRightSource>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Action<TRightSource> onNext)
        {
            if (onNext == null) { throw new ArgumentNullException(nameof(onNext)); }

            return source.Let(onNext);
        }

        /// <summary>Projects the Right value of an either value into a new form.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An either value to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to the Right value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking
        /// the transform function on the Right value of <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> Select<TLeftSource, TRightSource, TResult>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, TResult> selector)
        {
            if (selector == null) { throw new ArgumentNullException(nameof(selector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Map(selector);
        }

        /// <summary>
        /// Projects the Right value of an either value to an <see cref="Either{TLeft,TRight}"/>
        /// and flattens the resulting either-either value into an either value.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The Right type of the either value returned by <paramref name="selector"/>.
        /// </typeparam>
        /// <param name="source">An either value to project.</param>
        /// <param name="selector">A transform function to apply to the Right value.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking the
        /// right-to-either transform function on the Right value of the input either value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> SelectMany<TLeftSource, TRightSource, TResult>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, Either<TLeftSource, TResult>> selector)
        {
            if (selector == null) { throw new ArgumentNullException(nameof(selector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Bind(selector);
        }

        /// <summary>
        /// Projects the Right value of an either value to an <see cref="Either{TLeft,TRight}"/>,
        /// flattens the resulting either-either value into an either value,
        /// and invokes a result selector function on the Right value therein.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TEither">
        /// The Right type of the intermediate either value
        /// collected by <paramref name="eitherSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">The Right type of the resulting either value.</typeparam>
        /// <param name="source">An either value to project.</param>
        /// <param name="eitherSelector">
        /// A transform function to apply to the Right value of the input either value.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to the Right value of the intermediate either value.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> whose Right value is the result of invoking
        /// the right-to-either transform function <paramref name="eitherSelector"/> on the Right value
        /// of <paramref name="source"/> and then mapping that Right value and their corresponding
        /// either value to a result either value.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="eitherSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, LinqTunnel, EditorBrowsable(Never)]
        public static Either<TLeftSource, TResult> SelectMany<TLeftSource, TRightSource, TEither, TResult>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull, InstantHandle] Func<TRightSource, Either<TLeftSource, TEither>> eitherSelector,
            [NotNull, InstantHandle] Func<TRightSource, TEither, TResult> resultSelector)
        {
            if (eitherSelector == null) { throw new ArgumentNullException(nameof(eitherSelector)); }
            if (resultSelector == null) { throw new ArgumentNullException(nameof(resultSelector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Bind(sv => source.Bind(eitherSelector).Map(cv => resultSelector(sv, cv)));
        }

        /// <summary>Applies an accumulator function over an either value.</summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An <see cref="Either{TLeft,TRight}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on the Right value.</param>
        /// <returns>The final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TAccumulate Aggregate<TLeftSource, TRightSource, TAccumulate>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TRightSource, TAccumulate> func)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            return source.Fold(seed, func);
        }

        /// <summary>
        /// Applies an accumulator function over an either value.
        /// The specified seed value is used as the initial accumulator value,
        /// and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TLeftSource">The Left type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TRightSource">The Right type of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An <see cref="Either{TLeft,TRight}"/> to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to be invoked on the Right value.</param>
        /// <param name="resultSelector">
        /// A function to transform the final accumulator value into the result value.
        /// </param>
        /// <returns>The transformed final return accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="seed"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        /// <exception cref="InvalidOperationException">This result evaluated to <see langword="null"/>.</exception>
        [Pure, NotNull, EditorBrowsable(Never)]
        public static TResult Aggregate<TLeftSource, TRightSource, TAccumulate, TResult>(
            this Either<TLeftSource, TRightSource> source,
            [NotNull] TAccumulate seed,
            [NotNull, InstantHandle] Func<TAccumulate, TRightSource, TAccumulate> func,
            [NotNull, InstantHandle] Func<TAccumulate, TResult> resultSelector)
        {
            if (seed == null) { throw new ArgumentNullException(nameof(seed)); }
            if (func == null) { throw new ArgumentNullException(nameof(func)); }
            if (resultSelector == null) { throw new ArgumentNullException(nameof(resultSelector)); }
            if (source.State == Bottom) { throw new InvalidOperationException(EitherIsBottom); }

            var result = source.Fold(seed, func).Pipe(resultSelector);
            Assume(result != null, ResultIsNull); // ReSharper disable once AssignNullToNotNullAttribute
            return result;
        }

        #endregion

        #endregion

        #region Split

        /// <summary>Splits a value into an either value based on a provided condition.</summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static Either<TValue, TValue> Split<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return splitter(value)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to <paramref name="value"/> if the condition <paramref name="splitter"/> is
        /// satisfied by <paramref name="value"/>; otherwise, an <see cref="Either{TLeft,TRight}"/>
        /// in the Right state with its Right value set to <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static async Task<Either<TValue, TValue>> Split<TValue>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return await splitter(value).ConfigureAwait(false)
                ? Right<TValue, TValue>(value)
                : Left<TValue, TValue>(value);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TValue"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        public static Either<TOut, TOut> Split<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter,
            [NotNull, InstantHandle] Func<TValue, TOut> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return Split(value, splitter).Map(
                left: mapper,
                right: mapper);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">A condition by which <paramref name="value"/> can be split.</param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TValue"/>
        /// to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> Split<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, bool> splitter,
            [NotNull, InstantHandle] Func<TValue, Task<TOut>> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            return Split(value, splitter).Map(
                left: mapper,
                right: mapper);
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <param name="mapper">
        /// A transformation from <typeparamref name="TValue"/> to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> Split<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter,
            [NotNull, InstantHandle] Func<TValue, TOut> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            // todo(cosborn) I'm starting to regret skipping bi-mappable T-versions.
            return Split(value, splitter).Map(ev => ev.Map(
                left: mapper,
                right: mapper));
        }

        /// <summary>
        /// Splits a value into an either value based on a provided condition
        /// then maps it unconditionally, asynchronously.
        /// </summary>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <typeparam name="TOut">The return type of <paramref name="mapper"/>.</typeparam>
        /// <param name="value">A value to test for a condition.</param>
        /// <param name="splitter">
        /// An asynchronous condition by which <paramref name="value"/> can be split.
        /// </param>
        /// <param name="mapper">
        /// An asynchronous transformation from <typeparamref name="TValue"/>
        /// to <typeparamref name="TOut"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Either{TLeft,TRight}"/> in the Left state with its Left value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>
        /// if the condition <paramref name="splitter"/> is satisfied by <paramref name="value"/>;
        /// otherwise, an <see cref="Either{TLeft,TRight}"/> in the Right state with its Right value
        /// set to the result of transforming <paramref name="value"/> with <paramref name="mapper"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="splitter"/> is <see langword="null"/>.</exception>
        [NotNull]
        public static Task<Either<TOut, TOut>> Split<TValue, TOut>(
            [NotNull] TValue value,
            [NotNull, InstantHandle] Func<TValue, Task<bool>> splitter,
            [NotNull, InstantHandle] Func<TValue, Task<TOut>> mapper)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (splitter == null) { throw new ArgumentNullException(nameof(splitter)); }

            // todo(cosborn) I'm starting to regret skipping bi-mappable T-versions.
            return Split(value, splitter).Bind(ev => ev.Map(
                left: mapper,
                right: mapper));
        }

        #endregion
    }
}
