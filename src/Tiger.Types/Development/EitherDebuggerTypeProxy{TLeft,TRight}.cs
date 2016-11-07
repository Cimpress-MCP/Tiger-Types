using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// A simple view of the either value that the debugger
    /// can show to the developer.
    /// </summary>
    /// <typeparam name="TLeft">The Left type of the either value.</typeparam>
    /// <typeparam name="TRight">The Right type of the either value.</typeparam>
    [PublicAPI]
    sealed class EitherDebuggerTypeProxy<TLeft, TRight>
    {
        /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
        [CanBeNull]
        TLeft LeftValue => _value.LeftValue;

        /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
        [CanBeNull]
        TRight RightValue => _value.RightValue;

        /// <summary>Gets the internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
        string State => _value.State.ToString();

        readonly Either<TLeft, TRight> _value;

        EitherDebuggerTypeProxy(Either<TLeft, TRight> value)
        {
            _value = value;
        }
    }
}
