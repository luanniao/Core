using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        public class FileConfig {
            public const string Logging = "LuanNiaoLogging";
            public string Path { get; set; } = "C:/LuanNiaoLog";
            public bool DateFormat { get; set; } = true;
            public int MaxLenth { get; set; } = 1024;

            public void CreateDirectory() {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
            }
        }
        private  FileConfig _fileConfig = new FileConfig();

        private void FileWritter(GrapherOptions options, EventWrittenEventArgs data)
        {
            if (_configuration == null) {
                InitConfig();
            }
            if (options.AsyncSettings.TryGetValue(data.Level, out var isAsync) && isAsync)
            {
                _consoleWriterQueue.Enqueue(MessageBuilder(options, data));
                _consoleSemaphore.Release();
            }
            else
            {
                var msg = MessageBuilder(options, data);
                textWriter.WriteLine(msg);
                Write(msg);
            }
        }
        private void InitConfig() {
            var fileName = "appsettings.json";
            var filePath = $"{AppContext.BaseDirectory.Replace("\\", "/")}{fileName}";
            if (File.Exists(filePath))
            {
                _configuration = new ConfigurationBuilder().AddJsonFile(filePath, false, true).Build();
                _fileConfig = new FileConfig() { Path = _configuration["LuanNiaoLogging:Path"] };
            }
            _fileConfig.CreateDirectory();
        }
        
        private void Write(string msg)
        {
            var filePath = $"{_fileConfig.Path}/log.txt";
            var streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine(msg);
            streamWriter.Close();
        }
    }
}
