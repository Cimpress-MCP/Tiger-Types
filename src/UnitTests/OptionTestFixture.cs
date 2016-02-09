// ReSharper disable All

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Tiger.Types.Properties;

namespace Tiger.Types.UnitTests
{
    /* note(cosborn)
     * NUnit doesn't have good support for async in Assert.Throws<T>, so we
     * work around it where necessary using the accidental support in Assert.That.
     */
    /// <summary>Tests related to <see cref="Option{TSome}"/>.</summary>
    [TestFixture(TestOf = typeof(Option<>))]
    public sealed class OptionTestFixture
    {
        const string Sentinel = "sentinel";

        #region IsNone, IsSome

        [Test(Description = "Non-null values should create Some Options using the typed static From method.")]
        [TestCase(Sentinel)]
        [TestCase("")]
        public void TypedFrom_Value_IsSome(string innerValue)
        {
            // arrange, act
            var actual = Option<string>.From(innerValue);

            // assert
            Assert.That(actual.IsNone, Is.False);
            Assert.That(actual.IsSome, Is.True);
        }

        [Test(Description = "Null values should create None Options using the typed static From method.")]
        public void TypedFrom_Null_IsNone()
        {
            // arrange, act
            var actual = Option<string>.From(null);

            // assert
            Assert.That(actual.IsNone, Is.True);
            Assert.That(actual.IsSome, Is.False);
        }

        [Test(Description = "Non-null values should create Some Options using the untyped static From method.")]
        [TestCase(Sentinel)]
        [TestCase("")]
        public void UntypedFrom_Value_IsSome(string innerValue)
        {
            // arrange, act
            var actual = Option.From(innerValue);

            // assert
            Assert.That(actual.IsNone, Is.False);
            Assert.That(actual.IsSome, Is.True);
        }

        [Test(Description = "Null values should create None Options using the untyped static From method.")]
        public void UntypedFrom_Null_IsNone()
        {
            // arrange, act
            var actual = Option.From((string)null);

            // assert
            Assert.That(actual.IsNone, Is.True);
            Assert.That(actual.IsSome, Is.False);
        }

        [Test(Description = "Non-null nullable values should create Some Options.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        public void UntypedFrom_NullableValue_IsSome(int? innerValue)
        {
            // arrange, act
            var actual = Option.From(innerValue);

            // assert
            Assert.That(actual.IsNone, Is.False);
            Assert.That(actual.IsSome, Is.True);
        }

        [Test(Description = "Null nullable values should create None Options.")]
        public void UntypedFrom_NullableNull_IsNone()
        {
            // arrange, act
            var actual = Option.From((int?)null);

            // assert
            Assert.That(actual.IsNone, Is.True);
            Assert.That(actual.IsSome, Is.False);
        }

        #endregion

        #region Match

        [Test(Description = "Matching a None Option should return the None value branch, " +
                            "not the Some func branch.")]
        public void ValueFuncMatchReturn_None()
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
        public void ValueFuncMatchReturn_Some()
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
        public async Task ValueTaskMatchReturn_None()
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
        public async Task ValueTaskMatchReturn_Some()
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
        public void FuncFuncMatchReturn_None()
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
        public void FuncFuncMatchReturn_Some()
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
        public async Task FuncTaskMatchReturn_None()
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
        public async Task FuncTaskMatchReturn_Some()
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
        public async Task TaskFuncMatchReturn_None()
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
        public async Task TaskFuncMatchReturn_Some()
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
        public async Task TaskTaskMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test(Description = "Matching a Some Option should return the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatchReturn_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Match(
                none: () => Task.FromResult(0),
                some: v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length));
        }

        [Test(Description = "Matching a None Option should execute the None action branch, " +
                            "not the Some action branch.")]
        public void ActionActionMatchVoid_None()
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
        public void ActionActionMatchVoid_Some()
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
        public async Task ActionTaskMatchVoid_None()
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
        public async Task ActionTaskMatchVoid_Some()
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
        public async Task TaskActionMatchVoid_None()
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
        public async Task TaskActionMatchVoid_Some()
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
        public async Task TaskTaskMatchVoid_None()
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
        public async Task TaskTaskMatchVoid_Some()
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
            Assert.That(actual, Is.EqualTo(Sentinel.Length.Pipe(Option.From)));
        }

        [Test(Description = "Mapping a None Option over a task should return a None Option.")]
        public async Task TaskMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Map(v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Mapping a Some Option over a task should return a Some Option.")]
        public async Task TaskMap_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Map(v => v.Length.Pipe(Task.FromResult));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length.Pipe(Option.From)));
        }

        #endregion

        #region Tap

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

        #region Filter

        [Test(Description = "Filtering a None Option should produce a None Option.")]
        [Category("Extension")]
        public void Filter_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Filter(v => v <= 0);

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Filtering a Some Option with a false predicate should produce a None Option.")]
        [Category("Extension")]
        public void Filter_SomeFalse()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.Filter(v => v <= 0);

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Filtering a Some Option with a true predicate should produce a None Option.")]
        [Category("Extension")]
        public void Filter_SomeTrue()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.Filter(v => v > 0);

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(42)));
        }

        #endregion

        #region Fold

        #endregion

        #region Bind

        [Test(Description = "Binding a None Option over a func should return a None Option.")]
        public void FuncBind_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

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
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a func returning a Some Option " +
                            "should return a Some Option.")]
        public void FuncBindReturnSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length.Pipe(Option.From)));
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
                    ? Option.None
                    : Option.From(v.Length)));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a task returning a Some Option " +
                            "should return a Some Option.")]
        public async Task TaskBindReturnSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = await value.Bind(v =>
                Task.FromResult(v.Length == 0
                    ? Option.None
                    : Option.From(v.Length)));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length.Pipe(Option.From)));
        }

        #endregion

        #region Other Useful Methods

        [Test(Description = "Forcibly unwrapping a None Option should throw.")]
        public void Value_None()
        {
            // arrange
            var value = Option<string>.None;

            // act, assert
            var actual = Sentinel;
            try
            {
                actual = value.Value;
            }
            catch (InvalidOperationException ioe)
            {
                Assert.That(ioe, Has.Message.EqualTo(Resources.OptionIsNone));
            }
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Forcibly unwrapping a Some Option should return the Some value.")]
        public void Value_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Value;
            
            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with an alternative value " +
                            "should return the alternative value.")]
        public void GetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.That(actual, Is.EqualTo(default(string)));
        }

        [Test(Description = "Coalescing a None Option with an alternative value " +
                            "should return the alternative value.")]
        public void GetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with an alternative value " +
                            "should return the alternative value.")]
        public void ValueGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with an alternative value " +
                            "should return the Some value.")]
        public void ValueGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.GetValueOrDefault(string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with a func producing an alternative value " +
                            "should return the alternative value.")]
        public void FuncGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(() => Sentinel);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with a func producing an alternative value " +
                            "should return the Some value.")]
        public void FuncGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.GetValueOrDefault(() => string.Empty);

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a None Option with a task producing an alternative value " +
                            "should return the alternative value.")]
        public async Task TaskGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.GetValueOrDefault(() => Task.FromResult(Sentinel));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Coalescing a Some Option with a task producing an alternative value " +
                            "should return the Some value.")]
        public async Task TaskGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(Sentinel);
            Func<Task<string>> other = () => Task.FromResult(string.Empty);

            // act
            var actual = await value.GetValueOrDefault(other);

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

        [Test(Description = "A None Option should not iterate.")]
        [Category("Implementation")]
        public void ForEach_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Sentinel;
            foreach (var v in value)
            {
                actual = string.Empty;
            }

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "A Some Option should not iterate.")]
        [Category("Implementation")]
        public void ForEach_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = string.Empty;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
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
        public void OperatorIsTrue_None()
        {
            // arrange
            var value = Option<string>.None;

            // act, assert
            if (value)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
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
            Assert.That(actual, Is.False);
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

        [Test(Description = "A Some Option should evaluate as true.")]
        [Category("Operator")]
        public void OperatorIsTrue_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act, assert
            if (value)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
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
            Assert.That(actual, Is.False);
        }

        [Test(Description = "The logical inverse of a None Option should be true.")]
        [Category("Operator")]
        public void NamedLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "The logical inverse of a None Option should be true.")]
        [Category("Operator")]
        public void OperatorLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = !value;

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "The logical inverse of a Some Option should be false.")]
        [Category("Operator")]
        public void NamedLogicalNot_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "The logical inverse of a Some Option should be false.")]
        [Category("Operator")]
        public void OperatorNot_None()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = !value;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "The disjunction of a Some Option and a None Option should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalOr_SomeNone_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(Sentinel);
            string actual = Sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.None;
            };

            // act
            var dummy = left || right();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "The disjunction of two Some Options should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalOr_SomeSome_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(Sentinel);
            string actual = Sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.From(Sentinel);
            };

            // act
            var dummy = left || right();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
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
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.That(actualLeftFirst, Is.EqualTo(right));
            Assert.That(actualRightFirst, Is.EqualTo(left));
        }

        [Test(Description = "The conjunction of two None Options should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_NoneNone_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = Sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.None;
            };

            // act
            var dummy = left && right();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "The conjunction of a None Option and a Some Option should short-circuit.")]
        [Category("Operator")]
        public void OperatorLogicalAnd_NoneSome_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = Sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.From(Sentinel);
            };

            // act
            var dummy = left && right();

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "The untyped None should convert to a None Option.")]
        public void LiteralNone_IsNone()
        {
            // arrange, act
            Option<string> actual = Option.None;

            // assert
            Assert.That(actual.IsNone, Is.True);
            Assert.That(actual.IsSome, Is.False);
        }

        [Test(Description = "Null should convert to a None Option.")]
        public void Null_IsNone()
        {
            // arrange, act
            Option<string> actual = null;

            // assert
            Assert.That(actual.IsNone, Is.True);
            Assert.That(actual.IsSome, Is.False);
        }

        [Test(Description = "Values should convert to Some Options.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        public void Value_IsSome(int innerValue)
        {
            // arrange, act
            Option<int> actual = innerValue;

            // assert
            Assert.That(actual.IsNone, Is.False);
            Assert.That(actual.IsSome, Is.True);
        }

        [Test(Description = "Unwrapping a None Option should throw.")]
        public void Cast_None_Throws()
        {
            // arrange
            var value = Option<string>.None;

            // act, assert
            var actual = Sentinel;
            try
            {
                actual = (string)value;
            }
            catch (InvalidOperationException ioe)
            {
                Assert.That(ioe, Has.Message.EqualTo(Resources.OptionIsNone));
            }
            Assert.That(actual, Is.EqualTo(Sentinel));
        }

        [Test(Description = "Unwrapping a Some Option should return its Some value.")]
        public void Cast_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel));
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

        #region LINQ

        [Test(Description = "Asking a None Option for any should return false.")]
        [Category("Extension")]
        public void Any_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any();

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some Option for any should return true.")]
        [Category("Extension")]
        public void Any_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a None Option for any with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(v => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a None Option for any with a true predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(v => true);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some Option for any with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_SomeFalse()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some Option for any with a true predicate should return false.")]
        [Category("Extension")]
        public void PredicateAny_SomeTrue()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a None Option for all with a false predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(v => false);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a None Option for all with a true predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(v => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a Some Option for all with a false predicate should return false.")]
        [Category("Extension")]
        public void PredicateAll_SomeFalse()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.All(_ => false);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some Option for all with a true predicate should return true.")]
        [Category("Extension")]
        public void PredicateAll_SomeTrue()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.All(_ => true);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a None option whether it contains a value should return false.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        [Category("Extension")]
        public void None_Contains(int testValue)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(testValue);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some option whether it contains a value that it doesn't" +
                            "should return false.")]
        [Category("Extension")]
        public void Some_Contains_False()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Contains("megatron");

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some option whether it contains a value that it does" +
                            "should return true.")]
        [Category("Extension")]
        public void Some_Contains_True()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Contains(Sentinel);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Asking a None option whether it contains a value should return false.")]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(-1)]
        [Category("Extension")]
        public void None_ComparerContains(int testValue)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(testValue, EqualityComparer<int>.Default);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some option whether it contains a value that it doesn't" +
                            "should return false.")]
        [Category("Extension")]
        public void Some_ComparerContains_False()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Contains("megatron", StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.That(actual, Is.False);
        }

        [Test(Description = "Asking a Some option whether it contains a value that it does" +
                            "should return true.")]
        [Category("Extension")]
        public void Some_ComparerContains_True()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.Contains(Sentinel.ToUpperInvariant(), StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.That(actual, Is.True);
        }

        [Test(Description = "Selecting a None Option should produce a None Option.")]
        [Category("Extension")]
        public void Select_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Selecting a Some Option should produce a Some Option.")]
        [Category("Extension")]
        public void Select_Some()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(43)));
        }

        [Test(Description = "Filtering a None Option should produce a None Option.")]
        [Category("Extension")]
        public void Where_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         where v <= 0
                         select v;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Filtering a Some Option with a false predicate should produce a None Option.")]
        [Category("Extension")]
        public void Where_SomeFalse()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         where v <= 0
                         select v;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Filtering a Some Option with a true predicate should produce a None Option.")]
        [Category("Extension")]
        public void Where_SomeTrue()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         where v > 0
                         select v;

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(42)));
        }

        [Test(Description = "Selecting from two None Options should produce a None Option.")]
        [Category("Extension")]
        public void SelectManyResult_NoneNone()
        {
            // arrange
            var left = Option<int>.None;
            var right = Option<int>.None;

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Selecting from a Some Option and a None Option should produce a None Option.")]
        [Category("Extension")]
        public void SelectManyResult_SomeNone()
        {
            // arrange
            var left = Option.From(1);
            var right = Option<int>.None;

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Selecting from a None Option and a Some Option should produce a None Option.")]
        [Category("Extension")]
        public void SelectManyResult_NoneSome()
        {
            // arrange
            var left = Option<int>.None;
            var right = Option.From(2);

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Selecting from two Some Options should produce a Some ")]
        [Category("Extension")]
        public void SelectManyResult_SomeSome()
        {
            // arrange
            var left = Option.From(1);
            var right = Option.From(2);

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.That(actual, Is.EqualTo(Option.From(3)));
        }

        [Test(Description = "Binding a None Option over a func should return a None Option.")]
        public void SelectMany_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a func returning a None Option " +
                            "should return a None Option.")]
        public void SelectMany_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Option<int>.None));
        }

        [Test(Description = "Binding a Some Option over a func returning a Some Option " +
                            "should return a Some Option.")]
        public void SelectManyReturnSome_Some()
        {
            // arrange
            var value = Option.From(Sentinel);

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.That(actual, Is.EqualTo(Sentinel.Length.Pipe(Option.From)));
        }

        #endregion

        #endregion
    }
}
