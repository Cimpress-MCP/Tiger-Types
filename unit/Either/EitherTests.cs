using System;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;
using static System.Threading.Tasks.Task;
using static Tiger.Types.Resources;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [Properties(Arbitrary = new [] { typeof(Generators) }, QuietOnSuccess = true)]
    public static partial class EitherTests
    {
        [Property(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor_Left(NonEmptyString left)
        {
            var expected = Either<string, int>.From(left.Get);
            var actual = new Either<string, int>(expected);

            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor_Right(int right)
        {
            var expected = Either<string, int>.From(right);
            var actual = new Either<string, int>(expected);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Attempting to create a Left Either from null throws.")]
        public static void FromLeftBoth_Null_Throws()
        {
            var actual = Record.Exception(() => Either<string, int>.From(left: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A Left Either is in the Left state.")]
        public static void FromLeftBoth_IsLeft_Left(NonEmptyString left) =>
            Assert.True(Either<string, int>.From(left.Get).IsLeft);

        [Fact(DisplayName = "Attempting to create a Left Either from null throws.")]
        public static void FromLeftOne_Null_Throws()
        {
            var actual = Record.Exception(() => Either.From<string, int>(left: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("left", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A Left Either is in the Left state.")]
        public static void FromLeftOne_IsLeft_Left(NonEmptyString left) =>
            Assert.True(Either.From<string, int>(left.Get).IsLeft);

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        public static void FromRightBoth_IsLeft_Right(int right) => Assert.False(Either<string, int>.From(right).IsLeft);

        [Property(DisplayName = "A Right Either is not in the Left state.")]
        public static void FromRightOne_IsLeft_Right(int right) => Assert.False(Either.From<string, int>(right).IsLeft);

        [Fact(DisplayName = "A Bottom Either is not in the Left state.")]
        public static void Default_IsLeft_Bottom() => Assert.False(default(Either<string, int>).IsLeft);

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        public static void FromLeftBoth_IsRight_Left(NonEmptyString left) =>
            Assert.False(Either<string, int>.From(left.Get).IsRight);

        [Property(DisplayName = "A Left Either is not in the Right state.")]
        public static void FromLeftOne_IsRight_Left(NonEmptyString left) =>
            Assert.False(Either.From<string, int>(left.Get).IsRight);

        [Fact(DisplayName = "Attempting to create a Right Either from null throws.")]
        public static void FromRightBoth_Null_Throws()
        {
            var actual = Record.Exception(() => Either<int, string>.From(right: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A Right Either is in the Right state.")]
        public static void FromRightBoth_IsRight_Right(int right) =>
            Assert.True(Either<string, int>.From(right).IsRight);

        [Fact(DisplayName = "Attempting to create a Right Either from null throws.")]
        public static void FromRightOne_Null_Throws()
        {
            var actual = Record.Exception(() => Either.From<int, string>(right: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("right", ane.Message, Ordinal);
        }

        [Property(DisplayName = "A Right Either is in the Right state.")]
        public static void FromRightOne_IsRight_Right(int right) =>
            Assert.True(Either.From<string, int>(right).IsRight);

        [Fact(DisplayName = "A Bottom Either is not in the Right state.")]
        public static void Default_IsRight_Bottom() => Assert.False(default(Either<string, int>).IsRight);

        [Property(DisplayName = "Forcibly unwrapping a Left Either throws.")]
        public static void Value_Left_Throws(NonEmptyString left)
        {
            var actual = Record.Exception(() => Either.From<string, int>(left.Get).Value);

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(EitherIsNotRight, ioe.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping a Right Either returns the Right value.")]
        public static void Value_Right(int right) => Assert.Equal(right, Either.From<string, int>(right).Value);

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        public static void GetValueOrDefault_Left(NonEmptyString left) =>
            Assert.Equal(default, Either.From<string, int>(left.Get).GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        public static void GetValueOrDefault_Right(int right) =>
            Assert.Equal(right, Either.From<string, int>(right).GetValueOrDefault());

        [Property(DisplayName = "Attempting to coalesce an Either with null throws.")]
        public static void ValueGetValueOrDefault_Null_Throws(Either<int, string> either)
        {
            var actual = Record.Exception(() => either.GetValueOrDefault(other: (string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        public static void ValueGetValueOrDefault_Left(NonEmptyString left, int alternative) =>
            Assert.Equal(alternative, Either.From<string, int>(left.Get).GetValueOrDefault(alternative));

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        public static void ValueGetValueOrDefault_Right(int right, int alternative) =>
            Assert.Equal(right, Either.From<string, int>(right).GetValueOrDefault(alternative));

        [Property(DisplayName = "Attempting to coalesce an Either with null throws.")]
        public static void FuncGetValueOrDefault_Null_Throws(Either<string, int> either)
        {
            var actual = Record.Exception(() => either.GetValueOrDefault(other: null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        public static void FuncGetValueOrDefault_Left(NonEmptyString left, int alternative) =>
            Assert.Equal(alternative, Either.From<string, int>(left.Get).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        public static void FuncGetValueOrDefault_Right(int right, int alternative) =>
            Assert.Equal(right, Either.From<string, int>(right).GetValueOrDefault(() => alternative));

        [Property(DisplayName = "Attempting to coalesce an Either with null throws.")]
        public static async Task TaskGetValueOrDefault_Null_Throws(Either<string, int> either)
        {
            var actual = await Record.ExceptionAsync(() => either.GetValueOrDefaultAsync(other: null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a Left Either with an alternative value returns the alternative value.")]
        public static async Task TaskGetValueOrDefault_Left(NonEmptyString left, int alternative)
        {
            var actual = await Either.From<string, int>(left.Get)
                .GetValueOrDefaultAsync(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(alternative, actual);
        }

        [Property(DisplayName = "Coalescing a Right Either with an alternative value returns the Right value.")]
        public static async Task TaskGetValueOrDefault_Right(int right, int alternative)
        {
            var actual = await Either.From<string, int>(right)
                .GetValueOrDefaultAsync(() => FromResult(alternative))
                .ConfigureAwait(false);

            Assert.Equal(right, actual);
        }
    }
}
