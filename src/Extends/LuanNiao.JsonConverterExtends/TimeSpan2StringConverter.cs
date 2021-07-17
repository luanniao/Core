using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LuanNiao.JsonConverterExtends
{
    public class TimeSpan2StringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new NotSupportedException();
            if (typeToConvert != typeof(TimeSpan))
                throw new NotSupportedException();
            return TimeSpan.ParseExact(reader.GetString(), "c", CultureInfo.InvariantCulture);
        }
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("c", CultureInfo.InvariantCulture));
        }
    }
}
