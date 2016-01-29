// ReSharper disable All

using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    [TestFixture]
    public sealed class TaskExtensionsTestFixture
    {
        const string Sentinel = "sentinel";

        #region Apply

        #region Null Throws

        [Test, Precondition, Category("Extension")]
        public void Apply_NullTask_Throws()
        {
            // arrange
            Task value = null;
            Func<string> applier = () => Sentinel;

            // act, assert
            Assert.That(() => value.Apply(applier),
                Throws.ArgumentNullException.With.Message.Contains("task"));
        }

        [Test, Precondition, Category("Extension")]
        public void Apply_NullApplier_Throws()
        {
            // arrange
            Task value = Task.WhenAll();
            Func<string> applier = null;

            // act, assert
            Assert.That(() => value.Apply(applier),
                Throws.ArgumentNullException.With.Message.Contains("applier"));
        }

        #endregion

        [Test(Description = "Apply should complete its task, then invoke its func.")]
        [Category("Extension")]
        public async Task Apply()
        {
            // arrange
            var value = Task.WhenAll();
            Func<string> applier = () => Sentinel;

            // act
            var actual = await value.Apply(applier);

            // assert
            Assert.That(value.IsCompleted, Is.True);
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Then

        #region Null Throws

        [Test, Precondition, Category("Extension")]
        public void Then_NullTask_Throws()
        {
            // arrange
            Task value = null;
            Func<Task<string>> applier = () => Task.FromResult(Sentinel);

            // act, assert
            Assert.That(() => value.Then(applier),
                Throws.ArgumentNullException.With.Message.Contains("task"));
        }

        [Test, Precondition, Category("Extension")]
        public void Then_NullThenner_Throws()
        {
            // arrange
            Task value = Task.WhenAll();
            Func<Task<string>> thenner = null;

            // act, assert
            Assert.That(() => value.Then(thenner),
                Throws.ArgumentNullException.With.Message.Contains("thenner"));
        }

        #endregion

        [Test(Description = "Apply should complete its task, then invoke its task.")]
        [Category("Extension")]
        public async Task Then()
        {
            // arrange
            var value = Task.WhenAll();
            Func<Task<string>> thenner = () => Task.FromResult(Sentinel);

            // act
            var actual = await value.Then(thenner);

            // assert
            Assert.That(value.IsCompleted, Is.True);
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Map

        #region Null Throws

        [Test, Precondition, Category("Extension")]
        public void Map_NullTask_Throws()
        {
            // arrange
            Task<string> value = null;
            Func<string, string> mapper = _ => Sentinel;

            // act, assert
            Assert.That(() => value.Map(mapper),
                Throws.ArgumentNullException.With.Message.Contains("task"));
        }

        [Test, Precondition, Category("Extension")]
        public void Map_NullMapper_Throws()
        {
            // arrange
            Task<string> value = Task.FromResult(Sentinel);
            Func<string, string> mapper = null;

            // act, assert
            Assert.That(() => value.Map(mapper),
                Throws.ArgumentNullException.With.Message.Contains("mapper"));
        }

        #endregion

        [Test(Description = "Map should complete its task, then invoke its func with the task's" +
                            "return value as a parameter.")]
        public async Task Map()
        {
            // arrange
            Task<string> value = Task.FromResult(Sentinel);
            Func<string, int> mapper = v => v.Length;

            // act
            var actual = await value.Map(mapper);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        #endregion

        #region Bind

        #region Null Throws

        [Test, Precondition, Category("Extension")]
        public void Bind_NullTask_Throws()
        {
            // arrange
            Task<string> value = null;
            Func<string, Task<string>> binder = _ => Task.FromResult(Sentinel);

            // act, assert
            Assert.That(() => value.Bind(binder),
                Throws.ArgumentNullException.With.Message.Contains("task"));
        }

        [Test, Precondition, Category("Extension")]
        public void Bind_NullBinder_Throws()
        {
            // arrange
            Task<string> value = Task.FromResult(Sentinel);
            Func<string, Task<string>> binder = null;

            // act, assert
            Assert.That(() => value.Bind(binder),
                Throws.ArgumentNullException.With.Message.Contains("binder"));
        }

        #endregion

        [Test(Description = "Bind should complete its task, then invoke its task with the previous task's" +
                            "return value as a parameter.")]
        public async Task Bind()
        {
            // arrange
            Task<string> value = Task.FromResult(Sentinel);
            Func<string, Task<int>> binder = v => Task.FromResult(v.Length);

            // act
            var actual = await value.Bind(binder);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        #endregion
    }
}
