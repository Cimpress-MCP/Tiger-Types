using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    /// <summary>Provides a way of converting to and from the <see cref="Option{TSome}"/> space.</summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "We know what we're calling.")]
    sealed class OptionTypeConverter
        : TypeConverter
    {
        readonly Type _type;
        readonly Type _wrappedType;
        readonly TypeConverter _wrappedTypeConverter;
        readonly MethodInfo _optionFrom;

        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
        public OptionTypeConverter([NotNull] Type type)
        {
            Requires<ArgumentNullException>(type != null);
            Requires<ArgumentException>(type.GetGenericTypeDefinition() == typeof(Option<>));
            Assert(type.GetGenericArguments() != null); // note(cosborn) Trivially true, if above is.
            Assert(type.GetGenericArguments().Length > 0); // note(cosborn) Trivially true, if above is.

            _type = type; // note(cosborn) This is Option<TSome>.
            _wrappedType = type.GetGenericArguments()[0]; // note(cosborn) This is TSome itself.
            Assume(_wrappedType != null);

            _wrappedTypeConverter = TypeDescriptor.GetConverter(_wrappedType);

            Assume(typeof(Option<>).IsGenericTypeDefinition);
            Assume(typeof(Option<>).GetGenericArguments().Length == 1);
            _optionFrom = typeof(Option<>).MakeGenericType(_wrappedType)
                                          .GetMethod(nameof(Option<object>.From));
            Assume(_optionFrom != null);
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
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == _wrappedType || // note(cosborn) TSome can become Option<TSome>.
                sourceType.IsNullableOf(_wrappedType) || // note(cosborn) 1–1 mapping wtih TSome?.
                _wrappedTypeConverter.CanConvertFrom(context, sourceType);
        }

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
            if (value == null) { return Option.None; }

            var stringValue = value as string;
            if (stringValue != null && stringValue.Length == 0)
            { // note(cosborn) For TypeConverter purposes, an empty string is "no value."
                return Option.None;
            }

            if (value.GetType() == _wrappedType)
            { // note(cosborn) Wrap it up!
                return _optionFrom.Invoke(null, new[] { value });
            }

            try
            {
                return _optionFrom.Invoke(null, new[] { _wrappedTypeConverter.ConvertFrom(context, culture, value) });
            }
            catch (NotSupportedException)
            { // ReSharper disable once ThrowingSystemException note(cosborn) It's actually NotSupportedException.
                throw GetConvertFromException(value);
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
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            Assume(destinationType != null);

            return destinationType == _wrappedType || // note(cosborn) Option<TSome> can (usually) become TSome.
                   destinationType == _type || // note(cosborn) Natch.
                   destinationType.IsNullableOf(_wrappedType) || // note(cosborn) 1–1 mapping wtih TSome?.
                   destinationType == typeof(InstanceDescriptor) || // todo(cosborn) MS claims this is important, but is hazy on exactly why.
                   _wrappedTypeConverter.CanConvertTo(context, destinationType);
        }

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
                // ReSharper disable once ThrowingSystemException note(cosborn) It's really NotSupportedException.
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
            var valueProperty = _type.GetProperty(nameof(Option<object>.Value));
            Assume(valueProperty != null);
            var wrappedValue = valueProperty.GetValue(value);

            if (destinationType == _wrappedType) { return wrappedValue; }

            if (destinationType == typeof(InstanceDescriptor))
            {
                var constructor = _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                    null, CallingConventions.Any, new[] { _wrappedType }, null);
                Assume(_type.GetGenericArguments() != null);
                return new InstanceDescriptor(constructor, new[] { wrappedValue }, true);
            }

            try
            {
                return _wrappedTypeConverter.ConvertTo(context, culture, wrappedValue, destinationType);
            }
            catch (NotSupportedException)
            { // ReSharper disable once ThrowingSystemException note(cosborn) It's actually NotSupportedException.
                throw GetConvertToException(valueType, destinationType);
            }
        }

        object ConvertToNull(ITypeDescriptorContext context, CultureInfo culture, Type destinationType)
        {
            return destinationType.IsNullableOf(_wrappedType)
                ? null
                : base.ConvertTo(context, culture, null, destinationType);
        }
    }

    static class TypeExtensions
    {
        public static bool IsNullableOf([NotNull] this Type type, [NotNull] Type otherType)
        {
            Requires<ArgumentNullException>(otherType != null);

            if (!otherType.IsValueType)
            { // It can't possibly be; only value types can be nullable.
                return false;
            }

            Assume(typeof(Nullable<>).GetGenericArguments() != null);
            // ReSharper disable once ExceptionNotDocumented note(cosborn) It is definitely 1.
            Assume(typeof(Nullable<>).GetGenericArguments().Length == 1);
            Assume(typeof(Nullable<>).IsGenericTypeDefinition);
            return type == typeof(Nullable<>).MakeGenericType(otherType);
        }
    }
}
