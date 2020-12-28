using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LuanNiao.JsonConverterExtends
{
    public class Long2StringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var rawData = reader.GetString();
            if (Int64.TryParse(rawData, out var data))
            {
                return data;
            }
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) => writer?.WriteStringValue(value.ToString());
    }
}
