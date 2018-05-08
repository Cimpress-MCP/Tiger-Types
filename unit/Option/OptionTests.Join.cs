using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to joining <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Fact(DisplayName = "Joining a None Option Option returns a None Option.")]
        public static void Join_None()
        {
            var actual = Option<Option<int>>.None.Join();

            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Joining a Some Option containing a None Option returns a None Option.")]
        public static void Join_SomeNone()
        {
            var actual = Option.From(Option<int>.None).Join();

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Joining a Some Option containing a Some Option returns a Some Option.")]
        public static void Join_SomeSome(int some)
        {
            var actual = Option.From(Option.From(some)).Join();

            Assert.True(actual.IsSome);
            Assert.Equal(some, actual.Value);
        }
    }
}
