using System.ComponentModel;
using System.Runtime.InteropServices;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A None state that can be cast to an <see cref="Option{TSome}"/> of any type.</summary>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    public partial struct OptionNone
    {
    }
}
