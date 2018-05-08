using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>
    /// Tests related to extensions to the functionality of <see cref="Task{TResult}"/>,
    /// specialized for implementations of <see cref="IEnumerable{T}"/>.
    /// </summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class CollectionTaskExtensionsTests
    {
        [Property(DisplayName = "Mapping over null with a mapper func throws.")]
        public static async Task MapTFunc_NullValue_Throws(Func<string, int> mapper)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.MapT(null, mapper)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping with a null mapper func throws.")]
        public static async Task MapTFunc_NullMapper_Throws(Task<List<string>> collection)
        {
            var enumerableCollection = collection.Map(Enumerable.AsEnumerable);
            var actual = await Record.ExceptionAsync(
                () => enumerableCollection.MapT((Func<string, int>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping over a collection in a task with a func is the same as " +
                                "mapping over the collection not wrapped in a task.")]
        public static async Task MapTFunc_Maps(List<string> collection, Func<string, int> mapper)
        {
            var expected = collection.Map(mapper);
            var actual = await Task.FromResult<IEnumerable<string>>(collection).MapT(mapper).ConfigureAwait(false);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Mapping over null with a mapper task throws.")]
        public static async Task MapTTask_NullValue_Throws(Func<string, Task<int>> mapper)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.MapTAsync(null, mapper)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping with a null mapper task throws.")]
        public static async Task MapTTask_NullMapper_Throws(Task<List<string>> collection)
        {
            var enumerableCollection = collection.Map(Enumerable.AsEnumerable);
            var actual = await Record.ExceptionAsync(
                () => enumerableCollection.MapTAsync((Func<string, Task<int>>)null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping over a collection in a task with a func is the same as " +
                                "mapping over the collection not wrapped in a task.")]
        public static async Task MapTTask_Maps(List<string> collection, Func<string, Task<int>> mapper)
        {
            var expected = await collection.MapAsync(mapper).ConfigureAwait(false);
            var actual = await Task.FromResult<IEnumerable<string>>(collection).MapTAsync(mapper).ConfigureAwait(false);

            // note(cosborn) xUnit doesn't recognize these as collections without help.
            Assert.Equal<IEnumerable<int>>(expected, actual);
        }

        [Property(DisplayName = "Folding over null with folder func throws.")]
        public static async Task FoldTFunc_NullValue_Throws(int state, Func<int, int, int> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.FoldT(null, state, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over null with folder func throws.")]
        public static async Task FoldTImplicitFunc_NullValue_Throws(Func<int, int, int> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.FoldT(null, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null state throws.")]
        public static async Task FoldTFunc_NullState_Throws(
            List<string> collection,
            Func<string, string, string> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection).FoldT(null, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static async Task FoldTFunc_NullFolder_Throws(List<string> collection, NonEmptyString state)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection).FoldT(state.Get, null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static async Task FoldTImplicitFunc_NullFolder_Throws(List<string> collection)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection).FoldT<string, int>(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a collection in a task with a func is the same as " +
                                "folding over the collection not wrapped in a task.")]
        public static async Task FoldTFunc_Folds(
            List<string> collection,
            NonEmptyString state)
        {
            string Folder(string acc, string curr) => string.Concat(acc, curr);

            var expected = collection.Fold(state.Get, Folder);
            var actual = await Task.FromResult<IReadOnlyCollection<string>>(collection)
                .FoldT(state.Get, Folder)
                .ConfigureAwait(false);

            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        [Property(DisplayName = "Folding over a collection in a task with a func is the same as " +
                                "folding over the collection not wrapped in a task.")]
        public static async Task FoldTImplicitFunc_Folds(List<int> collection)
        {
            int Folder(int acc, int curr) => acc + curr;

            var expected = collection.Fold<int, int>(Folder);
            var actual = await Task.FromResult<IReadOnlyCollection<int>>(collection)
                .FoldT<int, int>(Folder)
                .ConfigureAwait(false);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Folding over null with folder task throws.")]
        public static async Task FoldTTask_NullValue_Throws(int state, Func<int, int, Task<int>> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.FoldTAsync(null, state, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over null with folder task throws.")]
        public static async Task FoldTImplicitTask_NullValue_Throws(Func<int, int, Task<int>> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => CollectionTaskExtensions.FoldTAsync(null, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableTaskValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null state throws.")]
        public static async Task FoldTTask_NullState_Throws(
            List<string> collection,
            Func<string, string, Task<string>> folder)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection).FoldTAsync(null, folder)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder task throws.")]
        public static async Task FoldTTask_NullFolder_Throws(List<string> collection, NonEmptyString state)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection)
                    .FoldTAsync(state.Get, null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder task throws.")]
        public static async Task FoldTImplicitTask_NullFolder_Throws(List<string> collection)
        {
            var actual = await Record.ExceptionAsync(
                () => Task.FromResult<IReadOnlyCollection<string>>(collection)
                    .FoldTAsync((Func<int, string, Task<int>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over a collection in a task with a task is the same as " +
                                "folding over the collection not wrapped in a task.")]
        public static async Task FoldTTask_Folds(
            List<string> collection,
            NonEmptyString state)
        {
            Task<string> Folder(string acc, string curr) => Task.FromResult(string.Concat(acc, curr));

            var expected = await collection.FoldAsync(state.Get, Folder).ConfigureAwait(false);
            var actual = await Task.FromResult<IReadOnlyCollection<string>>(collection)
                .FoldTAsync(state.Get, Folder)
                .ConfigureAwait(false);

            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        [Property(DisplayName = "Folding over a collection in a task with a task is the same as " +
                                "folding over the collection not wrapped in a task.")]
        public static async Task FoldTImplicitTask_Folds(List<int> collection)
        {
            Task<int> Folder(int acc, int curr) => Task.FromResult(acc + curr);

            var expected = await collection.FoldAsync<int, int>(Folder).ConfigureAwait(false);
            var actual = await Task.FromResult<IReadOnlyCollection<int>>(collection)
                .FoldTAsync<int, int>(Folder)
                .ConfigureAwait(false);

            Assert.Equal(expected, actual);
        }
    }
}
