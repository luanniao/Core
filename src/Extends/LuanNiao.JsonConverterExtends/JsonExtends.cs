using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static LuanNiao.JsonConverterExtends.CommonSerializerOptions;

namespace LuanNiao.JsonConverterExtends
{
    public interface IUseLNJsonExtends { }
    public enum JsonCompressLevel
    {
        None = 0,
        Normal = 1,
        High = 2
    }
    /*
        |             Method |         Mean |       Error |      StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
        |------------------- |-------------:|------------:|------------:|-------:|------:|------:|----------:|
        |   TestHighCompress | 699,485.9 ns | 2,587.15 ns | 2,293.44 ns |      - |     - |     - |     976 B |
        | TestNormalCompress |   4,744.4 ns |    31.86 ns |    28.24 ns | 0.1526 |     - |     - |     984 B |
        |   TestNoneCompress |     484.3 ns |     5.86 ns |     5.48 ns | 0.0629 |     - |     - |     400 B |
     */
    public static class JsonExtends
    {
        private static JsonSerializerOptions GetOptions(bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true)
        {

            JsonSerializerOptions options = null;
            if (camelCase && withChinese && nameCaseInsensitive)
            {
                options = CamelCaseChineseNameCaseInsensitive;
            }
            else if (!camelCase && withChinese && nameCaseInsensitive)
            {
                options = ChineseNameCaseInsensitive;
            }
            else if (!camelCase && !withChinese && nameCaseInsensitive)
            {
                options = NameCaseInsensitive;
            }
            else if (camelCase && !withChinese && nameCaseInsensitive)
            {
                options = CamelCaseNameCaseInsensitive;
            }
            else if (camelCase && !withChinese && !nameCaseInsensitive)
            {
                options = CamelCase;
            }
            else if (camelCase && withChinese && !nameCaseInsensitive)
            {
                options = CamelCaseChinese;
            }
            return options;
        }


        public static byte[] GetBytes<T>(this T target, JsonCompressLevel compress = JsonCompressLevel.None, bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true, JsonSerializerOptions options = null) where T : class, IUseLNJsonExtends
        {
            if (target == null)
            {
                return null;
            }
            if (options==null)
            {
                options = GetOptions(camelCase, withChinese, nameCaseInsensitive);
            }
            switch (compress)
            {
                case JsonCompressLevel.Normal:
                    return Core.TextTools.BrotliUTF8.Compress(JsonSerializer.Serialize(target, options));
                case JsonCompressLevel.High:
                    return Core.TextTools.BrotliUTF8.CompressHightLevel(JsonSerializer.Serialize(target, options));
                case JsonCompressLevel.None:
                default:
                    return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(target, options));
            }
        } 



        public static T GetObject<T>(this byte[] source, JsonCompressLevel compress = JsonCompressLevel.None, bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true) where T : class, IUseLNJsonExtends
        {
            if (source == null)
            {
                return null;
            }
            switch (compress)
            {
                case JsonCompressLevel.High:
                case JsonCompressLevel.Normal:
                    return JsonSerializer.Deserialize<T>(Core.TextTools.BrotliUTF8.GetString(source), GetOptions(camelCase, withChinese, nameCaseInsensitive));
                case JsonCompressLevel.None:
                default:
                    return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(source), GetOptions(camelCase, withChinese, nameCaseInsensitive));
            }
            
        }
    }
}
