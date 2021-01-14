using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static LuanNiao.JsonConverterExtends.Constants;

namespace LuanNiao.JsonConverterExtends
{
    public interface IUseLNJsonExtends { }
    public static class JsonExtends
    {
        private static JsonSerializerOptions GetOptions(bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true)
        {

            JsonSerializerOptions options = null;
            if (camelCase && withChinese&&nameCaseInsensitive)
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
        public static byte[] GetBytesWithCompress<T>(this T target, bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true) where T : class, IUseLNJsonExtends
        {
            if (target == null)
            {
                return null;
            }
            return GetBytesWithCompress(target, GetOptions(camelCase, withChinese, nameCaseInsensitive));
        }


        public static byte[] GetBytesWithHightLevelCompress<T>(this T target, bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true) where T : class, IUseLNJsonExtends
        {
            if (target == null)
            {
                return null;
            }
            return GetBytesWithHightLevelCompress(target, GetOptions(camelCase, withChinese, nameCaseInsensitive));
        }




        public static byte[] GetBytesWithCompress<T>(this T target, JsonSerializerOptions options) where T : class, IUseLNJsonExtends
        {
            return Core.TextTools.BrotliUTF8.Compress(JsonSerializer.Serialize(target, options));
        }
        public static byte[] GetBytesWithHightLevelCompress<T>(this T target, JsonSerializerOptions options) where T : class, IUseLNJsonExtends
        {
            return Core.TextTools.BrotliUTF8.CompressHightLevel(JsonSerializer.Serialize(target, options));
        }






        public static T GetObjectWithCompress<T>(this byte[] source, bool camelCase = true, bool withChinese = true, bool nameCaseInsensitive = true) where T : class, IUseLNJsonExtends
        {
            if (source == null)
            {
                return null;
            } 

            return JsonSerializer.Deserialize<T>(Core.TextTools.BrotliUTF8.GetString(source), GetOptions(camelCase,withChinese,nameCaseInsensitive));
        }
    }
}
