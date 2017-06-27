// <copyright file="Either{TLeft,TRight}.DebuggerTypeProxy.cs" company="Cimpress, Inc.">
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

using JetBrains.Annotations;
using static JetBrains.Annotations.ImplicitUseTargetFlags;

namespace Tiger.Types
{
    /// <content>Debugger proxy representation.</content>
    public partial struct Either<TLeft, TRight>
    {
        [UsedImplicitly(WithMembers)]
        sealed class DebuggerTypeProxy
        {
            readonly Either<TLeft, TRight> _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.
            /// </summary>
            /// <param name="value">The either value to be proxied.</param>
            public DebuggerTypeProxy(Either<TLeft, TRight> value)
            {
                _value = value;
            }

            /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
            [CanBeNull]
            public TLeft LeftValue => _value._leftValue;

            /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
            [CanBeNull]
            public TRight RightValue => _value._rightValue;

            /// <summary>Gets the internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
            public string State => _value.State.ToString();
        }
    }
}
