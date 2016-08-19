using JetBrains.Annotations;

namespace Tiger.Types
{
    [PublicAPI]
    sealed class UnionDebuggerTypeProxy<T1, T2>
    {
        /// <summary>An internal state of the <see cref="Union{T1,T2}"/>.</summary>
        [CanBeNull]
        T1 Value1 => _value.Value1;

        /// <summary>An internal state of the <see cref="Union{T1,T2}"/>.</summary>
        [CanBeNull]
        T2 Value2 => _value.Value2;

        /// <summary>The internal state of the <see cref="Union{T1,T2}"/>.</summary>
        int State => _value.State;

        readonly Union<T1, T2> _value;

        UnionDebuggerTypeProxy(Union<T1, T2> value)
        {
            _value = value;
        }
    }
}
