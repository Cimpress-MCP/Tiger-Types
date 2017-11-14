using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Threading.Tasks.Task;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Option{TSome}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) })]
    public static partial class OptionTests
    {
        [Property(DisplayName = "Non-null values create Some Options using the untyped static From method.")]
        static void UntypedFrom_Value_IsSome(NonNull<string> some)
        {
            var actual = Option.From(some.Get);

            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null values create None Options using the untyped static From method.")]
        static void UntypedFrom_Null_IsNone()
        {
            var actual = Option.From((string)null);

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Property(DisplayName = "Non-null nullable values create Some Options.")]
        static void UntypedFrom_NullableValue_IsSome(int some)
        {
            var actual = Option.From((int?)some);

            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null nullable values create None Options.")]
        static void UntypedFrom_NullableNull_IsNone()
        {
            var actual = Option.From((int?)null);

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Fact(DisplayName = "Forcibly unwrapping a None Option throws.")]
        static void Value_None_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Value);

            var ex = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(Resources.OptionIsNone, ex.Message);
        }

        [Property(DisplayName = "Forcibly unwrapping a Some Option returns the Some value.")]
        static void Value_Some(NonNull<string> some) => Assert.Equal(some.Get, Option.From(some.Get).Value);

        [Fact(DisplayName = "Coalescing a None Option with an alternative value returns the alternative value.")]
        static void GetValueOrDefault_None() => Assert.Equal(default, Option<string>.None.GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Some Option with an alternative value returns the Some value.")]
        static void GetValueOrDefault_Some(NonNull<string> some) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault());

        [Property(DisplayName = "Coalescing a None Option with an alternative value returns the alternative value.")]
        static void ValueGetValueOrDefault_None(NonNull<string> coalescey) =>
            Assert.Equal(coalescey.Get, Option<string>.None.GetValueOrDefault(coalescey.Get));

        [Property(DisplayName = "Coalescing a Some Option with an alternative value returns the Some value.")]
        static void ValueGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault(coalescey.Get));

        [Property(DisplayName = "Coalescing a None Option with a func producing an alternative value returns the alternative value.")]
        static void FuncGetValueOrDefault_None(NonNull<string> coalescey) =>
            Assert.Equal(coalescey.Get, Option<string>.None.GetValueOrDefault(() => coalescey.Get));

        [Property(DisplayName = "Coalescing a Some Option with a func producing an alternative value returns the Some value.")]
        static void FuncGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault(() => coalescey.Get));

        [Property(DisplayName = "Coalescing a None Option with a task producing an alternative value returns the alternative value.")]
        static async Task TaskGetValueOrDefault_None(NonNull<string> coalescey)
        {
            var actual = await Option<string>.None
                .GetValueOrDefault(() => FromResult(coalescey.Get))
                .ConfigureAwait(false);

            Assert.Equal(coalescey.Get, actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with a task producing an alternative value returns the Some value.")]
        static async Task TaskGetValueOrDefault_Some(NonNull<string> some, NonNull<string> coalescey)
        {
            var actual = await Option.From(some.Get)
                .GetValueOrDefault(() => FromResult(coalescey.Get))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Theory(DisplayName = "The underlying type of an Option is accessible.")]
        [InlineData(typeof(Option<int>), typeof(int))]
        [InlineData(typeof(Option<string>), typeof(string))]
        [InlineData(typeof(int), null)]
        [InlineData(typeof(string), null)]
        [InlineData(typeof(List<int>), null)]
        [InlineData(typeof(Option<>), null)]
        [InlineData(typeof(List<>), null)]
        static void GetUnderlyingType(Type optionalType, Type expected) =>
            Assert.Equal(expected, Option.GetUnderlyingType(optionalType));
    }
}
