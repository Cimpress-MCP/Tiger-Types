using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to extensions to the functionality of <see cref="Task{TResult}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class TaskExtensionsTests
    {
        [Property(DisplayName = "Replacing the value of a null task throws.")]
        public static async Task Replace_NullTask(NonEmptyString sentinel)
        {
            var actual = await Record.ExceptionAsync(() => TaskExtensions.Replace<int, string>(null, sentinel.Get))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("taskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Replacing the value of a task with null throws.")]
        public static async Task Replace_NullReplacement(Task<int> value)
        {
            var actual = await Record.ExceptionAsync(() => value.Replace<int, string>(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("replacement", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Replace completes its task, then replaces the result value.")]
        public static async Task Replace(Task<int> value, NonEmptyString sentinel)
        {
            var actual = await value.Replace(sentinel.Get).ConfigureAwait(false);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Mapping the value of a null task throws.")]
        public static async Task Map_NullTask(Func<string, int> mapper)
        {
            var actual = await Record.ExceptionAsync(() => TaskExtensions.Map(null, mapper))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("taskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Replacing the value of a task with null throws.")]
        public static async Task Map_NullMapper(Task<string> value)
        {
            var actual = await Record.ExceptionAsync(() => value.Map<string, int>(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Map completes its task, then invokes its func with the task's return value as a parameter.")]
        public static async Task Map(Task<string> value, Func<string, int> mapper)
        {
            var actual = await value.Map(mapper).ConfigureAwait(false);
            var expected = mapper(await value.ConfigureAwait(false));

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Binding the value of a null task throws.")]
        public static async Task Bind_NullTask(Func<string, Task<int>> binder)
        {
            var actual = await Record.ExceptionAsync(() => TaskExtensions.Bind(null, binder))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("taskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Binding the value of a task over null throws.")]
        public static async Task Bind_NullBinder(Task<string> value)
        {
            var actual = await Record.ExceptionAsync(() => value.Bind<string, int>(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("binder", ane.Message, Ordinal);
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
