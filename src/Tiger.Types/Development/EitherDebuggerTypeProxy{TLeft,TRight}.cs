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
        public TLeft LeftValue => _value.LeftValue;

        /// <summary>Gets an internal value of the <see cref="Either{TLeft,TRight}"/>.</summary>
        [CanBeNull]
        public TRight RightValue => _value.RightValue;

        /// <summary>Gets the internal state of the <see cref="Either{TLeft,TRight}"/>.</summary>
        public string State => _value.State.ToString();

        readonly Either<TLeft, TRight> _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="EitherDebuggerTypeProxy{TLeft,TRight}"/> class.
        /// </summary>
        /// <param name="value">The either value to be proxied.</param>
        public EitherDebuggerTypeProxy(Either<TLeft, TRight> value)
        {
            _value = value;
        }
    }
}
