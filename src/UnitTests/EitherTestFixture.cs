// ReSharper disable All
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="Either{TLeft,TRight}"/>.</summary>
    [TestFixture(TestOf = typeof(Either<,>))]
    public sealed class EitherTestFixture
    {
        const string sentinel = "sentinel";

        #region IsLeft, IsRight

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftBoth_IsLeft_Left()
        {
            // arrange, act
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.That(actual.IsLeft, Is.True);
        }

        [Test(Description = "A Left Either should be in the Left state.")]
        public void FromLeftOne_IsLeft_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

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
            var actual = Either.Right<int, string>(sentinel);

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
            var actual = Either<string, int>.FromLeft(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Left Either should not be in the Right state.")]
        public void FromLeftOne_IsRight_Left()
        {
            // arrange, act
            var actual = Either.Left<string, int>(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.False);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromLeftBoth_IsRight_Right()
        {
            // arrange, act
            var actual = Either<int, string>.FromRight(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.True);
        }

        [Test(Description = "A Right Either should be in the Right state.")]
        public void FromLeftOne_IsRight_Right()
        {
            // arrange, act
            var actual = Either.Right<int, string>(sentinel);

            // assert
            Assert.That(actual.IsRight, Is.True);
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
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Match(
                left: l => l.Length,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left func branch.")]
        public void FuncFuncMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Match(
                left: l => l,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left func branch, " +
                            "not the Right task branch.")]
        public async Task FuncTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right task branch, " +
                            "not the Left func branch.")]
        public async Task FuncTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                    "not the Right func branch.")]
        public async Task TaskFuncMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskFuncMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should return the Left task branch, " +
                            "not the Right func branch.")]
        public async Task TaskTaskMatchReturn_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Match(
                left: l => l.Length.Pipe(Task.FromResult),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Right Either should return the Right func branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchReturn_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Match(
                left: Task.FromResult,
                right: r => r.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel.Length));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right action branch.")]
        public void ActionActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => actual = sentinel,
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right v branch, " +
                            "not the Left v branch.")]
        public void ActionActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                left: l => { },
                right: r => actual = sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left action branch, " +
                            "not the Right task branch.")]
        public async Task ActionTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => actual = sentinel,
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left action branch.")]
        public async Task ActionTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => { },
                right: r => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right action branch.")]
        public async Task TaskActionMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = sentinel),
                right: r => { });

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right action branch, " +
                            "not the Left task branch.")]
        public async Task TaskActionMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => actual = sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Left Either should execute the Left task branch, " +
                            "not the Right task branch.")]
        public async Task TaskTaskMatchVoid_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.Run(() => actual = sentinel),
                right: r => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        [Test(Description = "Matching a Right Either should execute the Right task branch, " +
                            "not the Left task branch.")]
        public async Task TaskTaskMatchVoid_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                left: l => Task.WhenAll(),
                right: r => Task.Run(() => actual = sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(sentinel));
        }

        #endregion

        #region Map

        [Test(Description = "Left-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(left: _ => sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(left: _ => false);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapLeft_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(left: _ => Task.FromResult(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Left-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapLeft_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(left: _ => Task.FromResult(false));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a func should return a Left Either.")]
        public void FuncMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = value.Map(right: _ => 42);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a func should return a Right Either.")]
        public void FuncMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");
            Assert.True(value.IsRight);

            // act
            var actual = value.Map(right: _ => sentinel);
            Assert.IsTrue(actual.IsRight);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskMapRight_Left()
        {
            // arrange
            var value = Either.Left<string, int>(sentinel);

            // act
            var actual = await value.Map(right: _ => Task.FromResult(42));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Right-Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskMapRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>("megatron");

            // act
            var actual = await value.Map(right: _ => Task.FromResult(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public void FuncFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = value.Map(
                left: _ => sentinel,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public void FuncFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Map(
                left: _ => false,
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a func should return a Left Either.")]
        public async Task FuncTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(sentinel),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task FuncTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: r => r);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskFuncMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => sentinel,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a func should return a Right Either.")]
        public async Task TaskFuncMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => false,
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Mapping a Left Either over a task should return a Left Either.")]
        public async Task TaskTaskMap_Left()
        {
            // arrange
            var value = Either.Left<string, int>("megatron");

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(sentinel),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<string, int>(sentinel)));
        }

        [Test(Description = "Mapping a Right Either over a task should return a Right Either.")]
        public async Task TaskTaskMap_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Map(
                left: _ => Task.FromResult(false),
                right: Task.FromResult);

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        #endregion

        #region Bind

        [Test(Description = "Right-Binding a Left Either over a func should return a Left Either.")]
        public void FuncBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(42)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Left Either " + 
                            "should return a Left Either.")]
        public void FuncBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(33)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, bool>(true)));
        }

        [Test(Description = "Left-Binding a Right Either over a func should return a Right Either.")]
        public void FuncBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = value.Bind(v => v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, int>(42)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public void FuncBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42 ? Either.Right<bool, string>(sentinel) : Either.Left<bool, string>(true));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public void FuncBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Bind(v => v == 42 ? Either.Left<bool, string>(true) : Either.Right<bool, string>(sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<bool, string>(true)));
        }

        [Test(Description = "Right-Binding a Left Either over a func should return a Left Either.")]
        public async Task TaskBindRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(42)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskBindRight_Right_Left()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Left<int, bool>(33)
                : Either.Right<int, bool>(true)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<int, bool>(33)));
        }

        [Test(Description = "Right-Binding a Right Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskBindRight_Right_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Right<int, bool>(true)
                : Either.Left<int, bool>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<int, bool>(true)));
        }

        [Test(Description = "Left-Binding a Right Either over a func should return a Right Either.")]
        public async Task TaskBindLeft_Right()
        {
            // arrange
            var value = Either.Right<string, int>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == sentinel
                ? Either.Left<bool, int>(true)
                : Either.Right<bool, int>(33)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, int>(42)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Right Either " +
                            "should return a Right Either.")]
        public async Task TaskBindLeft_Left_Right()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == 42
                ? Either.Right<bool, string>(sentinel)
                : Either.Left<bool, string>(true)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Right<bool, string>(sentinel)));
        }

        [Test(Description = "Left-Binding a Left Either over a func that returns a Left Either " +
                            "should return a Left Either.")]
        public async Task TaskBindLeft_Left_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = await value.Bind(v => Task.FromResult(v == 42
                ? Either.Left<bool, string>(true)
                : Either.Right<bool, string>(sentinel)));

            // assert
            Assert.That(actual, Is.EqualTo(Either.Left<bool, string>(true)));
        }

        #endregion

        #region Fold

        [Test(Description = "Right-Folding over a Left Either should return the seed value.")]
        public void FuncFoldRight_Left()
        {
            // arrange
            var value = Either.Left<int, string>(42);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(34));
        }

        [Test(Description = "Right-Folding over a Right Either should return result of invoking the accumulator" +
                            "over the seed value and the Right value.")]
        public void FuncFoldRight_Right()
        {
            // arrange
            var value = Either.Right<int, string>(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(42));
        }

        #endregion
    }
}
