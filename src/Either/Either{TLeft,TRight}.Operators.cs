// <copyright file="Either{TLeft,TRight}.Operators.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using static Tiger.Types.Resources;

namespace Tiger.Types
{
    /// <content>Operators and named alternatives.</content>
    public partial struct Either<TLeft, TRight>
    {
        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="leftValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TLeft leftValue) => leftValue == null
            ? default(Either<TLeft, TRight>)
            : new Either<TLeft, TRight>(leftValue);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="rightValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>([CanBeNull] TRight rightValue) => rightValue == null
            ? default(Either<TLeft, TRight>)
            : new Either<TLeft, TRight>(rightValue);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="leftValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>(EitherLeft<TLeft> leftValue) =>
            new Either<TLeft, TRight>(leftValue.Value);

        /// <summary>Wraps a value in <see cref="Either{TLeft,TRight}"/>.</summary>
        /// <param name="rightValue">The value to be wrapped.</param>
        public static implicit operator Either<TLeft, TRight>(EitherRight<TRight> rightValue) =>
            new Either<TLeft, TRight>(rightValue.Value);

        /// <summary>Unwraps the Right value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        public static explicit operator TRight(Either<TLeft, TRight> value)
        {
            if (!value.IsRight) { throw new InvalidOperationException(EitherIsNotRight); }

            return value._rightValue;
        }

        /// <summary>Unwraps the Left value of this instance.</summary>
        /// <param name="value">The value to be unwrapped.</param>
        /// <exception cref="InvalidOperationException">This instance is in an invalid state.</exception>
        [NotNull]
        public static explicit operator TLeft(Either<TLeft, TRight> value)
        {
            if (!value.IsLeft) { throw new InvalidOperationException(EitherIsNotLeft); }

            return value._leftValue;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.EqualsCore(right);

        /// <summary>Indicates whether the current object is not equal to another object of the same type.</summary>
        /// <param name="left">An object to compare with <paramref name="right"/>.</param>
        /// <param name="right">An object to compare with <paramref name="left"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to the <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            !(left == right);
    }
}
