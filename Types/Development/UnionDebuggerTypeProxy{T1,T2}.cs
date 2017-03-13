using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// A simple view of the union value that the debugger
    /// can show to the developer.
    /// </summary>
    /// <typeparam name="T1">The first type of the union value.</typeparam>
    /// <typeparam name="T2">The second type of the union value.</typeparam>
    [PublicAPI]
    sealed class UnionDebuggerTypeProxy<T1, T2>
    {
        /// <summary>Gets an internal value of the <see cref="Union{T1,T2}"/>.</summary>
        [CanBeNull]
        public T1 Value1 => _value.Value1;

        /// <summary>Gets an internal state of the <see cref="Union{T1,T2}"/>.</summary>
        [CanBeNull]
        public T2 Value2 => _value.Value2;

        /// <summary>Gets the internal state of the <see cref="Union{T1,T2}"/>.</summary>
        public int State => _value.State;

        readonly Union<T1, T2> _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnionDebuggerTypeProxy{T1,T2}"/> class.
        /// </summary>
        /// <param name="value">The union value to be proxied.</param>
        public UnionDebuggerTypeProxy(Union<T1, T2> value)
        {
            _value = value;
        }
    }
}
