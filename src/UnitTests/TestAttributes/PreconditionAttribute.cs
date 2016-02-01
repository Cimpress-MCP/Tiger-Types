using System;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    /// <summary>Attribute to mark a test method as belonging to the category Precondition.</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PreconditionAttribute
        : CategoryAttribute
    {
    }
}
