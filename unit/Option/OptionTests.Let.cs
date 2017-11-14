using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Option{TSome}"/>.</context>
    public static partial class OptionTests
    {
        [Property(DisplayName = "Conditionally executing an action based on a None Option does not execute.")]
        static void ActionLet_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            Option<string>.None.Let(v => actual = sentinel.Get);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        static void ActionLet_Some(NonNull<string> some, NonNull<string> before)
        {
            var actual = before.Get;
            Option.From(some.Get).Let(v => actual = v);

            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing a task based on a None Option does not execute.")]
        static async Task TaskLet_None(NonNull<string> before, NonNull<string> sentinel)
        {
            var actual = before.Get;
            await Option<string>.None
                .Let(v => Run(() => actual = sentinel.Get))
                .ConfigureAwait(false);

            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing a task based on a Some Option executes.")]
        static async Task TaskLet_Some(NonNull<string> some, NonNull<string> before)
        {
            var actual = before.Get;
            await Option.From(some.Get)
                .Let(v => Run(() => actual = v))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }
    }
}
