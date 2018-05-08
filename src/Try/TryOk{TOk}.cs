// <copyright file="TryOk{TOk}.cs" company="Cimpress, Inc.">
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Diagnostics.Contracts.Contract;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A partially applied <see cref="Try{TErr, TOk}"/> in the Ok state.</summary>
    /// <typeparam name="TOk">The applied Ok type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct TryOk<TOk>
    {
        /// <summary>Initializes a new instance of the <see cref="TryOk{TOk}"/> struct.</summary>
        /// <param name="okValue">The value to wrap.</param>
        internal TryOk([NotNull] TOk okValue)
        {
            Assume(okValue != null, "Attempted to initialize TryOk with null value.");

            Value = okValue;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TOk Value { get; }

        #region Operators

        /// <summary>Compare two instances of <see cref="TryOk{TOk}"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="TryOk{TOk}"/>.</param>
        /// <param name="right">The right instance of <see cref="TryOk{TOk}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(TryOk<TOk> left, TryOk<TOk> right) => left.EqualsCore(right);

        /// <summary>Compare two instances of <see cref="TryOk{TOk}"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="TryOk{TOk}"/>.</param>
        /// <param name="right">The right instance of <see cref="TryOk{TOk}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(TryOk<TOk> left, TryOk<TOk> right) => !(left == right);

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => $"Ok({Value})";

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is TryOk<TOk> tryOk && EqualsCore(tryOk);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => EqualityComparer<TOk>.Default.GetHashCode(Value);

        [Pure]
        bool EqualsCore(in TryOk<TOk> other) =>
            EqualityComparer<TOk>.Default.Equals(Value, other.Value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = "Ok",
            Value
        };

        #endregion

        #endregion
    }
}
