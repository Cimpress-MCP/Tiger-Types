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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>Represents a type with one possible value.</summary>
    [PublicAPI]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct Unit
    {
        /// <summary>
        /// Gets the single value of the type <see cref="Unit"/>.
        /// </summary>
        public static readonly Unit Value;

        #region Operators

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [EditorBrowsable(Never)]
        [SuppressMessage("Roslynator", "RCS1163", Justification = "All instances are equal.")]
        public static bool operator ==(Unit left, Unit right) => true;

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [EditorBrowsable(Never)]
        [SuppressMessage("Roslynator", "RCS1163", Justification = "All instances are equal.")]
        public static bool operator !=(Unit left, Unit right) => false;

        #endregion

        #region object

        /// <inheritdoc/>
        [NotNull, Pure, EditorBrowsable(Never)]
        public override string ToString() => "()";

        /// <inheritdoc/>
        [Pure, EditorBrowsable(Never)]
        public override bool Equals(object obj) => obj is Unit;

        /// <inheritdoc/>
        [Pure, EditorBrowsable(Never)]
        public override int GetHashCode() => 1;

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new { };

        #endregion
    }
}
