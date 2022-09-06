using System.Text.Json;
using System.Text.Unicode;

namespace LuanNiao.JsonConverterExtends
{
    public static class CommonSerializerOptions
    {
        public static readonly JsonSerializerOptions CamelCase = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static readonly JsonSerializerOptions NameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static readonly JsonSerializerOptions ChineseOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
        };


        public static readonly JsonSerializerOptions CamelCaseChinese = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),
        };
        public static readonly JsonSerializerOptions CamelCaseNameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public static readonly JsonSerializerOptions ChineseNameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
        };


        public static readonly JsonSerializerOptions CamelCaseChineseNameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),
        };

    }
}
