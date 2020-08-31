using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LuanNiao.Service.Grapher
{
    public class Grapher : EventListener
    {
        private Grapher() { }
        private static Grapher _instance = null;
        private readonly Dictionary<string, GrapherOptions> _handlerOptions = new Dictionary<string, GrapherOptions>();

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

        public static void Init([NotNull] IList<GrapherOptions> handlerOptions)
        {
            if (_instance != null)
            {
                return;
            }
            lock (_instance)
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

        protected override void OnEventSourceCreated(EventSource source)
        {
            if (!_handlerOptions.TryGetValue(source.Name, out var handler))
            {
                return;
            }
            EnableEvents(source, handler.Level, handler.Keywords, handler.Arguments);
        }
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (!_handlerOptions.TryGetValue(eventData.EventSource.Name, out var handler))
            {
                return;
            }
            for (int i = 0; i < eventData.Payload.Count; ++i)
            {
                Console.WriteLine($"{eventData.Payload[i]}");
            }
        }
    }
}
