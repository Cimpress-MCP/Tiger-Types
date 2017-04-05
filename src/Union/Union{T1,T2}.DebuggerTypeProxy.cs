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
