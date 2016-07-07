﻿using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Provides a way of converting to and from the <see cref="Option{TSome}"/> space.</summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Using nameof() makes reflection safer.")]
    sealed class OptionTypeConverter
        : TypeConverter
    {
        readonly Type _type;
        readonly Type _underlyingType;
        readonly TypeConverter _underlyingTypeConverter;

        /// <summary>Initializes an instance of <see cref="OptionTypeConverter"/>.</summary>
        /// <param name="type">The type from which to convert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is not compatible.</exception>
        public OptionTypeConverter([NotNull] Type type)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }

            if (!type.IsValueType ||
                !type.IsGenericType ||
                type.IsGenericTypeDefinition ||
                type.GetGenericTypeDefinition() != typeof(Option<>))
            {
                throw new ArgumentOutOfRangeException(nameof(type), Resources.IncompatibleType);
            }

            _type = type; // note(cosborn) This is Option<TSome>.
            _underlyingType = Option.GetUnderlyingType(_type); // note(cosborn) This is TSome itself.
            Assume(_underlyingType != null);
            _underlyingTypeConverter = TypeDescriptor.GetConverter(_underlyingType);
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type
        /// to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        /// <see langword="true"/> if this converter can perform the conversion; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == _underlyingType || // note(cosborn) TSome can become Option<TSome>.
            Nullable.GetUnderlyingType(sourceType) == _underlyingType || // note(cosborn) 1–1 mapping wtih TSome?.
            _underlyingTypeConverter.CanConvertFrom(context, sourceType);

        /// <summary>
        /// Converts the given object to the type of this converter,
        /// using the specified context and culture information.
        /// </summary>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) { return Activator.CreateInstance(_type); }

            if (Option.From(value as string).Any(s => s.Length == 0))
            { // note(cosborn) For TypeConverter purposes, an empty string is "no value."
                return Activator.CreateInstance(_type);
            }

            if (value.GetType() == _underlyingType)
            { // note(cosborn) Since there's no other conversion to be done, wrap it up!
                return Activator.CreateInstance(_type,
                    BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { value }, null);
            }

            try
            { // note(cosborn) Wrap up value returned by the converter for the Some type.
                var convertedValue = _underlyingTypeConverter.ConvertFrom(context, culture, value);
                return Activator.CreateInstance(_type,
                    BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { convertedValue }, null);
            }
            catch (NotSupportedException)
            { // note(cosborn) Read the source of GetConvertFromException; the return type is fake.
                throw (NotSupportedException)GetConvertFromException(value);
            }
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type,
        /// using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">
        /// A <see cref="Type"/> that represents the type you want to convert to.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this converter can perform the conversion; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == _underlyingType || // note(cosborn) Option<TSome> can (usually) become TSome.
            destinationType == _type || // note(cosborn) Natch.
            Nullable.GetUnderlyingType(destinationType) == _underlyingType || // note(cosborn) 1–1 mapping wtih TSome?.
            destinationType == typeof(InstanceDescriptor) || // todo(cosborn) MS claims this is important, but is hazy on exactly why.
            _underlyingTypeConverter.CanConvertTo(context, destinationType);

        /// <summary>
        /// Converts the given value object to the specified type,
        /// using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">
        /// A <see cref="CultureInfo"/>. If null is passed, the current culture is assumed.
        /// </param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">
        /// The <see cref="Type"/> to convert the <paramref name="value"/> parameter to.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="destinationType"/> parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        { // todo(cosborn) There is a strong possibility that this can be more efficient.
            if (value == null)
            {
                return ConvertToNull(context, culture, destinationType);
            }

            var valueType = value.GetType();
            if (valueType != _type)
            { // note(cosborn) We can't convert what we don't (claim to) know how to convert.
                return base.ConvertTo(context, culture, valueType, destinationType);
            }

            if (valueType == destinationType)
            { // note(cosborn) This is easy.
                return value;
            }

            // note(cosborn) See if the Option<TSome> is in None state.
            var isNone = (bool?)_type.GetProperty(nameof(Option<object>.IsNone))?.GetValue(value);
            if (isNone.GetValueOrDefault(true))
            { // note(cosborn) It was None!
                return ConvertToNull(context, culture, destinationType);
            }

            // note(cosborn) Now that we know that it's in the Some state...
            var underlyingValue = _type.GetField(nameof(Option<object>.SomeValue),
                BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);

            if (destinationType == _underlyingType)
            { // note(cosborn) This is also easy.
                return underlyingValue;
            }

            if (destinationType == typeof(InstanceDescriptor))
            {
                var constructor = _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new[] { _underlyingType }, null);
                return new InstanceDescriptor(constructor, new[] { underlyingValue }, true);
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
