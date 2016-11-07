using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>
    /// Exception thrown when an <see cref="Tiger.Types.Option{TSome}"/>
    /// is unexpectedly not in the Some state.
    /// </summary>
    public class SomeException
        : AssertActualExpectedException
    {
        /// <summary>Initializes a new instance of the <see cref="SomeException"/> class.</summary>
        public SomeException()
            : base("Some", "None", "The result was not in the expected state.", "Expected State", "Actual State")
        {

        }
    }
}
