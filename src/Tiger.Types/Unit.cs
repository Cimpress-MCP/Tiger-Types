using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>Represents a type with one possible value.</summary>
    [StructLayout(Auto)]
    public struct Unit
    {
        /// <summary>
        /// Gets the single value of the type <see cref="Unit"/>.
        /// </summary>
        public static readonly Unit Value = default(Unit);

        #region Overrides

        #region object

        /// <inheritdoc />
        [NotNull, Pure, EditorBrowsable(Never)]
        public override string ToString() => "()";

        /// <inheritdoc />
        [Pure, EditorBrowsable(Never)]
        public override bool Equals(object obj) => obj is Unit;

        /// <inheritdoc />
        [Pure, EditorBrowsable(Never)]
        public override int GetHashCode() => 1;

        #endregion

        #endregion

        #region Implementations

        [NotNull, Pure, PublicAPI]
        object ToDump() => new { };

        #endregion

        #region Operators and Named Alternatives

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

        #endregion
    }
}
