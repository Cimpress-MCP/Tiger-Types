// <copyright file="Either{TLeft,TRight}.object.cs" company="Cimpress, Inc.">
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
    public partial struct Either<TLeft, TRight>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString()
        {
            switch (State)
            {
                case Left:
                    return string.Format(InvariantCulture, @"Left({0})", _leftValue);
                case Right:
                    return string.Format(InvariantCulture, @"Right({0})", _rightValue);
                case Bottom:
                    return @"Bottom";
                default: // note(cosborn) Why would you change this enum???
                    return string.Empty;
            }
        }

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Either<TLeft, TRight> either && EqualsCore(either);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode()
        {
            switch (State)
            {
                case Left:
                    return _leftValue.GetHashCode();
                case Right:
                    return _rightValue.GetHashCode();
                case Bottom:
                    return 0;
                default: // note(cosborn) Why would you change this enum???
                    return 0;
            }
        }

        [Pure]
        bool EqualsCore(Either<TLeft, TRight> other)
        { // note(cosborn) Eh, this gets gnarly using other implementations.
            if (State == Bottom && other.State == Bottom)
            {
                return true;
            }

            if (IsLeft && other.IsLeft)
            {
                return EqualityComparer<TLeft>.Default.Equals(_leftValue, other._leftValue);
            }

            if (IsRight && other.IsRight)
            {
                return EqualityComparer<TRight>.Default.Equals(_rightValue, other._rightValue);
            }

            // note(cosborn) Implicitly `_state != other._state`.
            return false;
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            left: l => new { State = Left, Value = l },
            right: r => new { State = Right, Value = r });
    }
}
