﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A partially-applied <see cref="Either{TLeft,TRight}"/> in the Left state.</summary>
    /// <typeparam name="TLeft">The applied Left type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    public struct EitherLeft<TLeft>
    {
        /// <summary>Gets the internal value of this instance.</summary>
        internal readonly TLeft Value;

        /// <summary>Initializes a new instance of the <see cref="EitherLeft{TLeft}"/> struct.</summary>
        /// <param name="value">The value to be wrapped.</param>
        internal EitherLeft([NotNull] TLeft value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            Value = value;
        }

        [NotNull, Pure, PublicAPI]
        object ToDump() => new
        {
            State = EitherState.Left,
            Value
        };
    }
}