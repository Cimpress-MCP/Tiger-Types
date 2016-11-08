// ReSharper disable All

using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;

namespace Tiger.Types.UnitTests
{
    public sealed class TaskExtensionsTests
    {
        const string sentinel = "sentinel";

        [Fact(DisplayName = "Apply completes its task, then invokes its func.")]
        public async Task Apply()
        {
            // arrange
            var task = Delay(FromSeconds(3));

            // act
            var actual = await task.Apply(() => sentinel);

            // assert
            Assert.True(task.IsCompleted);
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Then completes its task, then invokes its task.")]
        public async Task Then()
        {
            // arrange
            var task = Delay(FromSeconds(3));

            // act
            var actual = await task.Then(() => FromResult(sentinel));

            // assert
            Assert.True(task.IsCompleted);
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Map completes its task, then invokes its func with the task's " +
                            "return value as a parameter.")]
        public async Task Map()
        {
            // arrange
            
            // act
            var actual = await FromResult(sentinel).Map(v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName= "Bind complete its task, then invokes its task with the previous task's " +
                           "return value as a parameter.")]
        public async Task Bind()
        {
            // arrange
            
            // act
            var actual = await FromResult(sentinel).Bind(v => FromResult(v.Length));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }
    }
}
