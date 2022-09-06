﻿namespace LuanNiao.Yggdrasil.PluginLoader
{
    /// <summary>
    /// 插件描述
    /// </summary>
    public class PluginItem
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = "";
        /// <summary>
        /// 入口的类型描述
        /// </summary>
        public string EntranceTypeDesc { get; set; } = "";
    }
}
