// <copyright file="EitherLeft{TLeft}.Operators.cs" company="Cimpress, Inc.">
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

namespace Tiger.Types
{
    /// <content>Operators and named alternatives.</content>
    public partial struct EitherLeft<TLeft>
    {
        /// <summary>Compare two instances of <see cref="EitherLeft{TLeft}"/> for equality.</summary>
        /// <param name="left">The left instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are equal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(EitherLeft<TLeft> left, EitherLeft<TLeft> right) => left.EqualsCore(right);

        /// <summary>Compare two instances of <see cref="EitherLeft{TLeft}"/> for inequality.</summary>
        /// <param name="left">The left instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <param name="right">The right instance of <see cref="EitherLeft{TLeft}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two instances are unequal,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(EitherLeft<TLeft> left, EitherLeft<TLeft> right) => !(left == right);
    }
}
