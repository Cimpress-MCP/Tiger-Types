using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FsCheck;
using FsCheck.Xunit;
using Tiger.Types.UnitTest.Utility;
using Xunit;
using static System.StringComparison;

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to the functionality of the <see cref="Error"/> struct.</summary>
    [Properties(Arbitrary = new[] { typeof(Generators) }, QuietOnSuccess = true)]
    public static class ErrorTests
    {
        public static TheoryData<Error> Defaults { get; } = new TheoryData<Error>
        {
            new Error(),
            default
        };

        [Theory(DisplayName = "A default Error doesn't lie.")]
        [MemberData(nameof(Defaults))]
        public static void Constructor_Empty(Error actual)
        {
            Assert.NotNull(actual.Message);
            Assert.Empty(actual.Context);
        }

        [Fact(DisplayName = "Constructing an Error with a null message throws.")]
        public static void Constructor_Contextless_NullMessage_Throws()
        {
            var actual = Record.Exception(() => new Error(null));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("message", ane.Message, Ordinal);
        }

        [Property(DisplayName = "An error constructed with only a message has no context.")]
        public static void Constuctor_Contextless(NonEmptyString message)
        {
            var actual = new Error(message.Get);

            Assert.Equal(message.Get, actual.Message);
            Assert.Empty(actual.Context);
        }

        [Fact(DisplayName = "Constructing an Error with a null message throws.")]
        public static void Constructor_ObjectContext_NullMessage_Throws()
        {
            var actual = Record.Exception(() => new Error(null, new {}));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("message", ane.Message, Ordinal);
        }

        [Property(DisplayName = "An error constructed with no context has no context.")]
        public static void Constructor_ObjectContext_Null(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, (object)null).Context);

        [Property(DisplayName = "An error constructed with an empty context has no context.")]
        public static void Constructor_ObjectContext_Empty(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, new { }).Context);

        [Property(DisplayName = "An error constructed with a context has that context.")]
        public static void Constructor_ObjectContext(NonEmptyString message, int contextValue)
        {
            var actual = new Error(message.Get, new
            {
                Key = contextValue
            });

            var (key, value) = Assert.Single(actual.Context);
            Assert.Equal("Key", key, StringComparer.Ordinal);
            var actualValue = Assert.IsType<int>(value);
            Assert.Equal(contextValue, actualValue);
        }

        [Fact(DisplayName = "Constructing an Error with a null message throws.")]
        public static void Constructor_StringObjectContext_NullMessage_Throws()
        {
            var actual = Record.Exception(() => new Error(null, ImmutableDictionary<string, object>.Empty));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("message", ane.Message, Ordinal);
        }

        [Property(DisplayName = "An error constructed with no context has no context.")]
        public static void Constructor_StringObjectContext_Null(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, (IReadOnlyDictionary<string, object>)null).Context);

        [Property(DisplayName = "An error constructed with an empty context has no context.")]
        public static void Constructor_StringObjectContext_Empty(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, ImmutableDictionary<string, object>.Empty).Context);

        [Property(DisplayName = "An error constructed with a context has that context.")]
        public static void Constructor_StringObjectContext(
            NonEmptyString message,
            NonEmptyString contextKey,
            int contextValue)
        {
            var actual = new Error(message.Get, new Dictionary<string, object>
            {
                [contextKey.Get] = contextValue
            });

            var (key, value) = Assert.Single(actual.Context);
            Assert.Equal(contextKey.Get, key, StringComparer.Ordinal);
            var actualValue = Assert.IsType<int>(value);
            Assert.Equal(contextValue, actualValue);
        }

        [Fact(DisplayName = "Constructing an Error with a null message throws.")]
        public static void Constructor_StringStringContext_NullMessage_Throws()
        {
            var actual = Record.Exception(() => new Error(null, ImmutableDictionary<string, string>.Empty));

            var ane = Assert.IsType<ArgumentNullException>(actual);
            Assert.Contains("message", ane.Message, Ordinal);
        }

        [Property(DisplayName = "An error constructed with no context has no context.")]
        public static void Constructor_StringStringContext_Null(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, (IReadOnlyDictionary<string, string>)null).Context);

        [Property(DisplayName = "An error constructed with an empty context has no context.")]
        public static void Constructor_StringStringContext_Empty(NonEmptyString message) =>
            Assert.Empty(new Error(message.Get, ImmutableDictionary<string, string>.Empty).Context);

        [Property(DisplayName = "An error constructed with a context has that context.")]
        public static void Constructor_StringStringContext(
            NonEmptyString message,
            NonEmptyString contextKey,
            NonEmptyString contextValue)
        {
            var actual = new Error(message.Get, new Dictionary<string, string>
            {
                [contextKey.Get] = contextValue.Get
            });

            var (key, value) = Assert.Single(actual.Context);
            Assert.Equal(contextKey.Get, key, StringComparer.Ordinal);
            var actualValue = Assert.IsType<string>(value);
            Assert.Equal(contextValue.Get, actualValue, StringComparer.Ordinal);
        }

        [Theory(DisplayName = "A default Error becomes an empty string.")]
        [MemberData(nameof(Defaults))]
        public static void Default_ToString(Error error) => Assert.Empty(error.ToString());

        [Property(DisplayName = "An Error converts to a string by returning its message.")]
        public static void Message_ToString(NonEmptyString message) =>
            Assert.Equal(message.Get, new Error(message.Get).ToString());

#pragma warning disable CS1718 // Comparison made to same variable
        [Theory(DisplayName = "A default error is equal to itself.")]
        [MemberData(nameof(Defaults))]
        public static void ReflexiveEquality_Default(Error error)
        {
            Assert.Equal(error, error);
            Assert.StrictEqual(error, error);
            Assert.True(error == error);
            Assert.True(error.Equals((object)error));
            Assert.True(error.Equals(error));
            Assert.False(error != error);
        }

        [Property(DisplayName = "An error is equal to itself.")]
        public static void ReflexiveEquality(NonEmptyString message)
        {
            var error = new Error(message.Get);

            Assert.Equal(error, error);
            Assert.StrictEqual(error, error);
            Assert.True(error == error);
            Assert.True(error.Equals((object)error));
            Assert.True(error.Equals(error));
            Assert.False(error != error);
        }
#pragma warning restore CS1718 // Comparison made to same variable

        [Property(DisplayName = "A non-default error is not equal to a default error.")]
        public static void Equality_NonDefault(NonEmptyString message)
        {
            var @default = new Error();
            var error = new Error(message.Get);

            Assert.NotEqual(@default, error);
            Assert.NotStrictEqual(@default, error);
            Assert.False(@default == error);
            Assert.False(@default.Equals((object)error));
            Assert.False(@default.Equals(error));
            Assert.True(@default != error);

            Assert.NotEqual(error, @default);
            Assert.NotStrictEqual(error, @default);
            Assert.False(error == @default);
            Assert.False(error.Equals((object)@default));
            Assert.False(error.Equals(@default));
            Assert.True(error != @default);
        }

        [Property(DisplayName = "Unequal Errors compare unequal.")]
        public static void Equality_DifferentMessage(UnequalNonNullPair<string> messages)
        {
            var left = new Error(messages.Left);
            var right = new Error(messages.Right);

            Assert.NotEqual(left, right);
            Assert.NotStrictEqual(left, right);
            Assert.False(left == right);
            Assert.False(left.Equals((object)right));
            Assert.False(left.Equals(right));
            Assert.True(left != right);
        }

        [Theory(DisplayName = "A default Error has a hashcode of 0.")]
        [MemberData(nameof(Defaults))]
        public static void GetHashCode_Default(Error actual) => Assert.Equal(0, actual.GetHashCode());
    }
}
