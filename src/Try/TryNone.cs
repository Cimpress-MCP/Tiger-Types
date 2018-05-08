// <copyright file="TryNone.cs" company="Cimpress, Inc.">
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
    /// <summary>A None state that can be cast to an <see cref="Option{TSome}"/> of any type.</summary>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct TryNone
    {
        #region Operators

        /// <summary>Compare two instances of <see cref="TryNone"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="TryNone"/>.</param>
        /// <param name="right">The right instance of <see cref="TryNone"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        [SuppressMessage("Roslynator", "RCS1163", Justification = "All instances are equal.")]
        public static bool operator ==(TryNone left, TryNone right) => true;

        /// <summary>Compare two instances of <see cref="TryNone"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="TryNone"/>.</param>
        /// <param name="right">The right instance of <see cref="TryNone"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(TryNone left, TryNone right) => !(left == right);

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => "None";

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) => obj is TryNone;

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => 0;

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new { State = "None" };

        #endregion

        #endregion
    }
}
