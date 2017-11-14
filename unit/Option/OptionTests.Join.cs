using FsCheck.Xunit;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to joining <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Joining a None Option Option returns a None Option.")]
        static void Join_None()
        {
            var actual = Option<Option<int>>.None.Join();

            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Joining a Some Option containing a None Option returns a None Option.")]
        static void Join_SomeNone()
        {
            var actual = Option.From(Option<int>.None).Join();

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Joining a Some Option containing a Some Option returns a Some Option.")]
        static void Join_SomeSome(int some)
        {
            var actual = Option.From(Option.From(some)).Join();

            Assert.True(actual.IsSome);
            Assert.Equal(some, actual.Value);
        }
    }
}
