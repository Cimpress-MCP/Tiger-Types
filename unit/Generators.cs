using System;
using System.Collections.Generic;
using FsCheck;

namespace Tiger.Types.UnitTest
{
    public static class UnequalNonNullPair
    {
        public static Arbitrary<UnequalNonNullPair<T>> Generate<T>()
            where T : class => Arb.Default.NonNull<T>().Generator
            .Two()
            .Select(t => (fst: t.Item1.Get, snd: t.Item2.Get))
            .Where(t => !EqualityComparer<T>.Default.Equals(t.fst, t.snd))
            .ToArbitrary()
            .Convert(t => new UnequalNonNullPair<T>(t.fst, t.snd), unnp => (unnp.Left, unnp.Right));
    }

    public struct UnequalNonNullPair<T>
        where T : class
    {
        public T Left { get; }
        public T Right { get; }

        public UnequalNonNullPair(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }

    public static class UnequalPair
    {
        public static Arbitrary<UnequalPair<T>> Generate<T>() =>
            Arb.Generate<T>()
            .Two()
            .Where(t => !EqualityComparer<T>.Default.Equals(t.Item1, t.Item2))
            .ToArbitrary()
            .Convert(t => new UnequalPair<T>(t.Item1, t.Item2), unnp => Tuple.Create(unnp.Left, unnp.Right));
    }

    public struct UnequalPair<T>
    {
        public T Left { get; }
        public T Right { get; }

        public UnequalPair(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }
}