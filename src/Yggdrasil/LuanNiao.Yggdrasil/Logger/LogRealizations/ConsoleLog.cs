using System;

namespace LuanNiao.Yggdrasil.Logger.LogRealizations
{
    internal class ConsoleLog
    {
        /// <summary>
        /// ConsoleLog的实例化对象
        /// </summary>
        public static ConsoleLog Instance = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        private ConsoleLog() { }

        /// <summary>
        /// 输出控制台日志
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="bgColor">日志背景色</param>
        /// <param name="fontColor">字体颜色</param>
#pragma warning disable CA1822 // 将成员标记为 static
        public void Write(string text, ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fontColor = ConsoleColor.White)
#pragma warning restore CA1822 // 将成员标记为 static
        {
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fontColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
