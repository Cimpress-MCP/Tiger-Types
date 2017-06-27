// <copyright file="Union{T1,T2}.object.cs" company="Cimpress, Inc.">
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

using JetBrains.Annotations;
using static System.Globalization.CultureInfo;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    public partial class Union<T1, T2>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => Match(
            one: o => string.Format(InvariantCulture, "One({0})", o),
            two: t => string.Format(InvariantCulture, "Two({0})", t)) ?? string.Empty;

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Match(
            one: o => o.GetHashCode(),
            two: t => t.GetHashCode());

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Union<T1, T2> union && EqualsCore(union);

        [Pure]
        bool EqualsCore([NotNull] Union<T1, T2> other)
        {
            if (_state != other._state) { return false; }

            switch (_state)
            {
                case 1:
                    return _value1.Equals(other._value1);
                case 2:
                    return _value2.Equals(other._value2);
                default: // because(cosborn) Hush, ReSharper.
                    return false;
            }
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            one: o => new { State = 1, Value = o },
            two: t => new { State = 2, Value = t }) ?? new { };
    }
}
