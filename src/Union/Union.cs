// <copyright file="Union.cs" company="Cimpress, Inc.">
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

namespace Tiger.Types
{
    /// <summary>Explicitly creates instances of <see cref="Union{T1,T2}"/>.</summary>
    public static class Union
    {
        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
        /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A <see cref="Union{T1,T2}"/> containing <paramref name="value"/>.</returns>
        [NotNull]
        public static Union<T1, T2> From<T1, T2>([NotNull] T1 value) => Union<T1, T2>.From(value);

        /// <summary>Creates a <see cref="Union{T1,T2}"/> from the provided value.</summary>
        /// <typeparam name="T1">The first type of the value that is a composite.</typeparam>
        /// <typeparam name="T2">The second type of the value that is a composite.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A <see cref="Union{T1,T2}"/> containing <paramref name="value"/>.</returns>
        [NotNull]
        public static Union<T1, T2> From<T1, T2>([NotNull] T2 value) => Union<T1, T2>.From(value);
    }
}
