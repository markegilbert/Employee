using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeChallenge.Helpers
{
    // .NET 6 does not provide serialization for a DateOnly datatype, so this class takes care of it.
    // Source: https://stackoverflow.com/questions/74246482/system-notsupportedexception-serialization-and-deserialization-of-system-dateo
    public class DateOnlyJsonConverter: JsonConverter<DateOnly>
    {

        // TODO: Unit tests for this
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.FromDateTime(reader.GetDateTime());
        }

        // TODO: Unit tests for this
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            var isoDate = value.ToString("O");
            writer.WriteStringValue(isoDate);
        }
    }
}
