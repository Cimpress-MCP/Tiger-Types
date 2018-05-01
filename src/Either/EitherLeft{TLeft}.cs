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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;
using static Tiger.Types.EitherState;

namespace Tiger.Types
{
    /// <summary>A partially applied <see cref="Either{TLeft,TRight}"/> in the Left state.</summary>
    /// <typeparam name="TLeft">The applied Left type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct EitherLeft<TLeft>
    {
        /// <summary>Initializes a new instance of the <see cref="EitherLeft{TLeft}"/> struct.</summary>
        /// <param name="leftValue">The value to wrap.</param>
        internal EitherLeft([NotNull] TLeft leftValue)
        {
            if (leftValue == null) { throw new ArgumentNullException(nameof(leftValue)); }

            Value = leftValue;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TLeft Value { get; }

        #region Operators

        /// <summary>Compare two instances of <see cref="EitherLeft{TLeft}"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(EitherLeft<TLeft> left, EitherLeft<TLeft> right) => left.EqualsCore(right);

        /// <summary>Compare two instances of <see cref="EitherLeft{TLeft}"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(EitherLeft<TLeft> left, EitherLeft<TLeft> right) => !(left == right);

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => $"Left({Value})";

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is EitherLeft<TLeft> eitherLeft && EqualsCore(eitherLeft);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(Value);

        [Pure]
        bool EqualsCore(in EitherLeft<TLeft> other) =>
            EqualityComparer<TLeft>.Default.Equals(Value, other.Value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = Left,
            Value
        };

        #endregion

        #endregion
    }
}
