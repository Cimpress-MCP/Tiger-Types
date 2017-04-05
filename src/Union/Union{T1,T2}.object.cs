using JetBrains.Annotations;
using static System.Globalization.CultureInfo;

namespace Tiger.Types
{
    /// <content>Overrides and override-equivalents.</content>
    public partial class Union<T1, T2>
    {
        /// <inheritdoc/>
        [NotNull, Pure]
        public override string ToString() => Match(
            one: o => string.Format(InvariantCulture, "One({0})", o),
            two: t => string.Format(InvariantCulture, "Two({0})", t)) ?? string.Empty;

        /// <inheritdoc/>
        [Pure]
        public override int GetHashCode() => Match(
            one: o => o.GetHashCode(),
            two: t => t.GetHashCode());

        /// <inheritdoc/>
        [Pure]
        public override bool Equals(object obj) =>
            obj is Union<T1, T2> union && EqualsCore(union);

        [Pure]
        bool EqualsCore([NotNull] Union<T1, T2> other)
        {
            if (_state != other._state) { return false; }

            switch (_state)
            {
                case 1:
                    return _value1.Equals(other._value1);
                case 2:
                    return _value2.Equals(other._value2);
                default: // because(cosborn) Hush, ReSharper.
                    return false;
            }
        }

        [NotNull, Pure, UsedImplicitly]
        object ToDump() => Match<object>(
            one: o => new { State = 1, Value = o },
            two: t => new { State = 2, Value = t }) ?? new { };
    }
}
