using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <context>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</context>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Joining a Left Either Either returns a Left Either.")]
        static void Join_Left(Guid left)
        {
            var actual = Either<Guid, Either<Guid, Version>>.FromLeft(left).Join();

            Assert.True(actual.IsLeft);
            Assert.Equal(left, (Guid)actual);
        }

        [Property(DisplayName = "Joining a Right Either containing a Left Either returns a Left Either.")]
        static void Join_RightLeft(Guid left)
        {
            var actual = Either<Guid, Either<Guid, Version>>.FromRight(Either<Guid, Version>.FromLeft(left)).Join();

            Assert.True(actual.IsLeft);
            Assert.Equal(left, (Guid)actual);
        }

        [Property(DisplayName = "Joining a Right Either containing a Right Either returns a Right Either.")]
        static void Join_RightRight(Version right)
        {
            var actual = Either<Guid, Either<Guid, Version>>.FromRight(Either<Guid, Version>.FromRight(right)).Join();

            Assert.True(actual.IsRight);
            Assert.Equal(right, actual.Value);
        }
    }
}
