// <copyright file="EitherState.cs" company="Cimpress, Inc.">
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

namespace Tiger.Types
{
    /// <summary>Represents the allowable states of an <see cref="Either{TLeft,TRight}"/>.</summary>
    enum EitherState
        : byte // todo(cosborn) Does this save anything?  How does it pack?
    {
        /// <summary>An uninitialized state; represents an error condition.</summary>
        Bottom, // note(cosborn) Must be 0 in the case of default(Either<TLeft, TRight>).

        /// <summary>The Left state; usually represents a bad case.</summary>
        Left,

        /// <summary>The Right state; usually represents a good case.</summary>
        Right
    }
}
