using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LuanNiao.JsonConverterExtends
{
    public class NullableDateTime2StringConverter : JsonConverter<DateTime?>
    {

        private readonly string _format;
        public NullableDateTime2StringConverter(string format)
        {
            _format = format;
        }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var raw = reader.GetString();
                if (DateTime.TryParse(raw, out var data))
                {
                    return data;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteStringValue((string)null);
            }
            else
            {
                writer.WriteStringValue(value.Value.ToString(_format));
            }
        }
    }
}
