using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FsCheck;
using JetBrains.Annotations;
using static JetBrains.Annotations.ImplicitUseTargetFlags;

namespace Tiger.Types.UnitTest.Utility
{
    /// <summary>Generators for unit test data.</summary>
    [UsedImplicitly(Members)]
    public static class Generators
    {
        /// <summary>
        /// Generates an arbitrary instance of <typeparamref name="T"/>, wrapped in <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<Task<T>> TaskValue<T>() => Arb.Generate<T>()
            .Select(Task.FromResult)
            .ToArbitrary();

        /// <summary>Generates an arbitrary instance of <see cref="Option{TSome}"/>.</summary>
        /// <typeparam name="TSome">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<Option<TSome>> OptionValue<TSome>() => Arb.From(
            Gen.Frequency(
                Tuple.Create(24, Arb.Generate<TSome>().Where(t => t != null).Select(Option.From)),
                Tuple.Create(1, Gen.Constant(Option<TSome>.None))));

        public static Arbitrary<Either<TLeft, TRight>> EitherValue<TLeft, TRight>() => Arb.From(
            Gen.Frequency(
                Tuple.Create(50, Arb.Generate<TLeft>().Where(t => t != null).Select(Either<TLeft, TRight>.From)),
                Tuple.Create(50, Arb.Generate<TRight>().Where(t => t != null).Select(Either<TLeft, TRight>.From))));

        /// <summary>Generates an arbitrary instance of <see cref="Try{TErr, TOk}"/>.</summary>
        /// <typeparam name="TErr">The Err type of the value to generate.</typeparam>
        /// <typeparam name="TOk">The Ok type of the value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<Try<TErr, TOk>> TryValue<TErr, TOk>() => Arb.From(
            Gen.Frequency(
                Tuple.Create(49, Arb.Generate<TErr>().Where(t => t != null).Select(Try<TErr, TOk>.From)),
                Tuple.Create(49, Arb.Generate<TOk>().Where(t => t != null).Select(Try<TErr, TOk>.From)),
                Tuple.Create(2, Gen.Constant(Try<TErr, TOk>.None))));

        /// <summary>Generates an arbitrary instance of <see cref="UnequalNonNullPair{T}"/>.</summary>
        /// <typeparam name="T">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<UnequalNonNullPair<T>> UnequalNonNullPair<T>()
            where T : class => Arb.Generate<NonNull<T>>()
            .Two().Select(t => (left: t.Item1.Get, right: t.Item2.Get))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalNonNullPair<T>(t), unnp => (unnp.Left, unnp.Right));

        /// <summary>Generates an arbitrary instance of <see cref="UnequalPair{T}"/>.</summary>
        /// <typeparam name="T">The type of value to generate.</typeparam>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<UnequalPair<T>> UnequalPair<T>() => Arb.Generate<T>()
            .Two().Select(t => (left: t.Item1, right: t.Item2))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalPair<T>(t), up => (up.Left, up.Right));

        /// <summary>Generates an arbitrary instance of <see cref="System.Version"/>.</summary>
        /// <returns>An arbitrary value.</returns>
        public static Arbitrary<Version> Version() => Arb.Generate<byte>()
            .Four().Select(t => new Version(t.Item1, t.Item2, t.Item3, t.Item4))
            .ToArbitrary();
    }
}
