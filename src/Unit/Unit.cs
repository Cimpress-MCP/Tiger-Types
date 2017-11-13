// <copyright file="Unit.cs" company="Cimpress, Inc.">
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
        public static readonly Unit Value;
    }
}
