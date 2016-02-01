// ReSharper disable All

using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiger.Types.UnitTests
{
    /* note(cosborn)
     * NUnit doesn't have good support for async in Assert.Throws<T>, so we
     * work around it where necessary using the accidental support in Assert.That.
     */
    [TestFixture]
    public sealed class OptionTestFixture
    {
        const string Sentinel = "sentinel";

        #region IsNone, IsSome

        [Test(Description = "Non-null values should create Some Options using the typed static From method.")]
        [TestCase(Sentinel)]
        [TestCase("")]
        public void TypedFrom_Value_IsSome(string innerValue)
        {
            // arrange
            var value = Option<string>.From(innerValue);

            // assert
            Assert.That(value.IsNone, Is.False);
            Assert.That(value.IsSome, Is.True);
        }

        [Test(Description = "Null values should create None Options using the typed static From method.")]
        public void TypedFrom_Null_IsNone()
        {
            // arrange
            var value = Option<string>.From(null);

            // assert
            Assert.That(value.IsNone, Is.True);
            Assert.That(value.IsSome, Is.False);
        }

        [Test(Description = "Non-null values should create Some Options using the untyped static From method.")]
        [TestCase(Sentinel)]
        [TestCase("")]
        public void UntypedFrom_Value_IsSome(string innerValue)
        {
            // arrange
            var value = Option.From(innerValue);

            // assert
            Assert.That(value.IsNone, Is.False);
            Assert.That(value.IsSome, Is.True);
        }

        [Test(Description = "Null values should create None Options using the untyped static From method.")]
        public void UntypedFrom_Null_IsNone()
        {
            // arrange
            var value = Option.From((string)null);

            // assert
            Assert.That(value.IsNone, Is.True);
            Assert.That(value.IsSome, Is.False);
        }

        [Test(Description = "Non-null nullable values should create Some Options.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        public void UntypedFrom_NullableValue_IsSome(int? innerValue)
        {
            // arrange
            var value = Option.From(innerValue);

            // assert
            Assert.That(value.IsNone, Is.False);
            Assert.That(value.IsSome, Is.True);
        }

        [Test(Description = "Null nullable values should create None Options.")]
        public void UntypedFrom_NullableNull_IsNone()
        {
            // arrange
            var value = Option.From((int?)null);

            // assert
            Assert.That(value.IsNone, Is.True);
            Assert.That(value.IsSome, Is.False);
        }

        #endregion

        #region Match

        #region Null Throws

        [Test, Precondition]
        public void ValueFuncMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            string none = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: none,
                some: v => Sentinel));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contain("none"));
        }

        [Test, Precondition]
        public void ValueFuncMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, string> some = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: Sentinel,
                some: some));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contain("some"));
        }

        [Test, Precondition]
        public void ValueTaskMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            string none = null;

            // act, assert
            Assert.That(() => value.Match(
                none: none,
                some: v => Task.FromResult(Sentinel)),
                Throws.ArgumentNullException.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void ValueTaskMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task<string>> some = null;

            // act, assert
            Assert.That(() => value.Match(
                none: Sentinel,
                some: some),
                Throws.ArgumentNullException.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void FuncFuncMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string> none = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: none,
                some: v => Sentinel));

            // assert
            Assert.That(ex.Message, Does.Contain("none"));
        }

        [Test, Precondition]
        public void FuncFuncMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, string> some = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: () => Sentinel,
                some: some));

            // assert
            Assert.That(ex.Message, Does.Contain("some"));
        }

        [Test, Precondition]
        public void FuncTaskMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string> none = null;

            // act, assert
            Assert.That(() => value.Match(
                none: none,
                some: v => Task.FromResult(Sentinel)),
                Throws.ArgumentNullException.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void FuncTaskMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task<string>> some = null;

            // act, assert
            Assert.That(() => value.Match(
                none: () => Sentinel,
                some: some),
                Throws.ArgumentNullException.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void TaskFuncMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<Task<string>> none = null;

            // act, assert
            Assert.That(() => value.Match(
                none: none,
                some: v => Sentinel),
                Throws.ArgumentNullException.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void TaskFuncMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, string> some = null;

            // act, assert
            Assert.That(() => value.Match(
                none: () => Task.FromResult(Sentinel),
                some: some),
                Throws.ArgumentNullException.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void TaskTaskMatch_Return_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<Task<string>> none = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: none,
                some: v => Task.FromResult(Sentinel)));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void TaskTaskMatch_Return_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task<string>> some = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: () => Task.FromResult(Sentinel),
                some: some));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void ActionActionMatch_Void_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action none = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: none,
                some: v => Console.WriteLine(Sentinel)));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contain("none"));
        }

        [Test, Precondition]
        public void ActionActionMatch_Void_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action<string> some = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: () => Console.WriteLine(Sentinel),
                some: some));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contain("some"));
        }

        [Test, Precondition]
        public void ActionTaskMatch_Void_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action none = null;

            // act, assert
            Assert.That(() => value.Match(
                none: none,
                some: v => Task.Run(() => Console.WriteLine(Sentinel))),
                Throws.ArgumentNullException.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void ActionTaskMatch_Void_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task> some = null;

            // act, assert
            Assert.That(() => value.Match(
                none: () => Console.WriteLine(Sentinel),
                some: some),
                Throws.ArgumentNullException.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void TaskActionMatch_Void_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<Task> none = null;

            // act, assert
            Assert.That(() => value.Match(
                none: none,
                some: v => Console.WriteLine(Sentinel)),
                Throws.ArgumentNullException.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void TaskActionMatch_Void_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action<string> some = null;

            // act, assert
            Assert.That(() => value.Match(
                none: () => Task.Run(() => Console.WriteLine(Sentinel)),
                some: some),
                Throws.ArgumentNullException.With.Message.Contains("some"));
        }

        [Test, Precondition]
        public void TaskTaskMatch_Void_NullNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<Task> none = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: none,
                some: v => Task.Run(() => Console.WriteLine(Sentinel))));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("none"));
        }

        [Test, Precondition]
        public void TaskTaskMatch_Void_NullSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task> some = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Match(
                none: () => Task.Run(() => Console.WriteLine(Sentinel)),
                some: some));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("some"));
        }

        #endregion

        [Test(Description = "Matching a None Option should return the None value branch, " +
                            "not the Some func branch.")]
        public void ValueFuncMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: 0,
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some func branch, " +
                            "not the None value branch.")]
        public void ValueFuncMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Match(
                none: 0,
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should return the None value branch, " +
                            "not the Some task branch.")]
        public async Task ValueTaskMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: 0,
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some task branch, " +
                            "not the None value branch.")]
        public async Task ValueTaskMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Match(
                none: 0,
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should return the None func branch, " +
                            "not the Some func branch.")]
        public void FuncFuncMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => 0,
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some func branch, " +
                            "not the None func branch.")]
        public void FuncFuncMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Match(
                none: () => 0,
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should return the None func branch, " +
                            "not the Some task branch.")]
        public async Task FuncTaskMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => 0,
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some task branch, " +
                            "not the None func branch.")]
        public async Task FuncTaskMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Match(
                none: () => 0,
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should return the None task branch, " +
                            "not the Some func branch.")]
        public async Task TaskFuncMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some func branch, " +
                            "not the None task branch.")]
        public async Task TaskFuncMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should return the None task branch, " +
                            "not the Some task branch.")]
        public async Task TaskTaskMatch_Return_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => Task.FromResult(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatch_Return_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => Task.FromResult(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should execute the None action branch, " +
                            "not the Some action branch.")]
        public void ActionActionMatch_Void_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            value.Match(
                none: () => actual = Sentinel,
                some: v => { });

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Some Option should execute the Some action branch, " +
                            "not the None action branch.")]
        public void ActionActionMatch_Void_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                none: () => { },
                some: v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a None Option should execute the None action branch, " +
                            "not the Some task branch.")]
        public async Task ActionTaskMatch_Void_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => actual = Sentinel,
                some: v => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Some Option should execute the Some task branch, " +
                            "not the None action branch.")]
        public async Task ActionTaskMatch_Void_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => { },
                some: v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a None Option should execute the None task branch, " +
                            "not the Some action branch.")]
        public async Task TaskActionMatch_Void_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Task.Run(() => actual = Sentinel),
                some: v => { });

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Some Option should execute the Some action branch, " +
                            "not the None task branch.")]
        public async Task TaskActionMatch_Void_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Task.WhenAll(),
                some: v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a None Option should execute the None task branch, " +
                            "not the Some task branch.")]
        public async Task TaskTaskMatch_Void_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Task.Run(() => actual = Sentinel),
                some: v => Task.WhenAll());

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Matching a Some Option should execute the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatch_Void_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Task.WhenAll(),
                some: v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Map

        #region Null Throws

        [Test, Precondition]
        public void FuncMap_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, string> mapper = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Map(mapper));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("mapper"));
        }

        [Test, Precondition]
        public void TaskMap_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task<string>> mapper = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Map(mapper));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("mapper"));
        }

        #endregion

        [Test(Description = "Mapping a None Option over a func should return a None Option.")]
        public void FuncMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Map(v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Mapping a Some Option over a func should return a Some Option.")]
        public void FuncMap_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Map(v => v.Length);

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(Sentinel.Length)));
        }

        [Test(Description = "Mapping a None Option over a task should return a None Option.")]
        public async Task TaskMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Map(v => Task.FromResult(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Mapping a Some Option over a task should return a Some Option.")]
        public async Task TaskMap_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Map(v => Task.FromResult(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(Sentinel.Length)));
        }

        #endregion

        #region Tap

        #region Null Throws

        [Test, Precondition]
        public void FuncTap_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action<string> tapper = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Tap(tapper));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("tapper"));
        }

        [Test, Precondition]
        public void TaskTap_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task> tapper = null;

            // act, assert
            Assert.That(() => value.Tap(tapper),
                Throws.ArgumentNullException.With.Message.Contains("tapper"));
        }

        #endregion

        [Test(Description = "Tapping a None Option over a func should return a None Option " +
                            "and perform no action.")]
        public void FuncTap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = Sentinel;
            var actual = value.Tap(v => output = string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Option<string>.None));
            Assert.That(output, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Tapping a Some Option over a func should return a Some Option " +
                            "and perform an action.")]
        public void FuncTap_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var output = string.Empty;
            var actual = value.Tap(v => output = Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(value));
            Assert.That(output, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Tapping a None Option over a task should return a None Option " +
                            "and perform no action.")]
        public async Task TaskTap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = Sentinel;
            var actual = await value.Tap(v => Task.Run(() => output = string.Empty));

            // assert
            Assert.That(actual, Is.EqualTo(Option<string>.None));
            Assert.That(output, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Tapping a Some Option over a task should return a Some Option " +
                            "and perform an action.")]
        public async Task TaskTap_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var output = string.Empty;
            var actual = await value.Tap(v => Task.Run(() => output = Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(value));
            Assert.That(output, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Bind

        #region Null Throws

        [Test, Precondition]
        public void FuncBind_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Option<int>> binder = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Bind(binder));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("binder"));
        }

        [Test, Precondition]
        public void TaskBind_Null_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task<Option<int>>> binder = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.Bind(binder));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("binder"));
        }

        #endregion

        [Test(Description = "Binding a None Option over a func should return a None Option.")]
        public void FuncBind_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option<int>.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a func returning a None Option " +
                            "should return a None Option.")]
        public void FuncBind_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option<int>.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a func returning a Some Option " +
                            "should return a Some Option.")]
        public void FuncBind_ReturnSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option<int>.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(Sentinel.Length)));
        }

        [Test(Description = "Binding a Some Option over a task returning a None Option " +
                            "should return a None Option.")]
        public async Task TaskBind_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = await value.Bind(v =>
                Task.FromResult(v.Length == 0
                    ? Option<int>.None
                    : Option.From(v.Length)));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a task returning a Some Option " +
                            "should return a Some Option.")]
        public async Task TaskBind_ReturnSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Bind(v =>
                Task.FromResult(v.Length == 0
                    ? Option<int>.None
                    : Option.From(v.Length)));

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(Sentinel.Length)));
        }

        #endregion

        #region Other Useful Methods

        #region Null Throws

        [Test, Precondition]
        public void NullOther_ValueIfNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            string other = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.IfNone(other));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("other"));
        }

        [Test, Precondition]
        public void NullOther_FuncIfNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string> other = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.IfNone(other));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("other"));
        }

        [Test, Precondition]
        public void NullOther_TaskIfNone_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<Task<string>> other = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.IfNone(other));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("other"));
        }

        [Test, Precondition]
        public void NullAction_ActionIfSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Action<string> action = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => value.IfSome(action));

            // assert
            Assert.That(ex, Is.Not.Null.With.Message.Contains("action"));
        }

        [Test, Precondition]
        public void NullAction_TaskIfSome_Throws()
        {
            // arrange
            var value = Option<string>.None;
            Func<string, Task> action = null;

            // act, assert
            Assert.That(() => value.IfSome(action),
                Throws.ArgumentNullException.With.Message.Contains("action"));
        }

        #endregion

        [Test(Description = "Coalescing a None Option with an alternative value " +
                            "should return the alternative value.")]
        public void ValueIfNone_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IfNone(Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with an alternative value " +
                            "should return the Some value.")]
        public void ValueIfNone_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.IfNone(string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with a func producing an alternative value " +
                            "should return the alternative value.")]
        public void FuncIfNone_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IfNone(() => Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with a func producing an alternative value " +
                            "should return the Some value.")]
        public void FuncIfNone_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.IfNone(() => string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with a task producing an alternative value " +
                            "should return the alternative value.")]
        public async Task TaskIfNone_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.IfNone(() => Task.FromResult(Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with a task producing an alternative value " +
                            "should return the Some value.")]
        public async Task TaskIfNone_Some()
        {
            // arrange
            var value = Option.From(Sentinel);
            Func<Task<string>> other = () => Task.FromResult(string.Empty);

            // act
            var actual = await value.IfNone(other);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Conditionally executing an action based on a None Option " +
                            "should not execute.")]
        public void ActionIfSome_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Sentinel;
            value.IfSome(v => actual = string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Conditionally executing an action based on a Some Option " +
                            "should execute.")]
        public void ActionIfSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            value.IfSome(v => actual = v);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Conditionally executing a task based on a None Option " +
                            "should not execute.")]
        public async Task TaskIfSome_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Sentinel;
            await value.IfSome(v => Task.Run(() => actual = string.Empty));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Conditionally executing a task based on a Some Option " +
                            "should execute.")]
        public async Task TaskIfSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            await value.IfSome(v => Task.Run(() => actual = v));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        #endregion

        #region Overrides

        [Test(Description = "A None Option should stringify to None.")]
        [Category("Overload")]
        public void ToString_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo("None"));
        }

        [Test(Description = "A Some Option should stringify to a wrapped value.")]
        [Category("Overload")]
        public void ToString_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.That(actual, Is.EqualTo($"Some({Sentinel})"));
        }

        [Test(Description = "A None Option should not be equal to null.")]
        [Category("Overload")]
        public void ObjectEquals_NoneNull()
        {
            // arrange
            var left = Option<string>.None;
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A Some Option should not be equal to null.")]
        [Category("Overload")]
        public void ObjectEquals_SomeNull()
        {
            // arrange
            var left = Option.From(Sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Two None Options of the same type should be equal.")]
        [Category("Overload")]
        public void ObjectEquals_NoneNone_SameType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left.Equals((object)right);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Two None Options of different types should not be equal.")]
        [Category("Overload")]
        public void ObjectEquals_NoneNone_DifferentType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<int>.None;

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "A None Option and a Some Option of the same type should not be equal.")]
        [Category("Overload")]
        public void ObjectEquals_NoneSome_SameType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.Equals((object)right);
            var actualRightFirst = right.Equals((object)left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "A None Option and a Some Option of different types should not be equal.")]
        [Category("Overload")]
        public void ObjectEquals_NoneSome_DifferentType()
        {
            // arrange
            var left = Option<int>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options of the same type with different values should not be equal.")]
        [Category("Overload")]
        public void ObjectEquals_SomeSome_SameType_DifferentValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left.Equals((object)right);
            var actualRightFirst = right.Equals((object)left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options of the same type with the same values should be equal.")]
        [Category("Overload")]
        public void ObjectEquals_SomeSome_SameType_SameValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.Equals((object)right);
            var actualRightFirst = right.Equals((object)left);

            // assert
            Assert.That(actualLeftFirst, Is.True);
            Assert.That(actualRightFirst, Is.True);
        }

        [Test(Description = "Two Some Options of different types should be equal.")]
        [Category("Overload")]
        public void ObjectEquals_SomeSome_DifferentType()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From(0);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "A None Option should have a hashcode of 0.")]
        [Category("Overload")]
        public void GetHashCode_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "A Some Option should have the hashcode of its Some value.")]
        [Category("Overload")]
        public void GetHashCode_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.GetHashCode()));
        }

        #endregion

        #region Implementations

        [Test(Description = "Two None Options should be equal.")]
        [Category("Implementation")]
        public void EquatableEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A None Option and a Some Option should not be equal.")]
        [Category("Implementation")]
        public void EquatableEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options with different values should not be equal.")]
        [Category("Implementation")]
        public void EquatableEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options with the same values should be equal.")]
        [Category("Implementation")]
        public void EquatableEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.That(actualLeftFirst, Is.True);
            Assert.That(actualRightFirst, Is.True);
        }

        #endregion

        #region Operators and Named Alternatives

        [Test(Description = "Two None Options should be equal.")]
        [Category("Operator")]
        public void OperatorEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left == right;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A None Option and a Some Option should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options with different values should not be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "Two Some Options with the same values should be equal.")]
        [Category("Operator")]
        public void OperatorEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.That(actualLeftFirst, Is.True);
            Assert.That(actualRightFirst, Is.True);
        }

        [Test(Description = "Two None Options should not be unequal.")]
        [Category("Operator")]
        public void OperatorNotEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left != right;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "A None Option and a Some Option should be unequal.")]
        [Category("Operator")]
        public void OperatorNotEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.That(actualLeftFirst, Is.True);
            Assert.That(actualRightFirst, Is.True);
        }

        [Test(Description = "Two Some Options with different values should be unequal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.That(actualLeftFirst, Is.True);
            Assert.That(actualRightFirst, Is.True);
        }

        [Test(Description = "Two Some Options with the same values should not be unequal.")]
        [Category("Operator")]
        public void OperatorNotEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(Sentinel);
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.That(actualLeftFirst, Is.False);
            Assert.That(actualRightFirst, Is.False);
        }

        [Test(Description = "The disjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void OperatorBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The disjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void NamedBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The disjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void OperatorLogicalOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The disjunction of a None Option and a Some Option should be the Some Option.")]
        [Category("Operator")]
        public void OperatorBitwiseOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The disjunction of a None Option and a Some Option should be the Some Option.")]
        [Category("Operator")]
        public void NamedBitwiseOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The disjunction of a None Option and a Some Option should be the Some Option.")]
        [Category("Operator")]
        public void OperatorLogicalOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The disjunction of two Some Options should be the former Some Option.")]
        [Category("Operator")]
        public void OperatorBitwiseOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The disjunction of two Some Options should be the latter Some Option.")]
        [Category("Operator")]
        public void NamedBitwiseOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The disjunction of two Some Options should be the former Some Option.")]
        [Category("Operator")]
        public void OperatorLogicalOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "A None Option should not evaluate as true.")]
        [Category("Operator")]
        public void NamedIsTrue_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsTrue;

            // assert
            Assert.That(actual, Is.Not.True);
        }

        [Test(Description = "A Some Option should evaluate as true.")]
        [Category("Operator")]
        public void NamedIsTrue_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.IsTrue;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A None Option should evaluate as false.")]
        [Category("Operator")]
        public void NamedIsFalse_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsFalse;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "A Some Option should not evaluate as true.")]
        [Category("Operator")]
        public void NamedIsFalse_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.IsFalse;

            // assert
            Assert.That(actual, Is.Not.True);
        }

        [Test(Description = "The disjunction of a Some Option and a None Option should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalOr_SomeNone_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(Sentinel);
            string actual = string.Empty;
            Func<Option<string>> right = () =>
            {
                actual = Sentinel;
                return Option<string>.None;
            };

            // act
            var dummy = left || right();

            // assert
            Assert.That(actual, Is.Not.EqualTo(Sentinel));
            Assert.That(actual, Is.EqualTo(string.Empty));
        }

        [Test(Description = "The disjunction of two Some Options should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalOr_SomeSome_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(Sentinel);
            string actual = string.Empty;
            Func<Option<string>> right = () =>
            {
                actual = Sentinel;
                return Option.From(Sentinel);
            };

            // act
            var dummy = left || right();

            // assert
            Assert.That(actual, Is.Not.EqualTo(Sentinel));
            Assert.That(actual, Is.EqualTo(string.Empty));
        }

        [Test(Description = "The conjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void OperatorBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The conjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void NamedBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The conjunction of two None Options should be a None Option.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(Option<string>.None));
            Assert.That(actualRightFirst, Is.EqualTo(Option<string>.None));
        }

        [Test(Description = "The conjunction of a None Option and a Some Option should be a None Option.")]
        [Category("Operator")]
        public void OperatorBitwiseAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of a None Option and a Some Option should be a None Option.")]
        [Category("Operator")]
        public void NamedBitwiseAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of a None Option and a Some Option should be a None Option.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of two Some Options should be the latter Some Option.")]
        [Category("Operator")]
        public void OperatorBitwiseAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of two Some Options should be the latter Some Option.")]
        [Category("Operator")]
        public void NamedBitwiseAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of two Some Options should be the latter Some Option.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(Sentinel);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(left));
            Assert.That(actualRightFirst, Is.EqualTo(right));
        }

        [Test(Description = "The conjunction of two None Options should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_NoneNone_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = string.Empty;
            Func<Option<string>> right = () =>
            {
                actual = Sentinel;
                return Option<string>.None;
            };

            // act
            var dummy = left && right();

            // assert
            Assert.That(actual, Is.EqualTo(string.Empty));
            Assert.That(actual, Is.Not.EqualTo(Sentinel));
        }

        [Test(Description = "The disjunction of a None Option and a Some Option should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalOr_NoneSome_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = string.Empty;
            Func<Option<string>> right = () =>
            {
                actual = Sentinel;
                return Option.From(Sentinel);
            };

            // act
            var dummy = left && right();

            // assert
            Assert.That(actual, Is.EqualTo(string.Empty));
            Assert.That(actual, Is.Not.EqualTo(Sentinel));
        }

        #endregion

        #region Extensions

        [Test(Description = "A None Option converted to a Nullable should be null.")]
        [Category("Extension")]
        public void ToNullable_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.ToNullable();

            // assert
            Assert.That(actual, Is.Null);
        }

        [Test(Description = "A Some Option converted to a Nullable should be equal to the Some value.")]
        [Category("Extension")]
        public void ToNullable_Some()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.ToNullable();

            // assert
            Assert.That(actual, Is.Not.Null.And.EqualTo(42));
        }

        #endregion
    }
}
