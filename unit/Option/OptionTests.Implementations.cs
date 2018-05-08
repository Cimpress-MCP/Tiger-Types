using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to interface implementations of <see cref="Option{TSome}"/>.</summary>
    public static partial class OptionTests
    {
        [Property(DisplayName = "A None Option does not iterate.")]
        public static void GetEnumerator_None(NonEmptyString before, NonEmptyString sentinel)
        {
            var actual = before.Get;
            foreach (var v in Option<string>.None)
            {
                actual = sentinel.Get;
            }

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "A Some Option iterates.")]
        public static void GetEnumerator_Some(NonEmptyString some, NonEmptyString before)
        {
            var actual = before.Get;
            foreach (var v in Option.From(some.Get))
            {
                actual = v;
            }

            Assert.Equal(some.Get, actual);
        }
    }
}
