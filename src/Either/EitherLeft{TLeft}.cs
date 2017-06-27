// <copyright file="EitherLeft{TLeft}.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    public partial struct EitherLeft<TLeft>
    {
        /// <summary>Initializes a new instance of the <see cref="EitherLeft{TLeft}"/> struct.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [SuppressMessage("Style", "IDE0016:Use 'throw' expression", Justification = "Analyzer bug.")]
        internal EitherLeft([NotNull] TLeft value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            Value = value;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TLeft Value { get; }
    }
}
