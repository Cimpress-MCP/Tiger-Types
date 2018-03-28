using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to the functionality of <see cref="Task{TResult}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class TaskExtensionsTests
    {
        [Property(DisplayName = "Replace completes its task, then replaces the result value.")]
        public static async Task Replace(Task<int> value, NonEmptyString sentinel)
        {
            var actual = await value.Replace(sentinel.Get).ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Map completes its task, then invokes its func with the task's return value as a parameter.")]
        public static async Task Map(Task<string> value, Func<string, int> mapper)
        {
            var actual = await value.Map(mapper).ConfigureAwait(false);
            var expected = mapper(await value.ConfigureAwait(false));

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName= "Bind completes its task, then invokes its task with the previous task's return value as a parameter.")]
        public static async Task Bind(Task<string> value, Func<string, Task<int>> binder)
        {
            var actual = await value.Bind(binder).ConfigureAwait(false);
            var expected = await binder(await value.ConfigureAwait(false)).ConfigureAwait(false);

            Assert.Equal(expected, actual);
        }
    }
}
