using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        public static void Init(string configFilePath)
        {
            if (_instance != null)
            {
                return;
            }
            var configFile = new FileInfo(configFilePath);
            if (!configFile.Exists)
            {
                throw new FileNotFoundException($"The file in the path:{configFilePath}, wasn't exists.");
            }
            using (var fr = configFile.OpenText())
            {
                var content = fr.ReadToEnd();

                var configInfo = JsonSerializer.Deserialize<List<GrapherOptions>>(content);
                Init(configInfo);
            }
        }

        /// <summary>
        /// Get all options which in the grapher instance.
        /// </summary>
        /// <returns></returns>
        public static IList<GrapherOptions> GetAllOptions()
        {
            return _instance._handlerOptions.Values.ToList();
        }

        public static void ResetConfig([NotNull] IList<GrapherOptions> handlerOptions)
        {
            if (handlerOptions == null || handlerOptions.Count == 0 || _instance == null)
            {
                return;
            }
            _instance._handlerOptions.Clear(); 
            foreach (var item in handlerOptions)
            {
                _instance._handlerOptions.Add(item.SourceName, item);
            }
        }

        /// <summary>
        /// Use the custom options to init the grapher
        /// </summary>
        /// <param name="handlerOptions"></param>
        public static void Init([NotNull] IList<GrapherOptions> handlerOptions)
        {
            if (handlerOptions == null || handlerOptions.Count == 0 || _instance != null)
            { 
                return;
            }
            lock (_lock)
            {
                if (_instance != null)
                {
                    return;
                }
                if (handlerOptions.GroupBy(item => item.SourceName.ToLower()).Any(item => item.Count() > 1))
                {
                    throw new Exception("Your log configration was invalid, Please check the your config content.");
                }
                _instance = new Grapher();
                foreach (var item in handlerOptions)
                {
                    _instance._handlerOptions.Add(item.SourceName, item);
                }
            }
        }
    }
}
