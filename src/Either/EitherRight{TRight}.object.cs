// <copyright file="EitherRight{TRight}.object.cs" company="Cimpress, Inc.">
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
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using static System.Globalization.CultureInfo;
using static Tiger.Types.EitherState;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Prevent boxing.")]
    public partial struct EitherRight<TRight>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() =>
            string.Format(InvariantCulture, @"Right({0})", Value);

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) => obj is EitherRight<TRight> eitherRight && EqualsCore(eitherRight);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Value.GetHashCode();

        [Pure]
        bool EqualsCore(EitherRight<TRight> other) =>
            EqualityComparer<TRight>.Default.Equals(Value, other.Value);

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = Right,
            Value
        };
    }
}
