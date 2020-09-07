using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        public class FileConfig {
            public string LoggingName = "LuanNiaoLogging";
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
                _fileConfig  = new ServiceCollection().AddOptions().Configure<FileConfig>(_configuration.GetSection(_fileConfig.LoggingName))
                    .BuildServiceProvider()
                    .GetService<IOptions<FileConfig>>()
                    .Value;
            }
            _fileConfig.CreateDirectory();
        }
        
        private void Write(string msg)
        {
            var filePath = $"{_fileConfig.Path}/{{0}}.txt";
            var fileName = "log";
            if (_fileConfig.DateFormat)
            {
                fileName = $"{fileName}_{DateTime.Today.ToString("yyyy-MM-dd")}";
            }
            filePath = string.Format(filePath, fileName);
            //if (_fileConfig.MaxLenth > 0 && File.Exists(filePath))
            //{
            //    FileInfo info = new FileInfo(filePath);
            //    if (info.Length > _fileConfig.MaxLenth)
            //    {
                    
            //    }
            //}
            var streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine(msg);
            streamWriter.Close();
        }
    }
}
