using FsCheck;
using FsCheck.Xunit;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to interface implementations of <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "A None Option does not iterate.")]
        static void GetEnumerator_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            foreach (var v in Option<string>.None)
            {
                actual = sentinel.Get;
            }

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "A Some Option iterates.")]
        static void GetEnumerator_Some(NonNull<string> some, NonNull<string> before)
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
