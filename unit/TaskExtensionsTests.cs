using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    public static class TaskExtensionsTests
    {
        [Property(DisplayName = "Apply completes its task, then invokes its func.")]
        public static void Apply(NonNull<string> sentinel)
        {
            // act
            var actual = CompletedTask.Apply(() => sentinel.Get).Result;

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Then completes its task, then invokes its task.")]
        public static void Then(NonNull<string> sentinel)
        {
            // act
            var actual = CompletedTask.Then(() => FromResult(sentinel.Get)).Result;

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Map completes its task, then invokes its func with the task's " +
            "return value as a parameter.")]
        public static void Map(NonNull<string> sentinel)
        {
            // act
            var actual = FromResult(sentinel.Get).Map(v => v.Length).Result;

            // assert
            Assert.Equal(sentinel.Get.Length, actual);
        }

        [Property(DisplayName= "Bind complete its task, then invokes its task with the previous task's " +
            "return value as a parameter.")]
        public static void Bind(NonNull<string> sentinel)
        {
            // arrange

            // act
            var actual = FromResult(sentinel.Get).Bind(v => FromResult(v.Length)).Result;

            // assert
            Assert.Equal(sentinel.Get.Length, actual);
        }
    }
}
