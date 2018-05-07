using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.Reflection.BindingFlags;
using static System.StringComparison;
using static System.Threading.Tasks.Task;
using static Tiger.Types.Resources;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="Option{TSome}"/>.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static partial class OptionTests
    {
        [Property(DisplayName = "The copy constructor copies.")]
        public static void CopyConstructor(Option<int> expected) =>
            Assert.Equal(expected, new Option<int>(expected));

        [Property(DisplayName = "Non-null values create Some Options using the untyped static From method.")]
        public static void UntypedFrom_Value_IsSome(NonEmptyString some)
        {
            var actual = Option.From(some.Get);

            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null values create None Options using the untyped static From method.")]
        public static void UntypedFrom_Null_IsNone()
        {
            var actual = Option.From((string)null);

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Property(DisplayName = "Non-null nullable values create Some Options.")]
        public static void UntypedFrom_NullableValue_IsSome(int some)
        {
            var actual = Option.From((int?)some);

            Assert.False(actual.IsNone);
            Assert.True(actual.IsSome);
        }

        [Property(DisplayName = "Null nullable values create None Options.")]
        public static void UntypedFrom_NullableNull_IsNone()
        {
            var actual = Option.From((int?)null);

            Assert.True(actual.IsNone);
            Assert.False(actual.IsSome);
        }

        [Fact(DisplayName = "Forcibly unwrapping a None Option throws.")]
        public static void Value_None_Throws()
        {
            var actual = Record.Exception(() => Option<string>.None.Value);

            var ioe = Assert.IsType<InvalidOperationException>(actual);
            Assert.Contains(OptionIsNone, ioe.Message, Ordinal);
        }

        [Property(DisplayName = "Forcibly unwrapping a Some Option returns the Some value.")]
        public static void Value_Some(NonEmptyString some) => Assert.Equal(some.Get, Option.From(some.Get).Value);

        [Fact(DisplayName = "Coalescing a None Option with an alternative value returns the alternative value.")]
        public static void GetValueOrDefault_None() => Assert.Equal(default, Option<string>.None.GetValueOrDefault());

        [Property(DisplayName = "Coalescing a Some Option with an alternative value returns the Some value.")]
        public static void GetValueOrDefault_Some(NonEmptyString some) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault());

        [Property(DisplayName = "Coalescing an Option with a null value throws.")]
        public static void ValueGetOrDefault_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.GetValueOrDefault((string)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Option with an alternative value returns the alternative value.")]
        public static void ValueGetValueOrDefault_None(NonEmptyString coalescey) =>
            Assert.Equal(coalescey.Get, Option<string>.None.GetValueOrDefault(coalescey.Get));

        [Property(DisplayName = "Coalescing a Some Option with an alternative value returns the Some value.")]
        public static void ValueGetValueOrDefault_Some(NonEmptyString some, NonEmptyString coalescey) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault(coalescey.Get));

        [Property(DisplayName = "Coalescing an Option with a null func throws.")]
        public static void FuncGetOrDefault_Null_Throws(Option<string> option)
        {
            var actual = Record.Exception(() => option.GetValueOrDefault((Func<string>)null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Option with a func producing an alternative value returns the alternative value.")]
        public static void FuncGetValueOrDefault_None(NonEmptyString coalescey) =>
            Assert.Equal(coalescey.Get, Option<string>.None.GetValueOrDefault(() => coalescey.Get));

        [Property(DisplayName = "Coalescing a Some Option with a func producing an alternative value returns the Some value.")]
        public static void FuncGetValueOrDefault_Some(NonEmptyString some, NonEmptyString coalescey) =>
            Assert.Equal(some.Get, Option.From(some.Get).GetValueOrDefault(() => coalescey.Get));

        [Property(DisplayName = "Coalescing an Option with a null task throws.")]
        public static async Task TaskGetOrDefault_Null_Throws(Option<string> option)
        {
            var actual = await Record.ExceptionAsync(() => option.GetValueOrDefaultAsync(null)).ConfigureAwait(false);

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("other", ane.Message, Ordinal);
        }

        [Property(DisplayName = "Coalescing a None Option with a task producing an alternative value returns the alternative value.")]
        public static async Task TaskGetValueOrDefault_None(NonEmptyString coalescey)
        {
            var actual = await Option<string>.None
                .GetValueOrDefaultAsync(() => FromResult(coalescey.Get))
                .ConfigureAwait(false);

            Assert.Equal(coalescey.Get, actual);
        }

        [Property(DisplayName = "Coalescing a Some Option with a task producing an alternative value returns the Some value.")]
        public static async Task TaskGetValueOrDefault_Some(NonEmptyString some, NonEmptyString coalescey)
        {
            var actual = await Option.From(some.Get)
                .GetValueOrDefaultAsync(() => FromResult(coalescey.Get))
                .ConfigureAwait(false);

            Assert.Equal(some.Get, actual);
        }

        [Fact(DisplayName = "A None Option dumps to a None Option.")]
        public static void Dump_None()
        {
            var option = Option<int>.None;
            var actual = typeof(Option<int>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(option, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            var property = Assert.Single(properties);
            Assert.NotNull(property);
            Assert.Equal("State", property.Name);
            var value = Assert.IsType<string>(property.GetValue(actual));
            Assert.Equal("None", value);
        }

        [Property(DisplayName = "A Some Option dumps to a Some Option.")]
        public static void Dump_Some(int some)
        {
            var option = Option.From(some);
            var actual = typeof(Option<int>)
                .GetMethod("ToDump", Instance | NonPublic)
                .Invoke(option, Array.Empty<object>());
            var properties = actual
                .GetType()
                .GetProperties();

            Assert.All(properties, Assert.NotNull);
            Assert.Collection(properties,
                pi =>
                {
                    Assert.Equal("State", pi.Name);
                    var value = Assert.IsType<string>(pi.GetValue(actual));
                    Assert.Equal("Some", value);
                },
                pi =>
                {
                    Assert.Equal("Value", pi.Name);
                    var value = Assert.IsType<int>(pi.GetValue(actual));
                    Assert.Equal(some, value);
                });
        }

        [Fact(DisplayName = "Asking for the underlying type of null throws.")]
        public static void GetUnderlyingType_Null_Throws()
        {
            var actual = Record.Exception(() => Option.GetUnderlyingType(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("optionalType", ane.Message, Ordinal);
        }

        [Theory(DisplayName = "The underlying type of an Option is accessible.")]
        [InlineData(typeof(Option<int>), typeof(int))]
        [InlineData(typeof(Option<string>), typeof(string))]
        [InlineData(typeof(int), null)]
        [InlineData(typeof(string), null)]
        [InlineData(typeof(List<int>), null)]
        [InlineData(typeof(Option<>), null)]
        [InlineData(typeof(List<>), null)]
        public static void GetUnderlyingType(Type optionalType, Type expected) =>
            Assert.Equal(expected, Option.GetUnderlyingType(optionalType));
    }
}
