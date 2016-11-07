using System.ComponentModel;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A None state that can be cast to an <see cref="Option{TSome}"/> of any type.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [StructLayout(Auto)]
    public struct OptionNone
    {
        [NotNull, Pure, PublicAPI]
        object ToDump() => new
        {
            State = "None"
        };
    }
}
