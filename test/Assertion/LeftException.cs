using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>
    /// Exception thrown when an <see cref="Tiger.Types.Either{TLeft,TRight}"/>
    /// is unexpectedly not in the Left state.
    /// </summary>
    public class LeftException
        : AssertActualExpectedException
    {
        /// <summary>Initializes a new instance of the <see cref="LeftException"/> class.</summary>
        public LeftException()
            : base("Left", "Right", "The result was not in the expected state.", "Expected State", "Actual State")
        {

        }
    }
}
