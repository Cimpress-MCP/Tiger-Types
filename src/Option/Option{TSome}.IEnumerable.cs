using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Implementation of <see cref="IEnumerable{T}"/> (kind of).</content>
    public partial struct Option<TSome>
    {
        /// <summary>Returns an enumerator that iterates through the <see cref="Option{TSome}"/>.</summary>
        /// <returns>An <see cref="Enumerator"/> for the <see cref="Option{TSome}"/>.</returns>
        [Pure, EditorBrowsable(Never)]
        public Enumerator GetEnumerator() =>
            new Enumerator(this); // note(cosborn) OK, it's kind of an implementation.

        /// <summary>Enumerates the optional value of a <see cref="Option{TSome}"/>.</summary>
        [EditorBrowsable(Never)]
        public struct Enumerator
        {
            readonly Option<TSome> _optionValue;

            bool _moved;

            /// <summary>Initializes a new instance of the <see cref="Enumerator"/> struct.</summary>
            /// <param name="optionValue">The optional value to be enumerated.</param>
            internal Enumerator(Option<TSome> optionValue)
                : this()
            {
                _optionValue = optionValue;
            }

            /// <summary>Gets the value at the current position of the enumerator.</summary>
            public TSome Current => _optionValue._value;

            /// <summary>Advances the enumerator to the Some value of the <see cref="Option{TSome}"/>.</summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was successfully advanced to the Some value;
            /// otherwise, <see langword="false"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (_moved) { return false; }
                _moved = true;
                return _optionValue.IsSome;
            }
        }
    }
}
