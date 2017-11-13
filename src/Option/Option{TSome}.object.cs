// <copyright file="Option{TSome}.object.cs" company="Cimpress, Inc.">
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

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using static System.Globalization.CultureInfo;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Prevent boxing.")]
    public partial struct Option<TSome>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => IsNone
            ? @"None"
            : string.Format(InvariantCulture, @"Some({0})", _value);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => IsNone
            ? 0
            : _value.GetHashCode();

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Option<TSome> option && EqualsCore(option);

        [Pure]
        bool EqualsCore(Option<TSome> other)
        {
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return _value.Equals(other._value);
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            none: new { State = "None" },
            some: v => new { State = "Some", Value = v });
    }
}
