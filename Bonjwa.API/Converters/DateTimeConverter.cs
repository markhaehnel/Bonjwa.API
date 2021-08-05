using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bonjwa.API.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (options is null) throw new ArgumentNullException(nameof(options));

            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture));
        }
    }
}
