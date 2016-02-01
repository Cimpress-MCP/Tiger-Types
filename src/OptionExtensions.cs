using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>Extensions to the functionality of <see cref="object"/>.</summary>
    [PublicAPI]
    [DebuggerStepThrough]
    public static class OptionExtensions
    {
        /// <summary>Converts an <see cref="Option{T}"/> into a <see cref="Nullable{T}"/>.</summary>
        /// <typeparam name="T">The Some type of <paramref name="value"/>.</typeparam>
        /// <param name="value">THe value to be converted.</param>
        /// <returns>
        /// The Some value of <paramref name="value"/> if it is in the Some state;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        [CanBeNull]
        public static T? ToNullable<T>(this Option<T> value)
            where T : struct => value.Match(
                none: () => (T?)null,
                some: v => v);

        #region LINQ

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Option<TResult> Select<TSource, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, TResult> selector) => source.Map(selector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Option<T> Where<T>(
            this Option<T> source,
            [NotNull, InstantHandle] Func<T, bool> predicate) => source.Match(
                none: Option.None, 
                some: v => predicate(v) ? source : Option.None);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCollection"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="collectionSelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static Option<TResult> SelectMany<TSource, TCollection, TResult>(
            this Option<TSource> source,
            [NotNull, InstantHandle] Func<TSource, Option<TCollection>> collectionSelector,
            [NotNull, InstantHandle] Func<TSource, TCollection, TResult> resultSelector) =>
                source.Bind(sv => source.Bind(collectionSelector).Map(cv => resultSelector(sv, cv)));

        #endregion
    }
}
