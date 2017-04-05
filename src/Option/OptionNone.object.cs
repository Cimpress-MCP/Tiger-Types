using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    public partial struct OptionNone
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => "None";

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => 0;

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => new { State = "None" };
    }
}
