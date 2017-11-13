// <copyright file="Option{TSome}.IEnumerable.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Types
{
    /// <content>Implementation of <see cref="IEnumerable{T}"/> (kind of).</content>
    public partial struct Option<TSome>
    {
        /// <summary>Returns an enumerator that iterates through the <see cref="Option{TSome}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="Option{TSome}"/>.</returns>
        [Pure, EditorBrowsable(Never)]
        public IEnumerator<TSome> GetEnumerator()
        { // note(cosborn) OK, it's kind of an implementation.
            if (IsSome)
            {
                yield return _value;
            }
        }
    }
}
