using System;
using System.Collections.Generic;
using Xunit;
using static System.ComponentModel.TypeDescriptor;
// ReSharper disable All

namespace Tiger.Types.UnitTest
{
    /// <summary>Tests related to <see cref="OptionTypeConverter"/>.</summary>
    public sealed class OptionTypeConverterTests
    {
        [Theory(DisplayName = "An option type converter advertises its from-conversions.")]
        [InlineData(typeof(Option<int>), typeof(int), true)]
        [InlineData(typeof(Option<string>), typeof(string), true)]
        [InlineData(typeof(Option<int>), typeof(string), true)]
        [InlineData(typeof(Option<int>), typeof(int?), true)]
        // note(cosborn) Yes: `TypeDescriptor.GetConverter(typeof(int)).CanConvertFrom(typeof(int)) == false`.
        [InlineData(typeof(Option<int>), typeof(Option<int>), false)]
        [InlineData(typeof(Option<int>), typeof(long), false)]
        [InlineData(typeof(Option<string>), typeof(int), false)]
        public void CanConvertFrom(Type converterType, Type conversionType, bool expected) =>
            Assert.Equal(expected, GetConverter(converterType).CanConvertFrom(conversionType));

        public static readonly TheoryData<Type, object, object> ConvertFromSource =
            new TheoryData<Type, object, object>
            {
                { typeof(Option<int>), 33, Option.From(33) },
                { typeof(Option<int>), "33", Option.From(33) },
                { typeof(Option<int>), (int?)33, Option.From(33) },
                { typeof(Option<int>), string.Empty, Option<int>.None },
                { typeof(Option<int>), null, Option<int>.None }
            };

        [Theory(DisplayName = "An option type converter converts from what it advertises.")]
        [MemberData(nameof(ConvertFromSource))]
        public void ConvertFrom(Type converterType, object value, object expected) =>
            Assert.Equal(expected, GetConverter(converterType).ConvertFrom(value));

        [Fact(DisplayName = "An option type converter does not convert from what it does not advertise.")]
        public void ConvertFrom_Fail()
        {
            var actual = Record.Exception(() => GetConverter(typeof(Option<int>)).ConvertFrom(33L));

            var ex = Assert.IsType<NotSupportedException>(actual);
            Assert.Contains(nameof(OptionTypeConverter), ex.Message);
        }

        [Theory(DisplayName = "An option type converter advertises its to-conversions.")]
        [InlineData(typeof(Option<int>), typeof(int), true)]
        [InlineData(typeof(Option<string>), typeof(string), true)]
        [InlineData(typeof(Option<int>), typeof(string), true)]
        [InlineData(typeof(Option<int>), typeof(int?), true)]
        [InlineData(typeof(Option<int>), typeof(Option<int>), true)]
        [InlineData(typeof(Option<int>), typeof(long), true)]
        [InlineData(typeof(Option<long>), typeof(int), true)]
        [InlineData(typeof(Option<int>), typeof(List<int>), false)]
        public void CanConvertTo(Type converterType, Type conversionType, bool expected) =>
            Assert.Equal(expected, GetConverter(converterType).CanConvertTo(conversionType));

        public static readonly TheoryData<Type, Type, object, object> ConvertToSource =
            new TheoryData<Type, Type, object, object>
            {
                { typeof(Option<int>), typeof(int), Option.From(33), 33 },
                { typeof(Option<string>), typeof(string), Option<string>.None, string.Empty },
                { typeof(Option<int>), typeof(int?), Option<int>.None, null },
                { typeof(Option<string>), typeof(string), Option.From("megatron"), "megatron" },
                { typeof(Option<string>), typeof(string), null, string.Empty },
                { typeof(Option<int>), typeof(Option<int>), Option.From(33), Option.From(33) }
            };

        [Theory(DisplayName = "An option type converter converts to what it advertises.")]
        [MemberData(nameof(ConvertToSource))]
        public void ConvertTo(Type converterType, Type conversionType, object value, object expected) =>
            Assert.Equal(expected, GetConverter(converterType).ConvertTo(value, conversionType));

        public static readonly TheoryData<Type, Type, object> ConvertTo_FailSource =
            new TheoryData<Type, Type, object>
            {
                { typeof(Option<int>), typeof(List<int>), Option.From(33) },
                { typeof(Option<int>), typeof(int), 33 },
                { typeof(Option<int>), typeof(Option<int>), 33 },
                { typeof(Option<int>), typeof(IEnumerable<int>), new List<int> { 33 } }
            };

        [Theory(DisplayName = "An option type converter does not convert to what it does not advertise.")]
        [MemberData(nameof(ConvertTo_FailSource))]
        public void ConvertTo_Fail(Type converterType, Type conversionType, object value)
        {
            var actual = Record.Exception(() => GetConverter(converterType).ConvertTo(value, conversionType));

            var ex = Assert.IsType<NotSupportedException>(actual);
            Assert.Contains(nameof(OptionTypeConverter), ex.Message);
        }
    }
}
