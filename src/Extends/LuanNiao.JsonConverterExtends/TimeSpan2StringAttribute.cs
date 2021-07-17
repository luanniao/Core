using System;
using System.Text.Json.Serialization;

namespace LuanNiao.JsonConverterExtends
{
    public class TimeSpan2StringAttribute : JsonConverterAttribute
    {
        private readonly string _format;
        public TimeSpan2StringAttribute(string format = "yyyy-MM-dd")
        {
            _format = format;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {

            if (typeToConvert != typeof(TimeSpan) && typeToConvert != typeof(TimeSpan?))
            {
                throw new InvalidOperationException($"{nameof(TimeSpan2StringConverter)} just can use to type:{typeof(TimeSpan)} or type:{typeof(TimeSpan?)}");
            }

            if (typeToConvert == typeof(TimeSpan))
            {
                return new TimeSpan2StringConverter();
            }
            else if (typeToConvert == typeof(TimeSpan?))
            {
                return new NullableDateTime2StringConverter(_format);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(TimeSpan2StringConverter)} just can use to type:{typeof(TimeSpan)} or type:{typeof(TimeSpan?)}");
            }

        }

    }
}
