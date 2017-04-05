using JetBrains.Annotations;
using static JetBrains.Annotations.ImplicitUseTargetFlags;

namespace Tiger.Types
{
    /// <content>Debugger proxy representation.</content>
    public partial struct Option<TSome>
    {
        [UsedImplicitly(WithMembers)]
        sealed class DebuggerTypeProxy
        {
            readonly Option<TSome> _value;

            /// <summary>Initializes a new instance of the <see cref="DebuggerTypeProxy"/> class.</summary>
            /// <param name="value">The optional value to be proxied.</param>
            public DebuggerTypeProxy(Option<TSome> value)
            {
                _value = value;
            }

            /// <summary>Gets the internal value of the <see cref="Option{TSome}"/>.</summary>
            [NotNull]
            public TSome Value => _value._value;

            /// <summary>Gets the internal state of the <see cref="Option{TSome}"/>.</summary>
            [NotNull]
            public string State => _value.IsSome ? @"Some" : @"None";
        }
    }
}
