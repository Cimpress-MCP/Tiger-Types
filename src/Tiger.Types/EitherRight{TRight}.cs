using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <summary>A partially-applied <see cref="Either{TLeft,TRight}"/> in the Right state.</summary>
    /// <typeparam name="TRight">The applied Right type.</typeparam>
    [EditorBrowsable(Never)]
    [StructLayout(Auto)]
    public struct EitherRight<TRight>
    {
        /// <summary>Gets the internal value of this instance.</summary>
        internal readonly TRight Value;

        /// <summary>Initializes a new instance of the <see cref="EitherRight{TRight}"/> struct.</summary>
        /// <param name="value">The value to be wrapped.</param>
        internal EitherRight([NotNull] TRight value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            Value = value;
        }

        #region Overrides

        #region object

        /// <inheritdoc />
        [NotNull, Pure]
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, @"Right({0})", Value);

        /// <inheritdoc />
        [Pure]
        public override int GetHashCode() => Value.GetHashCode();

        #endregion

        #endregion

        #region Implementations

        [NotNull, Pure, PublicAPI]
        object ToDump() => new
        {
            State = EitherState.Right,
            Value
        };

        #endregion
    }
}
