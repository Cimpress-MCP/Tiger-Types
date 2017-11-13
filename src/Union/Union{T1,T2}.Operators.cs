// <copyright file="Union{T1,T2}.Operators.cs" company="Cimpress, Inc.">
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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Operators and named alternatives.</content>
    [SuppressMessage("Microsoft:Guidelines", "CA2225", Justification = "Parametric types play poorly with this analysis.")]
    public partial class Union<T1, T2>
    {
        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [CanBeNull]
        public static implicit operator Union<T1, T2>([CanBeNull] T1 value) => ToUnion(value);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        [CanBeNull]
        public static implicit operator Union<T1, T2>([CanBeNull] T2 value) => ToUnion(value);

        /// <summary>Unwraps the first value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        [NotNull]
        public static explicit operator T1([NotNull] Union<T1, T2> value) => value.Value1;

        /// <summary>Unwraps the second value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is not in the specified state.</exception>
        [NotNull]
        public static explicit operator T2([NotNull] Union<T1, T2> value) => value.Value2;

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==([CanBeNull] Union<T1, T2> left, [CanBeNull] Union<T1, T2> right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=([CanBeNull] Union<T1, T2> left, [CanBeNull] Union<T1, T2> right) =>
            !(left == right);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns><paramref name="value"/>, wrapped in <see cref="Union{T1,T2}"/>.</returns>
        [CanBeNull, EditorBrowsable(Never)]
        public static Union<T1, T2> ToUnion([CanBeNull] T1 value) => value == null
            ? null
            : new Union<T1, T2>(value);

        /// <summary>Wraps a value in <see cref="Union{T1,T2}"/>.</summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns><paramref name="value"/>, wrapped in <see cref="Union{T1,T2}"/>.</returns>
        [CanBeNull, EditorBrowsable(Never)]
        public static Union<T1, T2> ToUnion([CanBeNull] T2 value) => value == null
            ? null
            : new Union<T1, T2>(value);
    }
}
