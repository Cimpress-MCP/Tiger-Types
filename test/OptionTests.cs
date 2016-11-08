// ReSharper disable All

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static System.Threading.Tasks.Task;

namespace Tiger.Types.UnitTests
{
    /// <summary>Tests related to <see cref="Option{TSome}"/>.</summary>
    public sealed class OptionTests
    {
        const string sentinel = "sentinel";

        #region IsNone, IsSome

        [Theory(DisplayName = "Non-null values create Some Options using the untyped static From method.")]
        [InlineData(sentinel)]
        [InlineData("")]
        public void UntypedFrom_Value_IsSome(string innerValue)
        {
            // arrange, act
            var actual = Option.From(innerValue);

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Null values create None Options using the untyped static From method.")]
        public void UntypedFrom_Null_IsNone()
        {
            // arrange, act
            var actual = Option.From((string)null);

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Theory(DisplayName = "Non-null nullable values create Some Options.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        public void UntypedFrom_NullableValue_IsSome(int? innerValue)
        {
            // arrange, act
            var actual = Option.From(innerValue);

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Null nullable values create None Options.")]
        public void UntypedFrom_NullableNull_IsNone()
        {
            // arrange, act
            var actual = Option.From((int?)null);

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        #endregion

        #region Match

        [Fact(DisplayName = "Matching a None Option returns the None value branch, " +
                            "not the Some func branch.")]
        public void ValueFuncMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: 42,
                some: v => v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some func branch, " +
                            "not the None value branch.")]
        public void ValueFuncMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Match(
                none: 42,
                some: v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None value branch, " +
                            "not the Some task branch.")]
        public async Task ValueTaskMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some task branch, " +
                            "not the None value branch.")]
        public async Task ValueTaskMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Match(
                none: 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None func branch, " +
                            "not the Some func branch.")]
        public void FuncFuncMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => 42,
                some: v => v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some func branch, " +
                            "not the None func branch.")]
        public void FuncFuncMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Match(
                none: () => 42,
                some: v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None func branch, " +
                            "not the Some task branch.")]
        public async Task FuncTaskMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some task branch, " +
                            "not the None func branch.")]
        public async Task FuncTaskMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Match(
                none: () => 42,
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None task branch, " +
                            "not the Some func branch.")]
        public async Task TaskFuncMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => FromResult(42),
                some: v => v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some func branch, " +
                            "not the None task branch.")]
        public async Task TaskFuncMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Match(
                none: () => FromResult(42),
                some: v => v.Length);

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option returns the None task branch, " +
                            "not the Some task branch.")]
        public async Task TaskTaskMatchReturn_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Match(
                none: () => FromResult(42),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Matching a Some Option returns the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatchReturn_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Match(
                none: () => FromResult(42),
                some: v => v.Length.Pipe(FromResult));

            // assert
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Matching a None Option executes the None action branch, " +
                            "not the Some action branch.")]
        public void ActionActionMatchVoid_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            value.Match(
                none: () => actual = sentinel,
                some: v => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Some Option executes the Some action branch, " +
                            "not the None action branch.")]
        public void ActionActionMatchVoid_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            value.Match(
                none: () => { },
                some: v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a None Option executes the None action branch, " +
                            "not the Some task branch.")]
        public async Task ActionTaskMatchVoid_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => actual = sentinel,
                some: v => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Some Option executes the Some task branch, " +
                            "not the None action branch.")]
        public async Task ActionTaskMatchVoid_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => { },
                some: v => Run(() => actual = v));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a None Option executes the None task branch, " +
                            "not the Some action branch.")]
        public async Task TaskActionMatchVoid_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Run(() => actual = sentinel),
                some: v => { });

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Some Option executes the Some action branch, " +
                            "not the None task branch.")]
        public async Task TaskActionMatchVoid_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => CompletedTask,
                some: v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a None Option executes the None task branch, " +
                            "not the Some task branch.")]
        public async Task TaskTaskMatchVoid_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => Run(() => actual = sentinel),
                some: v => CompletedTask);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Matching a Some Option executes the Some task branch, " +
                            "not the None task branch.")]
        public async Task TaskTaskMatchVoid_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            await value.Match(
                none: () => CompletedTask,
                some: v => Run(() => actual = v));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Map

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public void FuncMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Map(v => v.Length);

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public void FuncMap_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Map(v => v.Length);

            // assert
            var length = Assert.Some(actual);
            Assert.Equal(sentinel.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public async Task TaskMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Map(v => v.Length.Pipe(FromResult));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public async Task TaskMap_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Map(v => v.Length.Pipe(FromResult));

            // assert
            var length = Assert.Some(actual);
            Assert.Equal(sentinel.Length, length);
        }

        #endregion

        #region Bind

        [Fact(DisplayName = "Binding a None Option over a func returns a None Option.")]
        public void FuncBind_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a None Option " +
                            "returns a None Option.")]
        public void FuncBind_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a Some Option " +
                            "returns a Some Option.")]
        public void FuncBindReturnSome_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Bind(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            var length = Assert.Some(actual);
            Assert.Equal(sentinel.Length, actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a task returning a None Option " +
                            "returns a None Option.")]
        public async Task TaskBind_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = await value.Bind(v =>
                FromResult(v.Length == 0
                    ? Option.None
                    : Option.From(v.Length)));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a task returning a Some Option " +
                            "returns a Some Option.")]
        public async Task TaskBindReturnSome_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Bind(v =>
                FromResult(v.Length == 0
                    ? Option.None
                    : Option.From(v.Length)));

            // assert
            var length = Assert.Some(actual);
            Assert.Equal(sentinel.Length, actual);
        }

        #endregion

        #region Filter

        [Fact(DisplayName = "Filtering a None Option produces a None Option.")]
        public void FuncFilter_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Filter(v => v > 0);

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public void FuncFilter_SomeFalse()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.Filter(_ => false);

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        public void FuncFilter_SomeTrue()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.Filter(_ => true);

            // assert
            var filteredValue = Assert.Some(actual);
            Assert.Equal(42, filteredValue);
        }

        [Fact(DisplayName = "Filtering a None Option produces a None Option.")]
        public async Task TaskFilter_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = await value.Filter(v => FromResult(v > 0));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public async Task TaskFilter_SomeFalse()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = await value.Filter(_ => FromResult(false));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a true predicate produces a None Option.")]
        public async Task TaskFilter_SomeTrue()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = await value.Filter(v => FromResult(true));

            // assert
            var filteredValue = Assert.Some(actual);
            Assert.Equal(42, filteredValue);
        }

        #endregion

        #region Fold

        [Fact(DisplayName = "Folding over a None Option returns the seed value.")]
        public void FuncFold_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator" +
                            "over the seed value and the Some value.")]
        public void FuncFold_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Fold(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Folding over a None Option returns the seed value.")]
        public async Task TaskFold_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Folding over a Some Option returns result of invoking the accumulator" +
                            "over the seed value and the Some value.")]
        public async Task TaskFold_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Fold(34, (s, v) => FromResult(s + v.Length));

            // assert
            Assert.Equal(42, actual);
        }

        #endregion

        #region Tap

        [Fact(DisplayName = "Tapping a None Option over a func returns a None Option " +
                            "and performs no action.")]
        public void FuncTap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = sentinel;
            var actual = value.Tap(v => output = string.Empty);

            // assert
            Assert.None(actual);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
                            "and performs an action.")]
        public void FuncTap_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var output = string.Empty;
            var actual = value.Tap(v => output = sentinel);

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a None Option over a task returns a None Option " +
                            "and performs no action.")]
        public async Task TaskTap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = sentinel;
            var actual = await value.Tap(v => Run(() => output = string.Empty));

            // assert
            Assert.None(actual);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a Some Option over a task returns a Some Option " +
                            "and performs an action.")]
        public async Task TaskTap_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var output = string.Empty;
            var actual = await value.Tap(v => Run(() => output = sentinel));

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(sentinel, innerValue);
            Assert.Equal(sentinel, output);
        }

        #endregion

        #region Let

        [Fact(DisplayName = "Conditionally executing an action based on a None Option does not execute.")]
        public void ActionLet_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = sentinel;
            value.Let(v => actual = string.Empty);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        public void ActionLet_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            value.Let(v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Conditionally executing a task based on a None Option does not execute.")]
        public async Task TaskLet_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = sentinel;
            await value.Let(v => Run(() => actual = string.Empty));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Conditionally executing a task based on a Some Option executes.")]
        public async Task TaskLet_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            await value.Let(v => Run(() => actual = v));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Recover

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        public void ValueRecover_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Recover(sentinel);

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a Some Option returns the original value.")]
        public void ValueRecover_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Recover("megatron");

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        public void FuncRecover_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Recover(() => sentinel);

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a Some Option returns the original value.")]
        public void FuncRecover_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Recover(() => "megatron");

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        public async Task TaskRecover_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.Recover(() => FromResult(sentinel));

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        [Fact(DisplayName = "Recovering a Some Option returns the original value.")]
        public async Task TaskRecover_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.Recover(() => FromResult("megatron"));

            // assert
            var recoveredValue = Assert.Some(actual);
            Assert.Equal(sentinel, recoveredValue);
        }

        #endregion

        #region Value

        [Fact(DisplayName = "Forcibly unwrapping a None Option throws.")]
        public void Value_None_Throws()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Record.Exception(() => value.Value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);
        }

        [Fact(DisplayName = "Forcibly unwrapping a Some Option returns the Some value.")]
        public void Value_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a None Option with an alternative value " +
                            "returns the alternative value.")]
        public void GetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(default(string), actual);
        }

        [Fact(DisplayName = "Coalescing a Some Option with an alternative value " +
                            "returns the Some value.")]
        public void GetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a None Option with an alternative value " +
                            "returns the alternative value.")]
        public void ValueGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Some Option with an alternative value " +
                            "returns the Some value.")]
        public void ValueGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.GetValueOrDefault(string.Empty);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a None Option with a func producing an alternative value " +
                            "returns the alternative value.")]
        public void FuncGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(() => sentinel);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Some Option with a func producing an alternative value " +
                            "returns the Some value.")]
        public void FuncGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.GetValueOrDefault(() => string.Empty);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a None Option with a task producing an alternative value " +
                            "returns the alternative value.")]
        public async Task TaskGetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = await value.GetValueOrDefault(() => FromResult(sentinel));

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Coalescing a Some Option with a task producing an alternative value " +
                            "returns the Some value.")]
        public async Task TaskGetValueOrDefault_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = await value.GetValueOrDefault(() => FromResult(string.Empty));

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Overrides

        [Fact(DisplayName = "A None Option stringifies to None.")]
        
        public void ToString_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("None", actual);
        }

        [Fact(DisplayName = "A Some Option stringifies to a wrapped value.")]
        
        public void ToString_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Some({sentinel})", actual);
        }

        [Fact(DisplayName = "A None Option is not equal to null.")]
        
        public void ObjectEquals_NoneNull()
        {
            // arrange
            var left = Option<string>.None;
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A Some Option is not equal to null.")]
        
        public void ObjectEquals_SomeNull()
        {
            // arrange
            var left = Option.From(sentinel);
            object right = null;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two None Options of the same type are equal.")]
        
        public void ObjectEquals_NoneNone_SameType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left.Equals(right);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Two None Options of different types are not equal.")]
        
        public void ObjectEquals_NoneNone_DifferentType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<int>.None;

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "A None Option and a Some Option of the same type are not equal.")]
        
        public void ObjectEquals_NoneSome_SameType()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "A None Option and a Some Option of different types are not equal.")]
        
        public void ObjectEquals_NoneSome_DifferentType()
        {
            // arrange
            var left = Option<int>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options of the same type with different values are not equal.")]
        
        public void ObjectEquals_SomeSome_SameType_DifferentValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options of the same type with the same values are equal.")]
        
        public void ObjectEquals_SomeSome_SameType_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options of different types are not equal.")]
        
        public void ObjectEquals_SomeSome_DifferentType()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(0);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "A None Option has a hashcode of 0.")]
        
        public void GetHashCode_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(0, actual);
        }

        [Fact(DisplayName = "A Some Option has the hashcode of its Some value.")]
        
        public void GetHashCode_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(sentinel.GetHashCode(), actual);
        }

        #endregion

        #region Implementations

        [Fact(DisplayName = "A None Option does not iterate.")]
        
        public void GetEnumerator_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = sentinel;
            foreach (var v in value)
            {
                actual = string.Empty;
            }

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "A Some Option iterates.")]
        
        public void GetEnumerator_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Operators and Named Alternates

        [Fact(DisplayName = "Two None Options are equal.")]
        public void OperatorEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option and a Some Option are not equal.")]
        public void OperatorEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are not equal.")]
        public void OperatorEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with the same values are equal.")]
        public void OperatorEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are not unequal.")]
        public void OperatorNotEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A None Option and a Some Option are unequal.")]
        public void OperatorNotEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are unequal.")]
        public void OperatorNotEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with the same values are not unequal.")]
        public void OperatorNotEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public void OperatorBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public void NamedBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public void OperatorLogicalOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public void OperatorBitwiseOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public void NamedBitwiseOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public void OperatorLogicalOr_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        public void OperatorBitwiseOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two Some Options is the latter Some Option.")]
        public void NamedBitwiseOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        public void OperatorLogicalOr_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "A None Option does not evaluate as true.")]
        public void OperatorIsTrue_None()
        {
            // arrange
            var value = Option<string>.None;
            bool actual;

            // act
            if (value)
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A None Option does not evaluate as true.")]
        public void NamedIsTrue_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsTrue;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "A Some Option evaluates as true.")]
        public void NamedIsTrue_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.IsTrue;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option evaluates as false.")]
        public void OperatorIsTrue_Some()
        {
            // arrange
            var value = Option.From(sentinel);
            bool actual;

            // act
            if (value)
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option evaluates as false.")]
        public void NamedIsFalse_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsFalse;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A Some Option does not evaluate as true.")]
        public void NamedIsFalse_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.IsFalse;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        public void NamedLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        public void OperatorLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = !value;

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "The logical inverse of a Some Option is false.")]
        
        public void NamedLogicalNot_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "The logical inverse of a Some Option is false.")]
        public void OperatorNot_None()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = !value;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "The disjunction of a Some Option and a None Option short-circuits.")]
        public void OperatorLogicalOr_SomeNone_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(sentinel);
            string actual = sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.None;
            };

            // act
            var dummy = left || right();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "The disjunction of two Some Options short-circuits.")]
        public void OperatorLogicalOr_SomeSome_ShortCircuits()
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(sentinel);
            string actual = sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.From(sentinel);
            };

            // act
            var dummy = left || right();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public void OperatorBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public void NamedBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public void OperatorLogicalAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.None(actualLeftFirst);
            Assert.None(actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public void OperatorBitwiseAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public void NamedBitwiseAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public void OperatorLogicalAnd_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public void OperatorBitwiseAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public void NamedBitwiseAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public void OperatorLogicalAnd_SomeSome()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Fact(DisplayName = "The conjunction of two None Options short-circuits.")]
        public void OperatorLogicalAnd_NoneNone_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.None;
            };

            // act
            var dummy = left && right();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "The conjunction of a None Option and a Some Option short-circuits.")]
        public void OperatorLogicalAnd_NoneSome_ShortCircuits()
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            string actual = sentinel;
            Func<Option<string>> right = () =>
            {
                actual = string.Empty;
                return Option.From(sentinel);
            };

            // act
            var dummy = left && right();

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "The untyped None converts to a None Option.")]
        public void LiteralNone_IsNone()
        {
            // arrange, act
            Option<string> actual = Option.None;

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Fact(DisplayName = "Null converts to a None Option.")]
        public void Null_IsNone()
        {
            // arrange, act
            Option<string> actual = null;

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Theory(DisplayName = "Values convert to Some Options.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        public void Value_IsSome(int innerValue)
        {
            // arrange, act
            Option<int> actual = innerValue;

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Unwrapping a None Option throws.")]
        public void Cast_None_Throws()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Record.Exception(() => (string)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);

        }

        [Fact(DisplayName = "Unwrapping a Some Option returns its Some value.")]
        public void Cast_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(sentinel, actual);
        }

        #endregion

        #region Split

        [Fact(DisplayName = "Splitting a null value over a func returns a None Option.")]
        public void FuncSplit_Null_None()
        {
            // arrange
            var value = (string)null;

            // act
            var actual = Option.Split(value, v => v.Length == 8);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a null nullable value over a func returns a None Option.")]
        public void FuncSplit_NullableNull_None()
        {
            // arrange
            var value = (int?)null;

            // act
            var actual = Option.Split(value, (int v) => v == 8);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a value over a func, failing the condition, " +
                            "returns a None Option.")]
        public void FuncSplit_ReturnFalse_None()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = Option.Split(value, v => v.Length == 33);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a nullable value over a func, failing the condition, " +
                            "returns a None Option.")]
        public void FuncSplit_NullableReturnFalse_None()
        {
            // arrange
            var value = (int?)42;

            // act
            var actual = Option.Split(value, (int v) => v == 33);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a value over a func, passing the condition, " +
                            "returns a Some Option.")]
        public void FuncSplit_Some()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = Option.Split(value, v => v.Length == 8);

            // assert
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Splitting a nullable value over a func, passing the condition, " +
                            "returns a Some Option.")]
        public void FuncSplit_Nullable_Some()
        {
            // arrange
            var value = (int?)42;

            // act
            var actual = Option.Split(value, (int v) => v == 42);

            // assert
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Splitting a null value over a task returns a None Option.")]
        public async Task TaskSplit_Null_None()
        {
            // arrange
            var value = (string)null;

            // act
            var actual = await Option.Split(value, v => FromResult(v.Length == 8));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a nullable null value over a task returns a None Option.")]
        public async Task TaskSplit_NullableNull_None()
        {
            // arrange
            var value = (int?)null;

            // act
            var actual = await Option.Split(value, (int v) => FromResult(v == 8));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a value over a task, failing the condition, " +
                            "returns a None Option.")]
        public async Task TaskSplit_ReturnFalse_None()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = await Option.Split(value, v => FromResult(v.Length == 33));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a nullable value over a task, failing the condition, " +
                            "returns a None Option.")]
        public async Task TaskSplit_NullableReturnFalse_None()
        {
            // arrange
            var value = (int?)42;

            // act
            var actual = await Option.Split(value, (int v) => FromResult(v == 33));

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Splitting a value over a func, passing the condition, " +
                            "returns a Some Option.")]
        public async Task TaskSplit_Some()
        {
            // arrange
            var value = sentinel;

            // act
            var actual = await Option.Split(value, v => FromResult(v.Length == 8));

            // assert
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Splitting a nullable value over a func, passing the condition, " +
                            "returns a Some Option.")]
        public async Task TaskSplit_Nullable_Some()
        {
            // arrange
            var value = (int?)42;

            // act
            var actual = await Option.Split(value, (int v) => FromResult(v == 42));

            // assert
            Assert.True(actual.IsSome);
        }

        #endregion

        #region Extensions

        [Fact(DisplayName = "A None Option converted to a Nullable is null.")]
        
        public void ToNullable_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.ToNullable();

            // assert
            Assert.Null(actual);
        }

        [Fact(DisplayName = "A Some Option converted to a Nullable is equal to the Some value.")]
        
        public void ToNullable_Some()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.ToNullable();

            // assert
            Assert.NotNull(actual);
            Assert.Equal(42, actual);
        }

        #region LINQ

        [Fact(DisplayName = "Asking a None Option for any returns false.")]
        
        public void Any_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any();

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some Option for any returns true.")]
        
        public void Any_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Any();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for any with a false predicate returns false.")]
        
        public void PredicateAny_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a None Option for any with a true predicate returns false.")]
        
        public void PredicateAny_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some Option for any with a false predicate returns false.")]
        
        public void PredicateAny_SomeFalse()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some Option for any with a true predicate returns false.")]
        
        public void PredicateAny_SomeTrue()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for all with a false predicate returns true.")]
        
        public void PredicateAll_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(_ => false);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for all with a true predicate returns true.")]
        
        public void PredicateAll_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(_ => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a Some Option for all with a false predicate returns false.")]
        
        public void PredicateAll_SomeFalse()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.All(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some Option for all with a true predicate returns true.")]
        
        public void PredicateAll_SomeTrue()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.All(_ => true);

            // assert
            Assert.True(actual);
        }

        [Theory(DisplayName = "Asking a None option whether it contains a value returns false.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        
        public void Contains_None(int testValue)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(testValue);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some option whether it contains a value that it doesn't" +
                            "returns false.")]
        
        public void Contains_Some_False()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Contains("megatron");

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some option whether it contains a value that it does" +
                            "returns true.")]
        
        public void Contains_Some_True()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Contains(sentinel);

            // assert
            Assert.True(actual);
        }

        [Theory(DisplayName = "Asking a None option whether it contains a value returns false.")]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-1)]
        
        public void ComparerContains_None(int testValue)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(testValue, EqualityComparer<int>.Default);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some option whether it contains a value that it doesn't" +
                            "returns false.")]
        
        public void ComparerContains_Some_False()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Contains("megatron", StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a Some option whether it contains a value that it does" +
                            "returns true.")]
        
        public void ComparerContains_Some_True()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Contains(sentinel.ToUpperInvariant(), StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        
        public void DefaultIfEmpty_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(0, innerValue);
        }

        [Fact(DisplayName = "Recovering a Some Option returns the Some value.")]
        
        public void DefaultIfEmpty_Some()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        
        public void ValueDefaultIfEmpty_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.DefaultIfEmpty(sentinel);

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Recovering a Some Option returns the Some value.")]
        
        public void ValueDefaultIfEmpty_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.DefaultIfEmpty("megatron");

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(sentinel, innerValue);
        }

        [Fact(DisplayName = "Tapping a None Option over a func returns a None Option " +
                            "and performs no action.")]
        
        public void Do_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = sentinel;
            var actual = value.Do(v => output = string.Empty);

            // assert
            Assert.None(actual);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
                            "and performs an action.")]
        
        public void Do_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var output = string.Empty;
            var actual = value.Do(v => output = sentinel);

            // assert
            Assert.Equal(value, actual);
            Assert.Equal(sentinel, output);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a None Option " +
                            "does not execute.")]
        
        public void ForEach_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = sentinel;
            value.ForEach(v => actual = string.Empty);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Conditionally executing an action based on a Some Option " +
                            "executes.")]
        
        public void ForEach_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = string.Empty;
            value.ForEach(v => actual = v);

            // assert
            Assert.Equal(sentinel, actual);
        }

        [Fact(DisplayName = "Selecting a None Option produces a None Option.")]
        
        public void Select_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Selecting a Some Option produces a Some Option.")]
        
        public void Select_Some()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(43, innerValue);
        }

        [Fact(DisplayName = "Filtering a None Option produces a None Option.")]
        
        public void Where_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         where v <= 0
                         select v;

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        
        public void Where_SomeFalse()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         where v <= 0
                         select v;

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Filtering a Some Option with a true predicate produces a None Option.")]
        
        public void Where_SomeTrue()
        {
            // arrange
            var value = Option.From(42);

            // act
            var actual = from v in value
                         where v > 0
                         select v;

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(42, innerValue);
        }

        [Fact(DisplayName = "Selecting from two None Options produces a None Option.")]
        
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
            Assert.None(actual);
        }

        [Fact(DisplayName = "Selecting from a Some Option and a None Option produces a None Option.")]
        
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
            Assert.None(actual);
        }

        [Fact(DisplayName = "Selecting from a None Option and a Some Option produces a None Option.")]
        
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
            Assert.None(actual);
        }

        [Fact(DisplayName = "Selecting from two Some Options produces a Some Option.")]
        
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
            var innerValue = Assert.Some(actual);
            Assert.Equal(3, innerValue);
        }

        [Fact(DisplayName = "Binding a None Option over a func returns a None Option.")]
        public void SelectMany_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a None Option " +
                            "returns a None Option.")]
        public void SelectMany_ReturnNone_Some()
        {
            // arrange
            var value = Option.From(string.Empty);

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            Assert.None(actual);
        }

        [Fact(DisplayName = "Binding a Some Option over a func returning a Some Option " +
                            "returns a Some Option.")]
        public void SelectManyReturnSome_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.SelectMany(v => v.Length == 0 ? Option.None : Option.From(v.Length));

            // assert
            var innerValue = Assert.Some(actual);
            Assert.Equal(sentinel.Length, innerValue);
        }

        [Fact(DisplayName = "Folding over a None Option returns the seed value.")]
        
        public void Aggregate_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Aggregate(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(34, actual);
        }

        [Fact(DisplayName = "Folding over a Some Option returns the result of invoking the " +
                            "accumulator over the seed value and the Some value.")]
        
        public void Aggregate_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Aggregate(34, (s, v) => s + v.Length);

            // assert
            Assert.Equal(42, actual);
        }

        [Fact(DisplayName = "Folding over a None Option returns the seed value.")]
        
        public void ResultAggregate_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Aggregate(34, (s, v) => s + v.Length, v => v * 2);

            // assert
            Assert.Equal(68, actual);
        }

        [Fact(DisplayName = "Folding over a Some Option returns the result of invoking the " +
                            "accumulator over the seed value and the Some value.")]
        
        public void ResultAggregate_Some()
        {
            // arrange
            var value = Option.From(sentinel);

            // act
            var actual = value.Aggregate(34, (s, v) => s + v.Length, v => v * 2);

            // assert
            Assert.Equal(84, actual);
        }

        #endregion

        #endregion

        #region Get Underlying Type

        [InlineData(typeof(Option<int>), typeof(int))]
        [InlineData(typeof(Option<string>), typeof(string))]
        [InlineData(typeof(int), null)]
        [InlineData(typeof(string), null)]
        [InlineData(typeof(List<int>), null)]
        [InlineData(typeof(Option<>), null)]
        [InlineData(typeof(List<>), null)]
        
        public void GetUnderlyingType(Type optionalType, Type expected)
        {
            // arrange

            // act
            var actual = Option.GetUnderlyingType(optionalType);

            // assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Comparisons

        [Fact(DisplayName = "Two None Options are equal.")]
        
        public void StaticEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = Option.OptionalEquals(left, right);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option and a Some Option are not equal.")]
        
        public void StaticEquals_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are not equal.")]
        
        public void StaticEquals_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with the same values are equal.")]
        
        public void StaticEquals_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        
        public void StaticEqualsComparer_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "A None Option and a Some Option are not equal.")]
        
        public void StaticEqualsComparer_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are not equal.")]
        
        public void StaticEqualsComparer_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From("megatron");

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with the same values are equal.")]
        
        public void StaticEqualsComparer_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel.ToUpper());

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        
        public void StaticCompare_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(0, actualLeftFirst);
            Assert.Equal(0, actualRightFirst);
        }

        [Fact(DisplayName = "A None Option and a Some Option are not equal.")]
        
        public void StaticCompare_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are not equal.")]
        
        public void StaticCompare_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        public void StaticCompare_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(0, actualLeftFirst);
            Assert.Equal(0, actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        
        public void StaticCompareComparer_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.Equal(0, actualLeftFirst);
            Assert.Equal(0, actualRightFirst);
        }

        [Fact(DisplayName = "A None Option and a Some Option are not equal.")]
        
        public void StaticCompareComparer_NoneSome()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with different values are not equal.")]
        
        public void StaticCompareComparer_SomeSome_DifferentValue()
        {
            // arrange
            var left = Option.From("megatron");
            var right = Option.From(sentinel);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        [Fact(DisplayName = "Two Some Options with the same values are equal.")]
        public void StaticCompareComparer_SomeSome_SameValue()
        {
            // arrange
            var left = Option.From(sentinel);
            var right = Option.From(sentinel.ToUpper());

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.Equal(0, actualLeftFirst);
            Assert.Equal(0, actualRightFirst);
        }

        #endregion
    }
}
