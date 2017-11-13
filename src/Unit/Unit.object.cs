// <copyright file="Unit.object.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Prevent boxing.")]
    public partial struct Unit
    {
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
    }
}
