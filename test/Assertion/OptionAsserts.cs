using System.Diagnostics.CodeAnalysis;
using Tiger.Types;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>XUnit assertions specialized for <see cref="Option{TSome}"/>.</summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Assert
    {
        /// <summary>Verifies that a given option is in the Some state.</summary>
        /// <typeparam name="TSome">The Some type.</typeparam>
        /// <param name="option">The option.</param>
        /// <returns>The Some value of the option.</returns>
        /// <exception cref="SomeException">The option is not in the Some state.</exception>
        public static TSome Some<TSome>(Option<TSome> option)
        {
            if (!option.IsSome)
            {
                throw new SomeException();
            }
            return option.Value;
        }

        /// <summary>Verifies that a given option is in the None state.</summary>
        /// <typeparam name="TSome">The Some type.</typeparam>
        /// <param name="option">The option.</param>
        /// <exception cref="NoneException">The result is not in the None state.</exception>
        // ReSharper disable once UnusedParameter.Global
        public static void None<TSome>(Option<TSome> option)
        {
            if (!option.IsNone)
            {
                throw new NoneException();
            }
        }
    }
}
