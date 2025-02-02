using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeChallenge.Helpers
{



    // .NET 6 does not provide serialization for a DateOnly datatype OOTB, so this class takes care of that.
    // Please also see App.AddServices() where this class is registered.
    // Source: https://stackoverflow.com/questions/74246482/system-notsupportedexception-serialization-and-deserialization-of-system-dateo

    // TODO: Write unit tests for this converter.  I found an article where the author created extension methods to encapsulate
    //       the stream operations needed: https://khalidabuhakmeh.com/systemtextjson-jsonconverter-test-helpers
    //       Adapt those for this converter, and then write tests to verify the logic.

    public class DateOnlyJsonConverter: JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.FromDateTime(reader.GetDateTime());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            var isoDate = value.ToString("O");
            writer.WriteStringValue(isoDate);
        }
    }
}
