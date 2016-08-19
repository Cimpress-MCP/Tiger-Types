namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Union{T1,T2}"/>.</summary>
    public static class Union
    {
        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
        /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A <see cref="Union{T1,T2}"/> containing <paramref name="value"/>.</returns>
        public static Union<T1, T2> From<T1, T2>(T1 value) => Union<T1, T2>.From(value);

        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
        /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A <see cref="Union{T1,T2}"/> containing <paramref name="value"/>.</returns>
        public static Union<T1, T2> From<T1, T2>(T2 value) => Union<T1, T2>.From(value);
    }
}
