using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LuanNiao.Services.Logger.Handler
{
    internal class LogHandler : EventListener
    {
        private LogHandler() { }
        private static LogHandler _instance = null;
        private readonly Dictionary<string, HandlerOptions> _handlerOptions = new Dictionary<string, HandlerOptions>();

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
                var configInfo = JsonSerializer.Deserialize<List<HandlerOptions>>(content);
                Init(configInfo);
            }
        }

        public static void Init([NotNull] IList<HandlerOptions> handlerOptions)
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
                else if (!handlerOptions.Any(item => item.SourceName.Equals(Common.Constants.LOGGER_EVENT_SOURCE_NAME, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new Exception("You must set the logger config info in to your json.");
                }
                _instance = new LogHandler();
                foreach (var item in handlerOptions)
                {
                    _instance._handlerOptions.Add(item.SourceName, item);
                }
            }
        }

        protected override void OnEventSourceCreated(EventSource source)
        {
            if (!_handlerOptions.TryGetValue(source.Name,out var handler))
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
