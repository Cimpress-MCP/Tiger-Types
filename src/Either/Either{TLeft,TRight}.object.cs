using System.Collections.Generic;
using JetBrains.Annotations;
using static System.Globalization.CultureInfo;
using static Tiger.Types.EitherState;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
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

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Either<TLeft, TRight> either && EqualsCore(either);

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
