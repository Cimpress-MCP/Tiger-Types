// <copyright file="Either{TLeft,TRight}.IEnumerable.cs" company="Cimpress, Inc.">
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
