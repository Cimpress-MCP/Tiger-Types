using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>
    /// Exception thrown when an <see cref="Tiger.Types.Either{TLeft,TRight}"/>
    /// is unexpectedly not in the Right state.
    /// </summary>
    public class RightException
        : AssertActualExpectedException
    {
        /// <summary>Initializes a new instance of the <see cref="RightException"/> class.</summary>
        public RightException()
            : base("Right", "Left", "The result was not in the expected state.", "Expected State", "Actual State")
        {

        }
    }
}