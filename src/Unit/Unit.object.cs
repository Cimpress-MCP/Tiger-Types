using System.ComponentModel;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
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
