using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Tiger.Types
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BottomException
        : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BottomException"/>.
        /// </summary>
        public BottomException()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BottomException"/>
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BottomException([CanBeNull] string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BottomException"/>
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BottomException([CanBeNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BottomException"/>
        /// with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected BottomException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
