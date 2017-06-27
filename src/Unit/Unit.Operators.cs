// <copyright file="Unit.Operators.cs" company="Cimpress, Inc.">
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
