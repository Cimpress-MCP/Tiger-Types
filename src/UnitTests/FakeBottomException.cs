// ReSharper disable All

using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Tiger.Types.UnitTests
{
    [Serializable]
    sealed class FakeBottomException
        : BottomException
    {
        public FakeBottomException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
