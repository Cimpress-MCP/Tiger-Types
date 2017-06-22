using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Option{TSome}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) })]
    public static class OptionTests
    {
        #region IsNone, IsSome

        [Property(DisplayName = "Non-null values create Some Options using the untyped static From method.")]
        public static void UntypedFrom_Value_IsSome(NonNull<string> some)
        {
            // arrange, act
            var actual = Option.From(some.Get);

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null values create None Options using the untyped static From method.")]
        public static void UntypedFrom_Null_IsNone()
        {
            // arrange, act
            var actual = Option.From((string)null);

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Property(DisplayName = "Non-null nullable values create Some Options.")]
        public static void UntypedFrom_NullableValue_IsSome(int some)
        {
            // arrange, act
            var actual = Option.From((int?)some);

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null nullable values create None Options.")]
        public static void UntypedFrom_NullableNull_IsNone()
        {
            // arrange, act
            var actual = Option.From((int?)null);

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        #endregion

        #region Match

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
            "not the Some func branch.")]
        public static void ValueFuncMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: noneValue,
                some: v => v.Length);

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None value branch.")]
        public static void ValueFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: noneValue,
                some: v => v.Length);

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None value branch, " +
            "not the Some task branch.")]
        public static void ValueTaskMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None value branch.")]
        public static void ValueTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: noneValue,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some func branch.")]
        public static void FuncFuncMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => noneValue,
                some: v => v.Length);

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None func branch.")]
        public static void FuncFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: () => noneValue,
                some: v => v.Length);

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None func branch, " +
            "not the Some task branch.")]
        public static void FuncTaskMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None func branch.")]
        public static void FuncTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: () => noneValue,
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some func branch.")]
        public static void TaskFuncMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length).Result;

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some func branch, " +
            "not the None task branch.")]
        public static void TaskFuncMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option returns the None task branch, " +
            "not the Some task branch.")]
        public static void TaskTaskMatchReturn_None(int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(noneValue, actual);
        }

        [Property(DisplayName = "Matching a Some Option returns the Some task branch, " +
            "not the None task branch.")]
        public static void TaskTaskMatchReturn_Some(NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Match(
                none: () => FromResult(noneValue),
                some: v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.Equal(some.Get.Length, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
            "not the Some action branch.")]
        public static void ActionActionMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            var unit = value.Match(
                none: () => actual = sentinel.Get,
                some: v => { });

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, not the None action branch.")]
        public static void ActionActionMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            var unit = value.Match(
                none: () => { },
                some: v => actual = v);

            // assert
            Assert.Equal(Unit.Value, unit);
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None action branch, " +
            "not the Some task branch.")]
        public static void ActionTaskMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.Match(
                none: () => actual = sentinel.Get,
                some: v => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
            "not the None action branch.")]
        public static void ActionTaskMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.Match(
                none: () => { },
                some: v => Run(() => actual = v)).Wait();

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
            "not the Some action branch.")]
        public static void TaskActionMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.Match(
                none: () => Run(() => actual = sentinel.Get),
                some: v => { }).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some action branch, " +
            "not the None task branch.")]
        public static void TaskActionMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.Match(
                none: () => CompletedTask,
                some: v => actual = v).Wait();

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Matching a None Option executes the None task branch, " +
            "not the Some task branch.")]
        public static void TaskTaskMatchVoid_None(NonNull<string> before, NonNull<string> sentinel, int noneValue)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.Match(
                none: () => Run(() => actual = sentinel.Get),
                some: v => CompletedTask).Wait();

            // assert
            Assert.Equal(sentinel.Get, actual);
        }

        [Property(DisplayName = "Matching a Some Option executes the Some task branch, " +
            "not the None task branch.")]
        public static void TaskTaskMatchVoid_Some(NonNull<string> before, NonNull<string> some, int noneValue)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.Match(
                none: () => CompletedTask,
                some: v => Run(() => actual = v)).Wait();

            // assert
            Assert.Equal(some.Get, actual);
        }

        #endregion

        #region Map

        [Fact(DisplayName = "Mapping a None Option over a func returns a None Option.")]
        public static void FuncMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Map(v => v.Length);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a func returns a Some Option.")]
        public static void FuncMap_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Map(v => v.Length);

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Fact(DisplayName = "Mapping a None Option over a task returns a None Option.")]
        public static void TaskMap_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Map(v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Mapping a Some Option over a task returns a Some Option.")]
        public static void TaskMap_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Map(v => v.Length.Pipe(FromResult)).Result;

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        #endregion

        #region Bind

        [Fact(DisplayName = "Binding a None Option over a func returning a None Option returns a None Option.")]
        public static void FuncBind_ReturnNone_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Bind(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Binding a None Option over a func returning a Some Option returns a None Option.")]
        public static void FuncBind_ReturnSome_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Bind(v => Option.From(v.Length));

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option " +
            "returns a None Option.")]
        public static void FuncBind_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Bind(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option " +
            "returns a Some Option.")]
        public static void FuncBindReturnSome_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Bind(v => Option.From(v.Length));

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a None Option " +
            "returns a None Option.")]
        public static void TaskBind_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Bind(_ => FromResult(Option<int>.None)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a task returning a Some Option " +
            "returns a Some Option.")]
        public static void TaskBindReturnSome_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Bind(v => FromResult(Option.From(v.Length))).Result;

            // assert
            Assert.True(actual.IsSome);
            var length = actual.Value;
            Assert.Equal(some.Get.Length, length);
        }

        #endregion

        #region Filter

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static void FuncFilter_NoneFalse(bool filter)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Filter(_ => filter);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static void FuncFilter_SomeFalse(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.Filter(_ => false);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        public static void FuncFilter_SomeTrue(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.Filter(_ => true);

            // assert
            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static void TaskFilter_None(bool filter)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Filter(v => FromResult(filter)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static void TaskFilter_SomeFalse(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.Filter(_ => FromResult(false)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a None Option.")]
        public static void TaskFilter_SomeTrue(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.Filter(v => FromResult(true)).Result;

            // assert
            Assert.True(actual.IsSome);
            var filteredValue = actual.Value;
            Assert.Equal(some, filteredValue);
        }

        #endregion

        #region Fold

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void FuncFold_None(int seed)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Fold(seed, (s, v) => s + v.Length);

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the accumulator" +
            "over the seed value and the Some value.")]
        public static void FuncFold_Some(NonNull<string> some, int seed)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Fold(seed, (s, v) => s + v.Length);

            // assert
            Assert.Equal(seed + some.Get.Length, actual);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void TaskFold_None(int seed)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Fold(seed, (s, v) => FromResult(s + v.Length)).Result;

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns result of invoking the accumulator" +
            "over the seed value and the Some value.")]
        public static void TaskFold_Some(NonNull<string> some, int seed)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Fold(seed, (s, v) => FromResult(s + v.Length)).Result;

            // assert
            Assert.Equal(seed + some.Get.Length, actual);
        }

        #endregion

        #region Tap

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option " +
            "and performs no action.")]
        public static void FuncTap_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = before.Get;
            var actual = value.Tap(v => output = sentinel.Get);

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
            "and performs an action.")]
        public static void FuncTap_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var output = before.Get;
            var actual = value.Tap(v => output = sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Tapping a None Option over a task returns a None Option " +
            "and performs no action.")]
        public static void TaskTap_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = before.Get;
            var actual = value.Tap(v => Run(() => output = sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a task returns a Some Option " +
            "and performs an action.")]
        public static void TaskTap_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var output = before.Get;
            var actual = value.Tap(v => Run(() => output = sentinel.Get)).Result;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
            Assert.Equal(sentinel.Get, output);
        }

        #endregion

        #region Let

        [Property(DisplayName = "Conditionally executing an action based on a None Option does not execute.")]
        public static void ActionLet_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.Let(v => actual = sentinel.Get);

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        public static void ActionLet_Some(NonNull<string> some, NonNull<string> before)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.Let(v => actual = v);

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing a task based on a None Option does not execute.")]
        public static void TaskLet_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.Let(v => Run(() => actual = sentinel.Get)).Wait();

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing a task based on a Some Option executes.")]
        public static void TaskLet_Some(NonNull<string> some, NonNull<string> before)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.Let(v => Run(() => actual = v)).Wait();

            // assert
            Assert.Equal(some.Get, actual);
        }

        #endregion

        #region Recover

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void ValueRecover_None(NonNull<string> recovery)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Recover(recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static void ValueRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Recover(recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void FuncRecover_None(NonNull<string> recovery)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Recover(() => recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static void FuncRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Recover(() => recovery.Get);

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void TaskRecover_None(NonNull<string> recovery)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Recover(() => FromResult(recovery.Get)).Result;

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(recovery.Get, recoveredValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the original value.")]
        public static void TaskRecover_Some(NonNull<string> some, NonNull<string> recovery)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Recover(() => FromResult(recovery.Get)).Result;

            // assert
            Assert.True(actual.IsSome);
            var recoveredValue = actual.Value;
            Assert.Equal(some.Get, recoveredValue);
        }

        #endregion

        #region Value

        [Fact(DisplayName = "Forcibly unwrapping a None Option throws.")]
        public static void Value_None_Throws()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Record.Exception(() => value.Value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);
        }

        [Property(DisplayName = "Forcibly unwrapping a Some Option returns the Some value.")]
        public static void Value_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Value;

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Fact(DisplayName = "Coalescing a None Option with an alternative value " +
            "returns the alternative value.")]
        public static void GetValueOrDefault_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(default(string), actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with an alternative value " +
            "returns the Some value.")]
        public static void GetValueOrDefault_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.GetValueOrDefault();

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Coalescing a None Option with an alternative value " +
            "returns the alternative value.")]
        public static void ValueGetValueOrDefault_None(NonNull<string> coalescey)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(coalescey.Get);

            // assert
            Assert.Equal(coalescey.Get, actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with an alternative value " +
            "returns the Some value.")]
        public static void ValueGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.GetValueOrDefault(coalescey.Get);

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Coalescing a None Option with a func producing an alternative value " +
            "returns the alternative value.")]
        public static void FuncGetValueOrDefault_None(NonNull<string> coalescey)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(() => coalescey.Get);

            // assert
            Assert.Equal(coalescey.Get, actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with a func producing an alternative value " +
            "returns the Some value.")]
        public static void FuncGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.GetValueOrDefault(() => coalescey.Get);

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Property(DisplayName = "Coalescing a None Option with a task producing an alternative value " +
            "returns the alternative value.")]
        public static void TaskGetValueOrDefault_None(NonNull<string> coalescey)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetValueOrDefault(() => FromResult(coalescey.Get)).Result;

            // assert
            Assert.Equal(coalescey.Get, actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with a task producing an alternative value " +
            "returns the Some value.")]
        public static void TaskGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.GetValueOrDefault(() => FromResult(coalescey.Get)).Result;

            // assert
            Assert.Equal(some.Get, actual);
        }

        #endregion

        #region Overrides

        [Fact(DisplayName = "A None Option stringifies to None.")]
        public static void ToString_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal("None", actual);
        }

        [Property(DisplayName = "A Some Option stringifies to a wrapped value.")]
        public static void ToString_Some(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.ToString();

            // assert
            Assert.Equal($"Some({some})", actual);
        }

        [Fact(DisplayName = "A None Option is not equal to null.")]
        public static void ObjectEquals_NoneNull()
        {
            // arrange
            var left = Option<string>.None;
            var right = default(object);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "A Some Option is not equal to null.")]
        public static void ObjectEquals_SomeNull(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = default(object);

            // act
            var actual = left.Equals(right);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Two None Options of the same type are equal.")]
        public static void ObjectEquals_NoneNone_SameType()
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
        public static void ObjectEquals_NoneNone_DifferentType()
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

        [Property(DisplayName = "A None Option and a Some Option of the same type are not equal.")]
        public static void ObjectEquals_NoneSome_SameType(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "A None Option and a Some Option of different types are not equal.")]
        public static void ObjectEquals_NoneSome_DifferentType(NonNull<string> some)
        {
            // arrange
            var left = Option<int>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options of the same type with different values are not equal.")]
        public static void ObjectEquals_SomeSome_SameType_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options of the same type with the same values are equal.")]
        public static void ObjectEquals_SomeSome_SameType_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options of different types are not equal.")]
        public static void ObjectEquals_SomeSome_DifferentType(NonNull<string> someLeft, int someRight)
        {
            // arrange
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight);

            // act
            var actualLeftFirst = left.Equals(right);
            var actualRightFirst = right.Equals(left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "A None Option has a hashcode of 0.")]
        public static void GetHashCode_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(0, actual);
        }

        [Property(DisplayName = "A Some Option has the hashcode of its Some value.")]
        public static void GetHashCode_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.GetHashCode();

            // assert
            Assert.Equal(some.Get.GetHashCode(), actual);
        }

        #endregion

        #region Implementations

        [Property(DisplayName = "A None Option does not iterate.")]
        public static void GetEnumerator_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            foreach (var v in value)
            {
                actual = sentinel.Get;
            }

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "A Some Option iterates.")]
        public static void GetEnumerator_Some(NonNull<string> some, NonNull<string> before)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            foreach (var v in value)
            {
                actual = v;
            }

            // assert
            Assert.Equal(some.Get, actual);
        }

        #endregion

        #region Operators and Named Alternates

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void OperatorEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left == right;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void OperatorEquals_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        public static void OperatorEquals_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        public static void OperatorEquals_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left == right;
            var actualRightFirst = right == left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are not unequal.")]
        public static void OperatorNotEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = left != right;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "A None Option and a Some Option are unequal.")]
        public static void OperatorNotEquals_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are unequal.")]
        public static void OperatorNotEquals_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are not unequal.")]
        public static void OperatorNotEquals_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left != right;
            var actualRightFirst = right != left;

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public static void OperatorBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public static void NamedBitwiseOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The disjunction of two None Options is a None Option.")]
        public static void OperatorLogicalOr_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public static void OperatorBitwiseOr_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public static void NamedBitwiseOr_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Property(DisplayName = "The disjunction of a None Option and a Some Option is the Some Option.")]
        public static void OperatorLogicalOr_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Property(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        public static void OperatorBitwiseOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left | right;
            var actualRightFirst = right | left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Property(DisplayName = "The disjunction of two Some Options is the latter Some Option.")]
        public static void NamedBitwiseOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left.BitwiseOr(right);
            var actualRightFirst = right.BitwiseOr(left);

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Property(DisplayName = "The disjunction of two Some Options is the former Some Option.")]
        public static void OperatorLogicalOr_SomeSome(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = left || right;
            var actualRightFirst = right || left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(right, actualRightFirst);
        }

        [Fact(DisplayName = "A None Option does not evaluate as true.")]
        public static void OperatorIsTrue_None()
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
        public static void NamedIsTrue_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsTrue;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "A Some Option evaluates as true.")]
        public static void NamedIsTrue_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.IsTrue;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A None Option evaluates as false.")]
        public static void OperatorIsTrue_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);
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
        public static void NamedIsFalse_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.IsFalse;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A Some Option does not evaluate as true.")]
        public static void NamedIsFalse_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.IsFalse;

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        public static void NamedLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "The logical inverse of a None Option is true.")]
        public static void OperatorLogicalNot_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = !value;

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "The logical inverse of a Some Option is false.")]

        public static void NamedLogicalNot_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.LogicalNot();

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "The logical inverse of a Some Option is false.")]
        public static void OperatorNot_None(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = !value;

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "The disjunction of a Some Option and a None Option short-circuits.")]
        public static void OperatorLogicalOr_SomeNone_ShortCircuits(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(some.Get);
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option<string>.None;
            }

            // act
            var dummy = left || Right();

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "The disjunction of two Some Options short-circuits.")]
        public static void OperatorLogicalOr_SomeSome_ShortCircuits(
            NonNull<string> some,
            NonNull<string> before,
            NonNull<string> sentinel)
        { // note(cosborn) This tests "operator true"/IsTrue
            // arrange
            var left = Option.From(some.Get);
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option.From(some.Get);
            }

            // act
            var dummy = left || Right();

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public static void OperatorBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public static void NamedBitwiseAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Fact(DisplayName = "The conjunction of two None Options is a None Option.")]
        public static void OperatorLogicalAnd_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.True(actualLeftFirst.IsNone);
            Assert.True(actualRightFirst.IsNone);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public static void OperatorBitwiseAnd_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public static void NamedBitwiseAnd_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option is a None Option.")]
        public static void OperatorLogicalAnd_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.Equal(left, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public static void OperatorBitwiseAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            // arrange
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            // act
            var actualLeftFirst = left & right;
            var actualRightFirst = right & left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public static void NamedBitwiseAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            // arrange
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            // act
            var actualLeftFirst = left.BitwiseAnd(right);
            var actualRightFirst = right.BitwiseAnd(left);

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of two Some Options is the latter Some Option.")]
        public static void OperatorLogicalAnd_SomeSome(NonNull<string> someLeft, NonNull<string> someRight)
        {
            // arrange
            var left = Option.From(someLeft.Get);
            var right = Option.From(someRight.Get);

            // act
            var actualLeftFirst = left && right;
            var actualRightFirst = right && left;

            // assert
            Assert.Equal(right, actualLeftFirst);
            Assert.Equal(left, actualRightFirst);
        }

        [Property(DisplayName = "The conjunction of two None Options short-circuits.")]
        public static void OperatorLogicalAnd_NoneNone_ShortCircuits(NonNull<string> before, NonNull<string> sentinel)
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option<string>.None;
            }

            // act
            var dummy = left && Right();

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "The conjunction of a None Option and a Some Option short-circuits.")]
        public static void OperatorLogicalAnd_NoneSome_ShortCircuits(NonNull<string> before, NonNull<string> sentinel)
        { // note(cosborn) This tests "operator false"/IsFalse
            // arrange
            var left = Option<string>.None;
            var actual = before.Get;
            Option<string> Right()
            {
                actual = sentinel.Get;
                return Option.From(sentinel.Get);
            }

            // act
            var dummy = left && Right();

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Fact(DisplayName = "The untyped None converts to a None Option.")]
        public static void LiteralNone_IsNone()
        {
            // arrange, act
            Option<string> actual = Option.None;

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Fact(DisplayName = "Null converts to a None Option.")]
        public static void Null_IsNone()
        {
            // arrange, act
            Option<string> actual = null;

            // assert
            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Property(DisplayName = "Values convert to Some Options.")]
        public static void Value_IsSome(int some)
        {
            // arrange, act
            Option<int> actual = some;

            // assert
            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Fact(DisplayName = "Unwrapping a None Option throws.")]
        public static void Cast_None_Throws()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = Record.Exception(() => (string)value);

            // assert
            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);

        }

        [Property(DisplayName = "Unwrapping a Some Option returns its Some value.")]
        public static void Cast_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = (string)value;

            // assert
            Assert.Equal(some.Get, actual);
        }

        #endregion

        #region Split

        [Property(DisplayName = "Splitting a null value over a func returns a None Option.")]
        public static void FuncSplit_Null_None(bool split)
        {
            // arrange
            const string value = null;

            // act
            var actual = Option.Split(value, _ => split);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a null nullable value over a func returns a None Option.")]
        public static void FuncSplit_NullableNull_None(bool split)
        {
            // arrange
            var value = (int?)null;

            // act
            var actual = Option.Split(value, (int _) => split);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, failing the condition, returns a None Option.")]
        public static void FuncSplit_ReturnFalse_None(NonNull<string> some)
        {
            // arrange
            var value = some.Get;

            // act
            var actual = Option.Split(value, _ => false);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, failing the condition, " +
            "returns a None Option.")]
        public static void FuncSplit_NullableReturnFalse_None(int some)
        {
            // arrange
            var value = (int?)some;

            // act
            var actual = Option.Split(value, (int _) => false);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, returns a Some Option.")]
        public static void FuncSplit_Some(NonNull<string> some)
        {
            // arrange
            var value = some.Get;

            // act
            var actual = Option.Split(value, _ => true);

            // assert
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, " +
            "returns a Some Option.")]
        public static void FuncSplit_Nullable_Some(int some)
        {
            // arrange
            var value = (int?)some;

            // act
            var actual = Option.Split(value, (int _) => true);

            // assert
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Splitting a null value over a task returns a None Option.")]
        public static void TaskSplit_Null_None(bool split)
        {
            // arrange
            const string value = null;

            // act
            var actual = Option.Split(value, _ => FromResult(split)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable null value over a task returns a None Option.")]
        public static void TaskSplit_NullableNull_None(bool split)
        {
            // arrange
            var value = (int?)null;

            // act
            var actual = Option.Split(value, (int _) => FromResult(split)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a task, failing the condition, " +
            "returns a None Option.")]
        public static void TaskSplit_ReturnFalse_None(NonNull<string> some)
        {
            // arrange
            var value = some;

            // act
            var actual = Option.Split(value, _ => FromResult(false)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a nullable value over a task, failing the condition, " +
            "returns a None Option.")]
        public static void TaskSplit_NullableReturnFalse_None(int some)
        {
            // arrange
            var value = (int?)some;

            // act
            var actual = Option.Split(value, (int _) => FromResult(false)).Result;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Splitting a value over a func, passing the condition, " +
            "returns a Some Option.")]
        public static void TaskSplit_Some(NonNull<string> some)
        {
            // arrange
            var value = some;

            // act
            var actual = Option.Split(value, _ => FromResult(true)).Result;

            // assert
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Splitting a nullable value over a func, passing the condition, " +
            "returns a Some Option.")]
        public static void TaskSplit_Nullable_Some(int some)
        {
            // arrange
            var value = (int?)some;

            // act
            var actual = Option.Split(value, (int _) => FromResult(true)).Result;

            // assert
            Assert.True(actual.IsSome);
        }

        #endregion

        #region Join

        [Fact(DisplayName = "Joining a None Option Option returns a None Option.")]
        public static void Join_None()
        {
            // arrange
            var value = Option<Option<int>>.None;

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsNone);
        }

        [Fact(DisplayName = "Joining a Some Option containing a None Option returns a None Option.")]
        public static void Join_SomeNone()
        {
            // arrange
            var value = Option.From(Option<int>.None);

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Joining a Some Option containing a Some Option returns a Some Option.")]
        public static void Join_SomeSome(int some)
        {
            // arrange
            var value = Option.From(Option.From(some));

            // act
            var actual = value.Join();

            // assert
            Assert.True(actual.IsSome);
            Assert.Equal(some, actual.Value);
        }

        #endregion

        #region Extensions

        [Fact(DisplayName = "A None Option converted to a Nullable is null.")]
        public static void ToNullable_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.ToNullable();

            // assert
            Assert.Null(actual);
        }

        [Property(DisplayName = "A Some Option converted to a Nullable is equal to the Some value.")]
        public static void ToNullable_Some(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.ToNullable();

            // assert
            Assert.NotNull(actual);
            Assert.Equal(some, actual);
        }

        #region LINQ

        [Fact(DisplayName = "Asking a None Option for any returns false.")]
        public static void Any_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any();

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some Option for any returns true.")]
        public static void Any_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Any();

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for any with a false predicate returns false.")]
        public static void PredicateAny_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Fact(DisplayName = "Asking a None Option for any with a true predicate returns false.")]
        public static void PredicateAny_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some Option for any with a false predicate returns false.")]
        public static void PredicateAny_SomeFalse(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Any(_ => false);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some Option for any with a true predicate returns true.")]
        public static void PredicateAny_SomeTrue(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Any(_ => true);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for all with a false predicate returns true.")]
        public static void PredicateAll_NoneFalse()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(_ => false);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Asking a None Option for all with a true predicate returns true.")]
        public static void PredicateAll_NoneTrue()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.All(_ => true);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Asking a Some Option for all with a false predicate returns false.")]
        public static void PredicateAll_SomeFalse(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.All(_ => false);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some Option for all with a true predicate returns true.")]
        public static void PredicateAll_SomeTrue(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.All(_ => true);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Asking a None option whether it contains a value returns false.")]
        public static void Contains_None(int contains)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(contains);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some option whether it contains a value that it doesn't returns false.")]
        public static void Contains_Some_False(UnequalNonNullPair<string> pair)
        {
            // arrange
            var value = Option.From(pair.Left);

            // act
            var actual = value.Contains(pair.Right);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some option whether it contains a value that it does" +
            "returns true.")]
        public static void Contains_Some_True(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Contains(some.Get);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "Asking a None option whether it contains a value returns false.")]
        public static void ComparerContains_None(int contains)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.Contains(contains, EqualityComparer<int>.Default);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some option whether it contains a value that it doesn't returns false.")]
        public static void ComparerContains_Some_False(UnequalNonNullPair<string> pair)
        {
            // arrange
            var value = Option.From(pair.Left);

            // act
            var actual = value.Contains(pair.Right, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actual);
        }

        [Property(DisplayName = "Asking a Some option whether it contains a value that it does" +
            "returns true.")]
        public static void ComparerContains_Some_True(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Contains(some.Get.ToUpperInvariant(), StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actual);
        }

        [Fact(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void DefaultIfEmpty_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(0, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        public static void DefaultIfEmpty_Some(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = value.DefaultIfEmpty();

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Property(DisplayName = "Recovering a None Option returns the recovery value.")]
        public static void ValueDefaultIfEmpty_None(NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.DefaultIfEmpty(sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(sentinel.Get, innerValue);
        }

        [Property(DisplayName = "Recovering a Some Option returns the Some value.")]
        public static void ValueDefaultIfEmpty_Some(NonNull<string> some, NonNull<string> sentinel)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.DefaultIfEmpty(sentinel.Get);

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some.Get, innerValue);
        }

        [Property(DisplayName = "Tapping a None Option over a func returns a None Option " +
            "and performs no action.")]
        public static void Do_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var output = before.Get;
            var actual = value.Do(v => output = sentinel.Get);

            // assert
            Assert.True(actual.IsNone);
            Assert.Equal(before.Get, output);
        }

        [Property(DisplayName = "Tapping a Some Option over a func returns a Some Option " +
            "and performs an action.")]
        public static void Do_Some(NonNull<string> some, NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var output = before.Get;
            var actual = value.Do(v => output = sentinel.Get);

            // assert
            Assert.Equal(value, actual);
            Assert.Equal(sentinel.Get, output);
        }

        [Property(DisplayName = "Conditionally executing an action based on a None Option " +
            "does not execute.")]
        public static void ForEach_None(NonNull<string> before, NonNull<string> sentinel)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = before.Get;
            value.ForEach(v => actual = sentinel.Get);

            // assert
            Assert.Equal(before.Get, actual);
        }

        [Property(DisplayName = "Conditionally executing an action based on a Some Option executes.")]
        public static void ForEach_Some(NonNull<string> some, NonNull<string> before)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = before.Get;
            value.ForEach(v => actual = v);

            // assert
            Assert.Equal(some.Get, actual);
        }

        [Fact(DisplayName = "Selecting a None Option produces a None Option.")]
        public static void Select_None()
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting a Some Option produces a Some Option.")]
        public static void Select_Some(short some)
        {
            // arrange
            var value = Option.From((int)some);

            // act
            var actual = from v in value
                         select v + 1;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some + 1, innerValue);
        }

        [Property(DisplayName = "Filtering a None Option produces a None Option.")]
        public static void Where_None(bool filter)
        {
            // arrange
            var value = Option<int>.None;

            // act
            var actual = from v in value
                         where filter
                         select v;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a false predicate produces a None Option.")]
        public static void Where_SomeFalse(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = from v in value
                         where false
                         select v;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Filtering a Some Option with a true predicate produces a Some Option.")]
        public static void Where_SomeTrue(int some)
        {
            // arrange
            var value = Option.From(some);

            // act
            var actual = from v in value
                         where true
                         select v;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(some, innerValue);
        }

        [Fact(DisplayName = "Selecting from two None Options produces a None Option.")]
        public static void SelectManyResult_NoneNone()
        {
            // arrange
            var left = Option<int>.None;
            var right = Option<int>.None;

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a Some Option and a None Option produces a None Option.")]
        public static void SelectManyResult_SomeNone(int someLeft)
        {
            // arrange
            var left = Option.From(someLeft);
            var right = Option<int>.None;

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from a None Option and a Some Option produces a None Option.")]
        public static void SelectManyResult_NoneSome(int someRight)
        {
            // arrange
            var left = Option<int>.None;
            var right = Option.From(someRight);

            // act
            var actual = from l in left
                         from r in right
                         select l + r;

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Selecting from two Some Options produces a Some Option.")]
        public static void SelectManyResult_SomeSome(UnequalPair<int> values)
        {
            // arrange
            var left = Option.From(values.Left);
            var right = Option.From(values.Right);

            // act
            var actual = from l in left
                         from r in right
                         select (long)l + r;

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(values.Left + values.Right, innerValue);
        }

        [Fact(DisplayName = "Binding a None Option over a func returning a None Option returns a None Option.")]
        public static void SelectMany_None()
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.SelectMany(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a None Option over a func returning a Some Option returns a None Option.")]
        public static void SelectMany_None(int some)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.SelectMany(_ => Option.From(some));

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a None Option " +
            "returns a None Option.")]
        public static void SelectMany_ReturnNone_Some(NonNull<string> some)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.SelectMany(_ => Option<int>.None);

            // assert
            Assert.True(actual.IsNone);
        }

        [Property(DisplayName = "Binding a Some Option over a func returning a Some Option " +
            "returns a Some Option.")]
        public static void SelectManyReturnSome_Some(NonNull<string> someString, int someInt)
        {
            // arrange
            var value = Option.From(someString.Get);

            // act
            var actual = value.SelectMany(_ => Option.From(someInt));

            // assert
            Assert.True(actual.IsSome);
            var innerValue = actual.Value;
            Assert.Equal(someInt, innerValue);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void Aggregate_None(short seed)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Aggregate((int)seed, (s, v) => s + v.Length);

            // assert
            Assert.Equal(seed, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the " +
            "accumulator over the seed value and the Some value.")]
        public static void Aggregate_Some(NonNull<string> some, short seed)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Aggregate((int)seed, (s, v) => s + v.Length);

            // assert
            var expected = some.Get.Length + seed;
            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Folding over a None Option returns the seed value.")]
        public static void ResultAggregate_None(short seed)
        {
            // arrange
            var value = Option<string>.None;

            // act
            var actual = value.Aggregate((int)seed, (s, v) => s + v.Length, v => v * 2);

            // assert
            var expected = seed * 2;
            Assert.Equal(expected, actual);
        }

        [Property(DisplayName = "Folding over a Some Option returns the result of invoking the " +
            "accumulator over the seed value and the Some value.")]
        public static void ResultAggregate_Some(NonNull<string> some, short seed)
        {
            // arrange
            var value = Option.From(some.Get);

            // act
            var actual = value.Aggregate((int)seed, (s, v) => s + v.Length, v => v * 2);

            // assert
            var expected = (seed + some.Get.Length) * 2;
            Assert.Equal(expected, actual);
        }

        #endregion

        #endregion

        #region Get Underlying Type

        [Theory(DisplayName = "The underlying type of an Option is accessible.")]
        [InlineData(typeof(Option<int>), typeof(int))]
        [InlineData(typeof(Option<string>), typeof(string))]
        [InlineData(typeof(int), null)]
        [InlineData(typeof(string), null)]
        [InlineData(typeof(List<int>), null)]
        [InlineData(typeof(Option<>), null)]
        [InlineData(typeof(List<>), null)]
        public static void GetUnderlyingType(Type optionalType, Type expected)
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
        public static void StaticEquals_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = Option.OptionalEquals(left, right);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticEquals_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        public static void StaticEquals_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        public static void StaticEquals_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right);
            var actualRightFirst = Option.OptionalEquals(right, left);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticEqualsComparer_NoneNone()
        {
            // arrange
            var left = Option<string>.None;
            var right = Option<string>.None;

            // act
            var actual = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actual);
        }

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticEqualsComparer_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        public static void StaticEqualsComparer_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.False(actualLeftFirst);
            Assert.False(actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        public static void StaticEqualsComparer_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get.ToUpperInvariant());

            // act
            var actualLeftFirst = Option.OptionalEquals(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalEquals(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.True(actualLeftFirst);
            Assert.True(actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticCompare_NoneNone()
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

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticCompare_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        public static void StaticCompare_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.NotEqual(0, actualLeftFirst);
            Assert.NotEqual(0, actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        public static void StaticCompare_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right);
            var actualRightFirst = Option.OptionalCompare(right, left);

            // assert
            Assert.Equal(0, actualLeftFirst);
            Assert.Equal(0, actualRightFirst);
        }

        [Fact(DisplayName = "Two None Options are equal.")]
        public static void StaticCompareComparer_NoneNone()
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

        [Property(DisplayName = "A None Option and a Some Option are not equal.")]
        public static void StaticCompareComparer_NoneSome(NonNull<string> some)
        {
            // arrange
            var left = Option<string>.None;
            var right = Option.From(some.Get);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.Equal(-1, actualLeftFirst);
            Assert.Equal(1, actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with different values are not equal.")]
        public static void StaticCompareComparer_SomeSome_DifferentValue(UnequalNonNullPair<string> pair)
        {
            // arrange
            var left = Option.From(pair.Left);
            var right = Option.From(pair.Right);

            // act
            var actualLeftFirst = Option.OptionalCompare(left, right, StringComparer.OrdinalIgnoreCase);
            var actualRightFirst = Option.OptionalCompare(right, left, StringComparer.OrdinalIgnoreCase);

            // assert
            Assert.NotEqual(0, actualLeftFirst);
            Assert.NotEqual(0, actualRightFirst);
        }

        [Property(DisplayName = "Two Some Options with the same values are equal.")]
        public static void StaticCompareComparer_SomeSome_SameValue(NonNull<string> some)
        {
            // arrange
            var left = Option.From(some.Get);
            var right = Option.From(some.Get.ToUpperInvariant());

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
