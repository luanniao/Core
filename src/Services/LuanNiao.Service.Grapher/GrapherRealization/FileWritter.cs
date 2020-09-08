using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuanNiao.Service.Grapher
{
    public sealed partial class Grapher
    {
        public class FileConfig
        {
            public string LoggingName = "LuanNiaoLogging";
            public string Path { get; set; } = "C:/LuanNiaoLogs";
            public bool DateFormat { get; set; } = true;
            public int MaxLenth { get; set; } = 1024;
            public void CreateDirectory()
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
            }
            public string FileName
            {
                get
                {
                    var extName = "txt";
                    var fileName = "log";
                    var isNewFile = false;
                    if (this.DateFormat)
                    {
                        fileName = $"{fileName}_{DateTime.Today.ToString("yyyy-MM-dd")}";
                    }
                    if (this.MaxLenth > 0)
                    {
                        fileName = $"{fileName}.";
                        var dirct = new DirectoryInfo(this.Path);
                        var logLength = 0;
                        var lastFile = dirct.GetFiles($"*{fileName}*.{extName}").OrderByDescending(t => t.LastWriteTime).FirstOrDefault();
                        if (lastFile != null && lastFile.Exists)
                        {
                            fileName = lastFile.Name;
                        }
                        else
                        {
                            fileName = $"{fileName}{logLength}.{extName}";
                            lastFile = dirct.GetFiles(fileName).FirstOrDefault();
                        }
                        isNewFile = lastFile != null && lastFile.Length > this.MaxLenth;
                        if (isNewFile)
                        {
                            var spFile = fileName.Split(".");
                            int.TryParse(spFile[1], out logLength);
                            logLength++;
                            fileName = $"{spFile[0]}.{logLength}.{extName}";
                        }
                    }
                    return fileName;
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
            var filePath = $"{_fileConfig.Path}/{_fileConfig.FileName}";
            var streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine(msg);
            streamWriter.Close();
        }

    }
}
