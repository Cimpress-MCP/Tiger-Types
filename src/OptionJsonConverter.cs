using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tiger.Types.Properties;
using static System.Diagnostics.Contracts.Contract;

namespace Tiger.Types
{
    //todo(cosborn) This should be its own library!
    /// <summary>
    /// Provides the capabilities to serialize and deserialize <see cref="Option{TSome}"/> to and from JSON.
    /// </summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Using nameof() makes reflection safer.")]
    public sealed class OptionJsonConverter
        : JsonConverter
    {
        /// <summary>Determines whether this instance can convert the specified object type.</summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <see langword="true"/> if this instance can convert the specified object type;
        /// otherwise, <see langword="true"/>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null &&
                   objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(Option<>);
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value), Resources.IncompatibleValue); }

            var objectType = value.GetType();
            Assume(objectType.IsGenericType, Resources.IncompatibleValue);
            Assume(objectType.GetGenericTypeDefinition() == typeof(Option<>), Resources.IncompatibleValue);

            var isNone = (bool?)objectType.GetProperty(nameof(Option<object>.IsNone))?.GetValue(value);
            if (isNone.GetValueOrDefault(true))
            {
                serializer.Serialize(writer, null);
                return;
            }

            var underlyingType = Option.GetUnderlyingType(objectType);
            var underlyingValue = objectType.GetField(nameof(Option<object>.SomeValue),
                BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);
            serializer.Serialize(writer, underlyingValue, underlyingType);
        }


        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var underlyingType = Option.GetUnderlyingType(objectType);
            Assume(underlyingType != null, Resources.IncompatibleType);

            return Option.From(reader.ValueType)
                         .Bind<object>(_ => serializer.Deserialize(reader, underlyingType))
                         .Map(v => Activator.CreateInstance(objectType,
                            BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { v }, null))
                         .GetValueOrDefault(() => Activator.CreateInstance(objectType));
        }
    }
}
