// ReSharper disable All

using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    /* note(cosborn)
     * NUnit doesn't have good support for async in Assert.Throws<T>, so we
     * work around it where necessary using the accidental support in Assert.That.
     */
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [TestFixture(TestOf = typeof(Either<,>))]
    public sealed class EitherTestFixture
    {
        const string Sentinel = "sentinel";

        #region IsLeft, IsRight

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftBoth_IsLeft_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(Sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftOne_IsLeft_Left()
        {
            // arrange, act
            var actual = Either<int>.WithLeft(Sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Right Either should not be in the Left state.")]
        public void FromRightBoth_IsLeft_Right()
        {
            // arrange, act
            var actual = Either<string, int>.FromRight(42);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Right Either should not be in the Left state.")]
        public void FromRightOne_IsLeft_Right()
        {
            // arrange, act
            var actual = Either<int>.WithRight(Sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Bottom Either should not be in the Left state.")]
        public void Default_IsLeft_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.That(actual.IsLeft, Is.False);
        }

        [Test(Description = "A Left Either should not be in the Right state.")]
        public void FromLeftBoth_IsRight_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(Sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Left Either should not be in the Right state.")]
        public void FromLeftOne_IsRight_Left()
        {
            // arrange, act
            var actual = Either<int>.WithLeft(Sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromLeftBoth_IsRight_Right()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(Sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromLeftOne_IsRight_Right()
        {
            // arrange, act
            var actual = Either<int>.WithLeft(Sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Bottom Either should not be in the Right state.")]
        public void Default_IsRight_Bottom()
        {
            // arrange, act
            var actual = default(Either<string, int>);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        #endregion

        #region Match

        [Test(Description = "Matching a Left Either should return the Left func branch, " +
                            "not the Right func branch.")]
        public void FuncFuncMatchReturn_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left func branch.")]
        public void FuncFuncMatchReturn_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = value.Match(
                left: l => l,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left func branch, " +
                            "not the Right task branch.")]
        public async Task FuncTaskMatchReturn_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right task branch, " +
                            "not the Left func branch.")]
        public async Task FuncTaskMatchReturn_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Match(
                left: l => l,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                    "not the Right func branch.")]
        public async Task TaskFuncMatchReturn_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskFuncMatchReturn_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                            "not the Right func branch.")]
        public async Task TaskTaskMatchReturn_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchReturn_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right action branch.")]
        public void ActionActionMatchVoid_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => actual = Sentinel,
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right v branch, " +
                            "not the Left v branch.")]
        public void ActionActionMatchVoid_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => { },
                right: r => actual = Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right task branch.")]
        public async Task ActionTaskMatchVoid_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => actual = Sentinel,
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left action branch.")]
        public async Task ActionTaskMatchVoid_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => { },
                right: r => Task.Run(() => actual = Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right action branch.")]
        public async Task TaskActionMatchVoid_Left()
        {
            // arrange
            var value = Either<string>.WithLeft(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = Sentinel),
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right action branch, " +
                            "not the Left task branch.")]
        public async Task TaskActionMatchVoid_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => actual = Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right task branch.")]
        public async Task TaskTaskMatchVoid_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = Sentinel),
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchVoid_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => Task.Run(() => actual = Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Map

        [Test(Description = "Left-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapLeft_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = value.Map(left: _ => Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapLeft_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = value.Map(left: _ => false);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        [Test(Description = "Left-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapLeft_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = await value.Map(left: _ => Task.FromResult(Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapLeft_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);
            Assert.True(value.IsRight);

            // act
            var actual = await value.Map(left: _ => Task.FromResult(false));
            Assert.IsTrue(actual.IsRight);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapRight_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = value.Map(right: _ => 42);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapRight_Right()
        {
            // arrange
            var value = Either<int>.WithRight("megatron");
            Assert.True(value.IsRight);

            // act
            var actual = value.Map(right: _ => Sentinel);
            Assert.IsTrue(actual.IsRight);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithRight(Sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapRight_Left()
        {
            // arrange
            var value = Either<int>.WithLeft(Sentinel);

            // act
            var actual = await value.Map(right: _ => Task.FromResult(42));

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapRight_Right()
        {
            // arrange
            var value = Either<int>.WithRight("megatron");

            // act
            var actual = await value.Map(right: _ => Task.FromResult(Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithRight(Sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public void FuncFuncMap_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = value.Map(
                left: _ => Sentinel,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public void FuncFuncMap_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = value.Map(
                left: _ => false,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public async Task FuncTaskMap_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(Sentinel),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task FuncTaskMap_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskFuncMap_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = await value.Map(
                left: _ => Sentinel,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public async Task TaskFuncMap_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Map(
                left: _ => false,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskTaskMap_Left()
        {
            // arrange
            var value = Either<int>.WithLeft("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(Sentinel),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either<int>.WithLeft(Sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskTaskMap_Right()
        {
            // arrange
            var value = Either<int>.WithRight(Sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either<bool>.WithRight(Sentinel)));
        }

        #endregion

        #region Bind



        #endregion
    }
}
