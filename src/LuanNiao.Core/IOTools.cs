using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LuanNiao.Core
{
    public static class IOTools
    {
        /// <summary>
        /// get hte assembly path
        /// </summary>
        public static string CurrentAssemblyPath { get; } = Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.LastIndexOf('\\'));
    }
}
