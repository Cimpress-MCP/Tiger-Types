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
        [Property(DisplayName = "Map completes its task, then invokes its func with the task's return value as a parameter.")]
        public static async Task Map(NonNull<string> sentinel)
        {
            var actual = await FromResult(sentinel.Get)
                .Map(v => v.Length)
                .ConfigureAwait(false);

            Assert.Equal(sentinel.Get.Length, actual);
        }

        [Property(DisplayName= "Bind completes its task, then invokes its task with the previous task's return value as a parameter.")]
        public static async Task Bind(NonNull<string> sentinel)
        {
            var actual = await FromResult(sentinel.Get)
                .Bind(v => FromResult(v.Length))
                .ConfigureAwait(false);
            
            Assert.Equal(sentinel.Get.Length, actual);
        }
    }
}
