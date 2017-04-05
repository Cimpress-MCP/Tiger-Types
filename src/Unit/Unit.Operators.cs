using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Operators and named alternatives.</content>
    public partial struct Unit
    {
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [EditorBrowsable(Never)]
        [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "All units are equal.")]
        public static bool operator ==(Unit left, Unit right) => true;

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [EditorBrowsable(Never)]
        [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "All units are equal.")]
        public static bool operator !=(Unit left, Unit right) => false;
    }
}
