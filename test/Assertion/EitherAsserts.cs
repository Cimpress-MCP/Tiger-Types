using System.Diagnostics.CodeAnalysis;
using Tiger.Types;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>XUnit assertions specialized for <see cref="Either{TLeft,TRight}"/>.</summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Assert
    {
        /// <summary>Verifies that a given either is in the Right state.</summary>
        /// <typeparam name="TLeft">The Left type.</typeparam>
        /// <typeparam name="TRight">The Right type.</typeparam>
        /// <param name="either">The either.</param>
        /// <returns>The Right value of the either.</returns>
        /// <exception cref="RightException">The either is not in the Right state.</exception>
        public static TRight Right<TLeft, TRight>(Either<TLeft, TRight> either)
        {
            if (!either.IsRight)
            {
                throw new RightException();
            }
            return either.Value;
        }

        /// <summary>Verifies that a given either is in the Left state.</summary>
        /// <typeparam name="TLeft">The Left type.</typeparam>
        /// <typeparam name="TRight">The Right type.</typeparam>
        /// <param name="either">The either.</param>
        /// <returns>The Left value of the either.</returns>
        /// <exception cref="LeftException">The either is not in the Left state.</exception>
        public static TLeft Left<TLeft, TRight>(Either<TLeft, TRight> either)
        {
            if (!either.IsLeft)
            {
                throw new LeftException();
            }

            return (TLeft)either;
        }
    }
}
