using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A partially-applied <see cref="Either{TLeft,TRight}"/> in the Right state.</summary>
    /// <typeparam name="TRight">The applied Right type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    public partial struct EitherRight<TRight>
    {
        /// <summary>Initializes a new instance of the <see cref="EitherRight{TRight}"/> struct.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        internal EitherRight([NotNull] TRight value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            Value = value;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TRight Value { get; }
    }
}
