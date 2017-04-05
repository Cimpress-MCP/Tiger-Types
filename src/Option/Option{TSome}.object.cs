using JetBrains.Annotations;
using static System.Globalization.CultureInfo;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    public partial struct Option<TSome>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => IsNone
            ? @"None"
            : string.Format(InvariantCulture, @"Some({0})", _value);

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => IsNone
            ? 0
            : _value.GetHashCode();

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Option<TSome> option && EqualsCore(option);

        [Pure]
        bool EqualsCore(Option<TSome> other)
        {
            if (IsNone && other.IsNone) { return true; }
            if (IsNone || other.IsNone) { return false; }

            // note(cosborn) Implicitly `IsSome && other.IsSome`.
            return _value.Equals(other._value);
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            none: new { State = "None" },
            some: v => new { State = "Some", Value = v });
    }
}
