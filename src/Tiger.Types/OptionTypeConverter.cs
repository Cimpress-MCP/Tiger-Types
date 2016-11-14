﻿using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Provides a way of converting to and from the <see cref="Option{TSome}"/> space.</summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Using nameof() makes reflection safer.")]
    public sealed class OptionTypeConverter
        : TypeConverter
    {
        readonly Type _conversionType;
        readonly Type _underlyingType;
        readonly TypeConverter _underlyingTypeConverter;

        /// <summary>Initializes a new instance of the <see cref="OptionTypeConverter"/> class.</summary>
        /// <param name="type">The type from which to convert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is not compatible.</exception>
        public OptionTypeConverter([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (!type.IsConstructedGenericType ||
                type.GetGenericTypeDefinition() != typeof(Option<>))
            {
                throw new ArgumentOutOfRangeException(nameof(type), Resources.IncompatibleType);
            }

            _conversionType = type; // note(cosborn) This is Option<TSome>.
            _underlyingType = Option.GetUnderlyingType(_conversionType); // note(cosborn) This is TSome itself.
            Assume(_underlyingType != null); // ReSharper disable once AssignNullToNotNullAttribute
            _underlyingTypeConverter = TypeDescriptor.GetConverter(_underlyingType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == _underlyingType || // note(cosborn) TSome can become Option<TSome>.
            Nullable.GetUnderlyingType(sourceType) == _underlyingType || // note(cosborn) 1–1 mapping wtih TSome?.
            _underlyingTypeConverter.CanConvertFrom(context, sourceType);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) { return Activator.CreateInstance(_conversionType); }

            if (Option.From(value as string).Any(s => s.Length == 0))
            { // note(cosborn) For TypeConverter purposes, an empty string is "no value."
                return Activator.CreateInstance(_conversionType);
            }

            var typeInfo = _conversionType.GetTypeInfo();
            var ctor = typeInfo.DeclaredConstructors.Single(c => c.GetParameters().Length == 1);

            if (value.GetType() == _underlyingType)
            { // note(cosborn) Since there's no other conversion to be done, wrap it up!
                return ctor.Invoke(new[] { value });
            }

            try
            { // note(cosborn) Wrap up value returned by the converter for the Some type.
                var convertedValue = _underlyingTypeConverter.ConvertFrom(context, culture, value);
                return ctor.Invoke(new[] { convertedValue });
            }
            catch (NotSupportedException)
            { // note(cosborn) Read the source of GetConvertFromException; the return type is fake.
                throw (NotSupportedException)GetConvertFromException(value);
            }
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == _underlyingType || // note(cosborn) Option<TSome> can (usually) become TSome.
            destinationType == _conversionType || // note(cosborn) Natch.
            Nullable.GetUnderlyingType(destinationType) == _underlyingType || // note(cosborn) 1–1 mapping wtih TSome?.
            _underlyingTypeConverter.CanConvertTo(context, destinationType);

        /// <inheritdoc/>
        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            [CanBeNull] object value,
            Type destinationType)
        { // todo(cosborn) There is a strong possibility that this can be more efficient.
            if (value == null)
            {
                return ConvertToNull(context, culture, destinationType);
            }

            var valueType = value.GetType();
            if (valueType != _conversionType)
            { // note(cosborn) We can't convert what we don't (claim to) know how to convert.
                return base.ConvertTo(context, culture, valueType, destinationType);
            }

            if (valueType == destinationType)
            { // note(cosborn) This is easy.
                return value;
            }

            var typeInfo = _conversionType.GetTypeInfo();

            // note(cosborn) See if the Option<TSome> is in None state.
            var isNone = (bool)typeInfo
                .GetDeclaredProperty(nameof(Option<object>.IsNone))
                .GetValue(value);
            if (isNone)
            { // note(cosborn) It was None!
                return ConvertToNull(context, culture, destinationType);
            }

            // note(cosborn) Now that we know that it's in the Some state...
            var underlyingValue = typeInfo
                .GetDeclaredProperty(nameof(Option<object>.Value))
                .GetValue(value);

            if (destinationType == _underlyingType)
            { // note(cosborn) This is also easy.
                return underlyingValue;
            }

            try
            {
                return _underlyingTypeConverter.ConvertTo(context, culture, underlyingValue, destinationType);
            }
            catch (NotSupportedException)
            { // note(cosborn) Read the source of GetConvertToException; the return type is fake.
                throw (NotSupportedException)GetConvertToException(valueType, destinationType);
            }
        }

        [CanBeNull]
        object ConvertToNull(
            [CanBeNull] ITypeDescriptorContext context,
            [CanBeNull] CultureInfo culture,
            [NotNull] Type destinationType)
        {
            if (destinationType == null) { throw new ArgumentNullException(nameof(destinationType)); }

            return Nullable.GetUnderlyingType(destinationType) == _underlyingType
                ? null
                : base.ConvertTo(context, culture, null, destinationType);
        }
    }
}
