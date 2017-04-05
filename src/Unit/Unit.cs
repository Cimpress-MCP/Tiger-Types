using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>Represents a type with one possible value.</summary>
    [PublicAPI]
    [StructLayout(Auto)]
    public partial struct Unit
    {
        /// <summary>
        /// Gets the single value of the type <see cref="Unit"/>.
        /// </summary>
        public static readonly Unit Value = default(Unit);
    }
}
