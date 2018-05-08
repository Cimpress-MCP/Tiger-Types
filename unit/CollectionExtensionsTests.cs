using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of <see cref="CollectionExtensions"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class CollectionExtensionsTests
    {
        [Fact(DisplayName = "Optionally getting the first element of null throws.")]
        public static void FirstOrNone_Null_Throws()
        {
            var actual = Record.Exception(() => ((IEnumerable<int>)null).FirstOrNone());

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("source", ane.Message, Ordinal);
        }

        [Fact(DisplayName = "Optionally getting the first element of an empty collection returns a None Option.")]
        public static void FirstOrNone_Empty_None()
        {
            var actual = Enumerable.Empty<int>().FirstOrNone();

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Optionally getting the first element of a populated collection returns a Some Option.")]
        public static void FirstOrNone_Populated_Some(NonEmptyArray<int> values)
        {
            var actual = values.Get.FirstOrNone();

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(values.Get[0], innerValue);
        }

        [Property(DisplayName = "Optionally getting the first element of a populated collection returns a Some Option.")]
        public static void FirstOrNone_PopulatedEnumerable_Some(int value, PositiveInt count)
        {
            var actual = Enumerable.Repeat(value, count.Get).FirstOrNone();

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(value, innerValue);
        }

        [Property(DisplayName = "Optionally getting the first element of null throws.")]
        public static void FirstOrNonePredicate_SourceNull_Throws(Func<int, bool> predicate)
        {
            var actual = Record.Exception(() => CollectionExtensions.FirstOrNone(null, predicate));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("source", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Optionally getting the first element of a collection with a null predicate throws.")]
        public static void FirstOrNonePredicate_PredicateNull_Throws(NonEmptyArray<int> source)
        {
            var actual = Record.Exception(() => source.Get.FirstOrNone(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("predicate", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Optionally getting the first element of an empty collection with any filter " +
                                "returns a None Option.")]
        public static void FirstOrNonePredicate_Empty_None(Func<int, bool> predicate)
        {
            var actual = Enumerable.Empty<int>().FirstOrNone(predicate);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Optionally getting the first element of a populated collection returns a Some Option.")]
        public static void FirstOrNonePredicate_Populated_Some(NonEmptyArray<int> values)
        {
            var actual = values.Get.FirstOrNone(_ => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(values.Get[0], innerValue);
        }

        [Property(DisplayName = "Optionally getting the first element of a populated collection " +
                                "failing the filter for all members returns a None Option.")]
        public static void FirstOrNonePredicate_False_Populated_Some(NonEmptyArray<int> values)
        {
            var actual = values.Get.FirstOrNone(_ => false);

            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Optionally getting the first element of a populated collection returns a Some Option.")]
        public static void FirstOrNonePredicate_PopulatedEnumerable_Some(int value, PositiveInt count)
        {
            var actual = Enumerable.Repeat(value, count.Get).FirstOrNone(_ => true);

            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(value, innerValue);
        }

        [Property(DisplayName = "Folding over null with folder func throws.")]
        public static void Fold_NullValue_Throws(int state, Func<int, int, int> folder)
        {
            var actual = Record.Exception(() => CollectionExtensions.Fold(null, state, folder));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("collection", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over null with folder func throws.")]
        public static void FoldImplicit_NullValue_Throws(Func<int, int, int> folder)
        {
            var actual = Record.Exception(() => CollectionExtensions.Fold(null, folder));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("collection", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null state throws.")]
        public static void Fold_NullState_Throws(
            List<string> collection,
            Func<string, string, string> folder)
        {
            var actual = Record.Exception(() => collection.Fold(null, folder));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static void Fold_NullFolder_Throws(List<string> collection, NonEmptyString state)
        {
            var actual = Record.Exception(() => collection.Fold(state.Get, null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static void FoldImplicit_NullFolder_Throws(List<string> collection)
        {
            var actual = Record.Exception(() => collection.Fold<string, int>(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over null with folder task throws.")]
        public static async Task FoldAsync_NullValue_Throws(int state, Func<int, int, Task<int>> folder)
        {
            var actual = await Record.ExceptionAsync(() => CollectionExtensions.FoldAsync(null, state, folder))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("collection", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding over null with folder task throws.")]
        public static async Task FoldAsyncImplicit_NullValue_Throws(Func<int, int, Task<int>> folder)
        {
            var actual = await Record.ExceptionAsync(() => CollectionExtensions.FoldAsync(null, folder))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("collection", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null state throws.")]
        public static async Task FoldAsync_NullState_Throws(
            List<string> collection,
            Func<string, string, Task<string>> folder)
        {
            var actual = await Record.ExceptionAsync(() => collection.FoldAsync(null, folder))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("state", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static async Task FoldAsync_NullFolder_Throws(List<string> collection, NonEmptyString state)
        {
            var actual = await Record.ExceptionAsync(() => collection.FoldAsync(state.Get, null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Folding with a null folder func throws.")]
        public static async Task FoldAsyncImplicit_NullFolder_Throws(List<string> collection)
        {
            var actual = await Record.ExceptionAsync(() => collection.FoldAsync<string, int>(null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("folder", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping over null with a func mapper throws.")]
        public static void MapFunc_NullValue_Throws(Func<int, string> mapper)
        {
            var actual = Record.Exception(() => CollectionExtensions.Map(null, mapper));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping with a null func mapper throws.")]
        public static void MapFunc_NullMapper_Throws(NonEmptyArray<int> collection)
        {
            var actual = Record.Exception(() => collection.Item.Map((Func<int, string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping over a collection is the same as Select.")]
        public static void MapFunc_Maps(string[] collection, Func<string, int> mapper)
        {
            var expected = collection.Select(mapper);
            var actual = collection.Map(mapper);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Mapping over null with a task mapper throws.")]
        public static async Task MapTask_NullValue_Throws(Func<int, Task<string>> mapper)
        {
            var actual = await Record.ExceptionAsync(() => CollectionExtensions.MapAsync(null, mapper))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping with a null task mapper throws.")]
        public static async Task MapTask_NullMapper_Throws(int[] collection)
        {
            var actual = await Record.ExceptionAsync(() => collection.MapAsync((Func<int, Task<string>>)null))
                .ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Mapping over a collection is the same as Select.")]
        public static async Task MapTask_Maps(string[] collection, Func<string, Task<int>> mapper)
        {
            var expected = await Task.WhenAll(collection.Select(mapper)).ConfigureAwait(false);
            var actual = await collection.MapAsync(mapper).ConfigureAwait(false);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "MapCatting over null throws.")]
        public static void MapCat_NullValue_Throws(Func<int, Option<string>> mapper)
        {
            var actual = Record.Exception(() => CollectionExtensions.MapCat(null, mapper));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("enumerableValue", ane.Message, Ordinal);
        }

        [Property(DisplayName = "MapCatting with a null mapper.")]
        public static void MapCat_NullMapper_Throws(int[] collection)
        {
            var actual = Record.Exception(() => collection.MapCat<int, string>(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("mapper", ane.Message, Ordinal);
        }

        [Property(DisplayName = "MapCatting, uh... MapCats successfully?")]
        public static void MapCat_MapCats(int[] collection)
        {
            Option<string> Mapper(int value) => value < 0
                ? Option.None
                : Option.From(value.ToString(CultureInfo.InvariantCulture));

            var expected = collection.Count(v => v >= 0);
            var actual = collection.MapCat(Mapper).Count();

            Assert.Equal(expected, actual);
        }
    }
}
