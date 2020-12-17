using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LuanNiao.JsonConverterExtends
{
    public class DateTime2StringAtrribute : JsonConverterAttribute
    {
        private readonly string _format;
        public DateTime2StringAtrribute(string format = "yyyy-MM-dd")
        {
            _format = format;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {

            if (typeToConvert != typeof(DateTime) && typeToConvert != typeof(DateTime?))
            {
                throw new InvalidOperationException($"{nameof(DateTime2StringAtrribute)} just can use to type:{typeof(DateTime)} or type:{typeof(DateTime?)}");
            }

            if (typeToConvert == typeof(DateTime))
            {
                return new DateTime2StringConverter(_format);
            }
            else if (typeToConvert == typeof(DateTime?))
            {
                return new NullableDateTime2StringConverter(_format);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(DateTime2StringAtrribute)} just can use to type:{typeof(DateTime)} or type:{typeof(DateTime?)}");
            }

        }

    }
}
