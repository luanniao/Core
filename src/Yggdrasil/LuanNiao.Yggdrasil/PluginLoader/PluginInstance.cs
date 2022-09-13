using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using LuanNiao.Yggdrasil.ASPNet;
using LuanNiao.Yggdrasil.Job;

namespace LuanNiao.Yggdrasil.PluginLoader
{
    /// <summary>
    /// API插件选项
    /// </summary>
    public class PluginInstance
    {
        /// <summary>
        /// 插件信息
        /// </summary>
        private readonly PluginItem _pluginItem;


        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly? Assembly { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; init; } = "";
        /// <summary>
        /// 文件sha256
        /// </summary>
        public string Sha256 { get; private set; } = "";
        /// <summary>
        /// 版本
        /// </summary>
        public Version Version { get; private set; } = new();
        /// <summary>
        /// 插件文件地址
        /// </summary>
        public string PluginFilePath { get; private set; } = "";
        /// <summary>
        /// 插件文件夹地址
        /// </summary>
        public string PluginFolderPath { get; private set; } = "";
        /// <summary>
        /// 是否是发布版本
        /// </summary>
        public bool IsRelease { get; init; } = false;

        /// <summary>
        /// 入口实例
        /// </summary>
        public AbsEntrance? EnteranceInstance { get; private set; }

        /// <summary>
        /// 所有的GRPC服务类型
        /// </summary>
        public List<Type> AllGrpcServices { get; init; } = new();
        /// <summary>
        /// 所有的API服务
        /// </summary>
        public List<Type> AllApi { get; init; } = new();

        /// <summary>
        /// 启动异常信息
        /// </summary>
        public string? StartExceptionMessage { get; set; }
        /// <summary>
        /// 所有的Job服务
        /// </summary>
        public List<Type> AllJobs { get; init; } = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginItem"></param>
        public PluginInstance(PluginItem pluginItem)
        {
            _pluginItem = pluginItem;
            Name = pluginItem.Name;
        }

        /// <summary>
        /// 检查路径信息
        /// </summary>
        /// <returns></returns>
        public bool CheckPathInfo()
        {
            PluginFilePath = Path.IsPathRooted(_pluginItem.FilePath) ? _pluginItem.FilePath : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _pluginItem.FilePath));
            PluginFolderPath = new FileInfo(PluginFilePath).DirectoryName ?? "";
            return File.Exists(PluginFilePath);
        }


        /// <summary>
        /// 尝试创建入口
        /// </summary>
        /// <param name="instance">入口实例</param>
        /// <returns></returns>
        public bool TryCreateEntrance(out AbsEntrance? instance)
        {
            instance = null;
            Assembly = Assembly.LoadFrom(PluginFilePath);
            var tempInfo = Assembly.CreateInstance(_pluginItem.EntranceTypeDesc);
            if (tempInfo is AbsEntrance entrance && Assembly is not null)
            {
                EnteranceInstance = instance = entrance;
                Version = Assembly.GetName().Version ?? new Version();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取引用的库版本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Version? GetReferencedLibVersion(string name)
        {
            if (Assembly is null)
            {
                return default;
            }
            var info = Assembly.GetReferencedAssemblies().FirstOrDefault(item => !string.IsNullOrWhiteSpace(item.Name) && item.Name.Equals
    (name));
            if (info is null)
            {
                return default;
            }
            return info.Version;
        }

        /// <summary>
        /// 加载所有插件
        /// </summary>
        public void LoadAllPlugins()
        {
            if (Assembly is null)
            {
                return;
            }
            var allTypes = Assembly.GetExportedTypes();
            foreach (var type in allTypes)
            {
                var grpcAttr = type.GetCustomAttribute(typeof(LNGRPCServerAttribute), true);
                var apiAttr = type.GetCustomAttribute(typeof(LNAPIServerAttribute), true);
                var jobAttr = type.GetCustomAttribute(typeof(LNJobAttribute), true);
                if (grpcAttr is not null)
                {
                    AllGrpcServices.Add(type);
                }
                if (apiAttr is not null)
                {
                    AllApi.Add(type);
                }
                if (jobAttr is not null)
                {
                    AllJobs.Add(type);
                }
            }
        }
    }
}
