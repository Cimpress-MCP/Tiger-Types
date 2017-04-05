using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;
using static System.Runtime.InteropServices.LayoutKind;

namespace Tiger.Types
{
    /// <content>Implementation of <see cref="IEnumerable{T}"/> (kind of).</content>
    public partial struct Either<TLeft, TRight>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <returns>An <see cref="Enumerator"/> for the <see cref="Either{TLeft,TRight}"/>.</returns>
        [Pure, EditorBrowsable(Never)]
        public Enumerator GetEnumerator() =>
            new Enumerator(this); // note(cosborn) OK, it's kind of an implementation.

        /// <summary>Enumerates the either value of a <see cref="Either{TLeft, TRight}"/>.</summary>
        [EditorBrowsable(Never)]
        [StructLayout(Auto)]
        public struct Enumerator
        {
            readonly Either<TLeft, TRight> _eitherValue;

            bool _moved;

            /// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
            /// <param name="eitherValue">The either value to be enumerated.</param>
            internal Enumerator(Either<TLeft, TRight> eitherValue)
                : this()
            {
                _eitherValue = eitherValue;
            }

            /// <summary>Gets the value at the current position of the enumerator.</summary>
            public TRight Current => _eitherValue._rightValue;

            /// <summary>Advances the enumerator to the Some value of the <see cref="Option{TSome}"/>.</summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was successfully advanced to the Some value;
            /// otherwise, <see langword="false"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (_moved) { return false; }
                _moved = true;
                return _eitherValue.IsRight;
            }
        }
    }
}
