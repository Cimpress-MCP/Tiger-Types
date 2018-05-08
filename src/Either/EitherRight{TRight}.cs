// <copyright file="EitherRight{TRight}.cs" company="Cimpress, Inc.">
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
    /// <summary>A partially applied <see cref="Either{TLeft,TRight}"/> in the Right state.</summary>
    /// <typeparam name="TRight">The applied Right type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Type system isn't rich enough to prove this.")]
    public readonly struct EitherRight<TRight>
    {
        /// <summary>Initializes a new instance of the <see cref="EitherRight{TRight}"/> struct.</summary>
        /// <param name="value">The value to wrap.</param>
        internal EitherRight([NotNull] TRight value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            Value = value;
        }

        /// <summary>Gets the internal value of this instance.</summary>
        internal TRight Value { get; }

        /// <summary>Compare two instances of <see cref="EitherRight{TRight}"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="EitherRight{TRight}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherRight{TRight}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(EitherRight<TRight> left, EitherRight<TRight> right) => left.EqualsCore(right);

        /// <summary>Compare two instances of <see cref="EitherRight{TRight}"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="EitherRight{TRight}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherRight{TRight}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(EitherRight<TRight> left, EitherRight<TRight> right) => !(left == right);

        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => $"Right({Value})";

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) => obj is EitherRight<TRight> eitherRight && EqualsCore(eitherRight);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Value.GetHashCode();

        [Pure]
        bool EqualsCore(in EitherRight<TRight> other) => EqualityComparer<TRight>.Default.Equals(Value, other.Value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = Right,
            Value
        };
    }
}
