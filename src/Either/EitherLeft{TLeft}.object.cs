using JetBrains.Annotations;
using static System.Globalization.CultureInfo;
using static Tiger.Types.EitherState;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    // ReSharper disable once UnusedTypeParameter because(cosborn) Analysis bug.
    public partial struct EitherLeft<TLeft>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() =>
            string.Format(InvariantCulture, @"Left({0})", Value);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Value.GetHashCode();

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new
        {
            State = Left,
            Value
        };
    }
}
