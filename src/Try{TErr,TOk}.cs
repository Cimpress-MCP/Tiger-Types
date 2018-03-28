// <copyright file="Try{TErr,TOk}.cs" company="Cimpress, Inc.">
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
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>Represents a value that can represent one of two values, or none.</summary>
    /// <typeparam name="TErr">The Error type of the value that may be represented.</typeparam>
    /// <typeparam name="TOk">The OK type of the value that may be represented.</typeparam>
    [StructLayout(Auto)]
    [SuppressMessage("Microsoft:Guidelines", "CA1066", Justification = "Prevent boxing.")]
    public readonly struct Try<TErr, TOk>
    {
        readonly Option<Either<TErr, TOk>> _value;

        #region Operators

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Try<TErr, TOk> left, Try<TErr, TOk> right)
        {
            return left.Equals(right);
        }

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Try<TErr, TOk> left, Try<TErr, TOk> right)
        {
            return !(left == right);
        }

        #endregion

        #region Overrides

        #region object

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Try<TErr, TOk> @try && EqualsCore(@try);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>Compares this instance with another instance for equality.</summary>
        /// <param name="other">Another instance.</param>
        /// <returns>
        /// <see langword="true"/> if this instance and <paramref name="other"/> are equal;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        internal bool EqualsCore(Try<TErr, TOk> other) => _value.EqualsCore(other._value);

        #endregion

        #endregion
    }
}
