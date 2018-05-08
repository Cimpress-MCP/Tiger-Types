using System;
using FsCheck.Xunit;
using Xunit;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to acting upon <see cref="Either{TLeft,TRight}"/>.</summary>
    public static partial class EitherTests
    {
        [Property(DisplayName = "Joining a Left Either Either returns a Left Either.")]
        public static void Join_Left(Guid left)
        {
            var actual = Either<Guid, Either<Guid, Version>>.From(left).Join();

            Assert.True(actual.IsLeft);
            Assert.Equal(left, (Guid)actual);
        }

        [Property(DisplayName = "Joining a Right Either containing a Left Either returns a Left Either.")]
        public static void Join_RightLeft(Guid left)
        {
            var actual = Either<Guid, Either<Guid, Version>>.From(Either<Guid, Version>.From(left)).Join();

            Assert.True(actual.IsLeft);
            Assert.Equal(left, (Guid)actual);
        }

        [Property(DisplayName = "Joining a Right Either containing a Right Either returns a Right Either.")]
        public static void Join_RightRight(Version right)
        {
            var actual = Either<Guid, Either<Guid, Version>>.From(Either<Guid, Version>.From(right)).Join();

            Assert.True(actual.IsRight);
            Assert.Equal(right, actual.Value);
        }
    }
}
