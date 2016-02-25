// ReSharper disable All
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    [TestFixture]
    public sealed class TaskExtensionsTestFixture
    {
        const string sentinel = "sentinel";

        [Test(Description = "Apply should complete its task, then invoke its func.")]
        [Category("Extension")]
        public async Task Apply()
        {
            // arrange
            var value = Task.WhenAll();
            Func<string> applier = () => sentinel;

            // act
            var actual = await value.Apply(applier);

            // assert
            Assert.That(value.IsCompleted, Is.True);
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Apply should complete its task, then invoke its task.")]
        [Category("Extension")]
        public async Task Then()
        {
            // arrange
            var value = Task.WhenAll();
            Func<Task<string>> thenner = () => Task.FromResult(sentinel);

            // act
            var actual = await value.Then(thenner);

            // assert
            Assert.That(value.IsCompleted, Is.True);
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Map should complete its task, then invoke its func with the task's" +
                            "return value as a parameter.")]
        [Category("Extension")]
        public async Task Map()
        {
            // arrange
            Task<string> value = Task.FromResult(sentinel);
            Func<string, int> mapper = v => v.Length;

            // act
            var actual = await value.Map(mapper);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Bind should complete its task, then invoke its task with the previous task's" +
                            "return value as a parameter.")]
        [Category("Extension")]
        public async Task Bind()
        {
            // arrange
            Task<string> value = Task.FromResult(sentinel);
            Func<string, Task<int>> binder = v => Task.FromResult(v.Length);

            // act
            var actual = await value.Bind(binder);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }
    }
}
