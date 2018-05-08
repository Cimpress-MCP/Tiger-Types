// <copyright file="TryErr{TErr}.cs" company="Cimpress, Inc.">
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
    /// <summary>A partially applied <see cref="Try{TErr, TOk}"/> in the Err state.</summary>
    /// <typeparam name="TErr">The applied Err type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct TryErr<TErr>
    {
        /// <summary>Initializes a new instance of the <see cref="TryErr{TErr}"/> struct.</summary>
        /// <param name="errValue">The value to wrap.</param>
        internal TryErr([NotNull] TErr errValue)
        {
            Assume(errValue != null, "Attempted to initialize TryErr with null value.");

            Value = errValue;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TErr Value { get; }

        #region Operators

        /// <summary>Compare two instances of <see cref="TryErr{TErr}"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="TryErr{TErr}"/>.</param>
        /// <param name="right">The right instance of <see cref="TryErr{TErr}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(TryErr<TErr> left, TryErr<TErr> right) => left.EqualsCore(right);

        /// <summary>Compare two instances of <see cref="TryErr{TErr}"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="TryErr{TErr}"/>.</param>
        /// <param name="right">The right instance of <see cref="TryErr{TErr}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(TryErr<TErr> left, TryErr<TErr> right) => !(left == right);

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => $"Err({Value})";

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is TryErr<TErr> tryErr && EqualsCore(tryErr);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => EqualityComparer<TErr>.Default.GetHashCode(Value);

        [Pure]
        bool EqualsCore(in TryErr<TErr> other) =>
            EqualityComparer<TErr>.Default.Equals(Value, other.Value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = "Err",
            Value
        };

        #endregion

        #endregion
    }
}
