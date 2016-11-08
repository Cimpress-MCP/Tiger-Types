using System.ComponentModel;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A None state that can be cast to an <see cref="Option{TSome}"/> of any type.</summary>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    public struct OptionNone
    {
        #region Overrides

        #region object

        /// <inheritdoc />
        [NotNull, Pure]
        public override string ToString() => "None";

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => 0;

        #endregion

        #endregion

        #region Implementations

        [NotNull, Pure, PublicAPI]
        object ToDump() => new
        {
            State = "None"
        };

        #endregion
    }
}
