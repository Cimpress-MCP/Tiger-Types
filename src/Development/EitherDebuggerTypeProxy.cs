using JetBrains.Annotations;

namespace Tiger.Types
{
    [PublicAPI]
    sealed class EitherDebuggerTypeProxy<TLeft, TRight>
    {
        /// <summary>An internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
        [CanBeNull]
        TLeft LeftValue => _value.LeftValue;

        /// <summary>An internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
        [CanBeNull]
        TRight RightValue => _value.RightValue;

        /// <summary>The internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
        string State => _value.State.ToString();

        readonly Either<TLeft, TRight> _value;

        EitherDebuggerTypeProxy(Either<TLeft, TRight> value)
        {
            _value = value;
        }
    }
}