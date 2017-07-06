using System;
using System.Collections.Generic;
using FsCheck;

namespace Tiger.Types.UnitTest
{
    public static class Generators
    {
        public static Arbitrary<UnequalNonNullPair<T>> UnequalNonNullPair<T>()
            where T : class => Arb.Generate<NonNull<T>>()
            .Two().Select(t => (left: t.Item1.Get, right: t.Item2.Get))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalNonNullPair<T>(t), unnp => (unnp.Left, unnp.Right));

        public static Arbitrary<UnequalPair<T>> UnequalPair<T>() => Arb.Generate<T>()
            .Two().Select(t => (left: t.Item1, right: t.Item2))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.left, t.right))
            .ToArbitrary()
            .Convert(t => new UnequalPair<T>(t), up => (up.Left, up.Right));

        public static Arbitrary<Version> Version() => Arb.Generate<byte>()
            .Four().Select(t => new Version(t.Item1, t.Item2, t.Item3, t.Item4))
            .ToArbitrary();
    }

    public struct UnequalNonNullPair<T>
        where T : class
    {
        public T Left => _values.left;
        public T Right => _values.right;

        readonly (T left, T right) _values;

        public UnequalNonNullPair((T, T) values)
        {
            _values = values;
        }

        // todo(cosborn) Can this be better?
        public override string ToString() => $"UnequalNonNullPair {_values}";
    }

    public static class UnequalNonNullPair
    {
        public static void Deconstruct<T>(this UnequalNonNullPair<T> pair, out T left, out T right)
            where T : class
        {
            left = pair.Left;
            right = pair.Right;
        }
    }

    public struct UnequalPair<T>
    {
        public T Left => _values.left;
        public T Right => _values.right;

        readonly (T left, T right) _values;

        public UnequalPair((T, T) values)
        {
            _values = values;
        }

        // todo(cosborn) Can this be better?
        public override string ToString() => $"UnequalPair {_values}";
    }

    public static class UnequalPair
    {
        public static void Deconstruct<T>(this UnequalPair<T> pair, out T left, out T right)
        {
            left = pair.Left;
            right = pair.Right;
        }
    }
}