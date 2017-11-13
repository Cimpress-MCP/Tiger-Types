using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    public static class TaskExtensionsTests
    {
        [Property(DisplayName = "Apply completes its task, then invokes its func.")]
        public static async Task Apply(NonNull<string> sentinel)
        {
            var actual = await CompletedTask.Apply(() => sentinel.Get);

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Then completes its task, then invokes its task.")]
        public static async Task Then(NonNull<string> sentinel)
        {
            var actual = await CompletedTask.Then(() => FromResult(sentinel.Get));

            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Map completes its task, then invokes its func with the task's return value as a parameter.")]
        public static async Task Map(NonNull<string> sentinel)
        {
            var actual = await FromResult(sentinel.Get).Map(v => v.Length);

            Assert.Equal(sentinel.Get.Length, actual);
        }

        [Property(DisplayName= "Bind complete its task, then invokes its task with the previous task's return value as a parameter.")]
        public static async Task Bind(NonNull<string> sentinel)
        {
            var actual = await FromResult(sentinel.Get).Bind(v => FromResult(v.Length));

            Assert.Equal(sentinel.Get.Length, actual);
        }
    }
}
