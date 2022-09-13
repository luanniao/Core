using System;

using LuanNiao.Yggdrasil.Logger.AbsInfos;
using LuanNiao.Yggdrasil.Logger.LogRealizations;
using LuanNiao.Yggdrasil.Logger.Models;

namespace LuanNiao.Yggdrasil.Logger
{
    /// <summary>
    /// 日志中心
    /// </summary>
    internal class LogCenter : AbsLNLogger, IDisposable
    {
        /// <summary>
        /// 选项
        /// </summary>
        private readonly LogOptions _options;

        /// <summary>
        /// 文件日志实例
        /// </summary>
        private readonly FileLog? _fileLogInstance;

        /// <summary>
        /// 区域号
        /// </summary>
        private readonly string? _scope;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="options">配置信息</param>
        public LogCenter(string moduleName, LogOptions options, FileLog? fileLog) : this(moduleName, options, fileLog, null)
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="options">配置信息</param>
        public LogCenter(string moduleName, LogOptions options, FileLog? fileLog, string? scopeName) : base(moduleName)
        {
            _fileLogInstance = fileLog;
            _options = options;
            _scope = scopeName;
        }

        /// <summary>
        /// 销毁函数
        /// </summary>
        protected override void Dispose(bool flag)
        {
            if (_disposed) return;
            if (_scope is not null)
            {
                try
                {
                    _fileLogInstance?.Dispose();
                    base.Dispose(flag);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dispose Log Center error:{ex.Message}");
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="text">文本</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="exception">异常</param>
        /// <param name="filePath">文件名</param>
        /// <param name="name">函数名</param>
        /// <param name="num">行数</param>
        public override void Log(LogLevelEnum level, string text, string? eventID, Exception? exception, string filePath, int num, string name)
        {
            if (_options.EnableConsole && _options.MinLevel <= level)
            {
                WriteConsoleLog(level, text, eventID, exception, filePath, num, name);
            }
            if (_options.FileLog.EnableFileLog && _options.MinLevel <= level)
            {
                WriteFileLog(level, text, eventID, exception, filePath, num, name);
            }
            LNLoggerProvider.Instance?.WriteLog(_moduleName, _scope, level, text, eventID, exception, filePath, num, name);
        }

        /// <summary>
        /// 写入console日志
        /// </summary> 
        /// <param name="level">级别</param>
        /// <param name="text">文本</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="exception">异常</param>
        private void WriteConsoleLog(LogLevelEnum level, string text, string? eventID, Exception? exception, string filePath, int num, string name)
        {
            switch (level)
            {
                case LogLevelEnum.Trace:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name));
                    break;
                case LogLevelEnum.Debug:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name));
                    break;
                case LogLevelEnum.Information:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name), fontColor: ConsoleColor.Green);
                    break;
                case LogLevelEnum.Warning:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name), ConsoleColor.Yellow, ConsoleColor.Black);
                    break;
                case LogLevelEnum.Error:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name), ConsoleColor.Red, ConsoleColor.White);
                    break;
                case LogLevelEnum.Critical:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name), ConsoleColor.DarkRed, ConsoleColor.White);
                    break;
                default:
                    ConsoleLog.Instance.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name), ConsoleColor.Green, ConsoleColor.Red);
                    break;
            }
        }

        /// <summary>
        /// 写入文件日志
        /// </summary> 
        /// <param name="level">级别</param>
        /// <param name="text">文本</param>
        /// <param name="eventID">事件ID</param>
        /// <param name="exception">异常</param>
        private void WriteFileLog(LogLevelEnum level, string text, string? eventID, Exception? exception, string filePath, int num, string name)
        {
            _fileLogInstance?.Write(GetLogFullString(level, text, eventID, exception, filePath, num, name));
        }


    }
}
