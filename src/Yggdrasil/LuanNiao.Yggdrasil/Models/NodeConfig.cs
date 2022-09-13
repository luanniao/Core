using System.Collections.Generic;

using LuanNiao.Yggdrasil.Logger.Models;
using LuanNiao.Yggdrasil.PluginLoader;

namespace LuanNiao.Yggdrasil.Models
{
    /// <summary>
    /// 节点配置
    /// </summary>
    public class NodeConfig
    {
        /// <summary>
        /// 对这个服务的描述信息
        /// </summary>
        public string Desc { get; set; } = string.Empty;
        /// <summary>
        /// 只启动Job服务
        /// </summary>
        public bool JustJobServer { get; set; }
        /// <summary>
        /// 本地数据库类型
        /// </summary>
        public int DBType { get; set; }
        /// <summary>
        /// 本地数据库地址
        /// </summary>
        public string DBAddress { get; set; } = string.Empty;
        /// <summary>
        /// 本地读数据库地址
        /// </summary>
        public List<string> ReadDBAddress { get; set; } = new();

        /// <summary>
        /// 所有的插件
        /// </summary>
        public List<PluginItem> Plugins { get; set; } = new List<PluginItem>();
        /// <summary>
        /// 插件文件夹,默认TTPPlugins
        /// </summary>
        public string PluginsFolder { get; set; } = "plugins";

        /// <summary>
        /// 系统日志配置
        /// </summary>
        public LogOptions SystemLog { get; set; } = new LogOptions();

        /// <summary>
        /// webAPI配置信息,如果jobserver则可能为空,如果为空,则一定为job server
        /// </summary>

        public WebApiConfig? WebAPIConfig { get; set; }

    }
}
