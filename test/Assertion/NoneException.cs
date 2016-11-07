using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    /// <summary>
    /// Exception thrown when an <see cref="Tiger.Types.Option{TSome}"/>
    /// is unexpectedly not in the None state.
    /// </summary>
    public class NoneException
        : AssertActualExpectedException
    {
        /// <summary>Initializes a new instance of the <see cref="NoneException"/> class.</summary>
        public NoneException()
            : base("None", "Some", "The result was not in the expected state.", "Expected State", "Actual State")
        {

        }
    }
}
