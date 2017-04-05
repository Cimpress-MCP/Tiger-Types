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
