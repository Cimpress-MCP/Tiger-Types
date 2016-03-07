using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// A simple view of the optional value that the debugger
    /// can show to the developer.
    /// </summary>
    /// <typeparam name="TSome">The Some type of the optional value.</typeparam>
    [PublicAPI]
    sealed class OptionDebuggerTypeProxy<TSome>
    {
        /// <summary>The internal value of the <see cref="Option{TSome}"/>.</summary>
        [CanBeNull]
        TSome Value => _value.SomeValue;

        /// <summary>The internal state of the <see cref="Option{TSome}"/>.</summary>
        [NotNull]
        string State => _value.IsSome ? "Some" : "None";

        readonly Option<TSome> _value;

        OptionDebuggerTypeProxy(Option<TSome> value)
        {
            _value = value;
        }
    }
}