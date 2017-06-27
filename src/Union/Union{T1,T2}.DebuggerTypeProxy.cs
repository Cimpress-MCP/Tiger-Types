// <copyright file="Union{T1,T2}.DebuggerTypeProxy.cs" company="Cimpress, Inc.">
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
    public partial class Union<T1, T2>
    {
        [UsedImplicitly(WithMembers)]
        sealed class DebuggerTypeProxy
        {
            readonly Union<T1, T2> _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.
            /// </summary>
            /// <param name="value">The union value to be proxied.</param>
            public DebuggerTypeProxy(Union<T1, T2> value)
            {
                _value = value;
            }

            /// <summary>Gets an internal value of the <see cref="Union{T1,T2}"/>.</summary>
            [CanBeNull]
            public T1 Value1 => _value.Value1;

            /// <summary>Gets an internal state of the <see cref="Union{T1,T2}"/>.</summary>
            [CanBeNull]
            public T2 Value2 => _value.Value2;

            /// <summary>Gets the internal state of the <see cref="Union{T1,T2}"/>.</summary>
            public int State => _value._state;
        }
    }
}
