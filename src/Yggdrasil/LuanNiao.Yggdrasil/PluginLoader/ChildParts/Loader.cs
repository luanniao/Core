using System;
using System.Collections.Generic;

using LuanNiao.Yggdrasil.PluginLoader;


namespace TTP.Container.PluginLoader
{
    /// <summary>
    /// 管理器
    /// </summary>
    public partial class Manager
    {

        private readonly List<PluginInstance> _items = new();
        /// <summary>
        /// 加载指定路径
        /// </summary>
        /// <param name="folderPath"></param>
        public bool LoadPlugin(PluginItem plugin)
        {

            var pluginInstance = new PluginInstance(plugin);
            if (!pluginInstance.CheckPathInfo())
            {
                _logger.Warning($"File {pluginInstance.PluginFilePath} not exists, plugin:{plugin.Name} load failed.");
                return false;
            }
            try
            {
                if (!pluginInstance.TryCreateEntrance(out var entranceInstance) || entranceInstance is null)
                {
                    _logger.Warning($"Plugin {plugin.Name}'s enterance type:{plugin.EntranceTypeDesc} create failed.");
                    return false;
                }
                var absVersion = pluginInstance.GetReferencedLibVersion(this._absName);
                if (absVersion is null)
                {
                    _logger.Warning($"Plugin {plugin.Name}'s has not absEntrance.");
                    return false;
                }
                if (absVersion.Major != this._absVersion.Major)
                {
                    _logger.Warning($"Skip load pliugin {plugin.Name}, cause it's TTP.Container.Abstracts Major version is:{absVersion.Major}, but current container's Major version is:{this._absVersion.Major}.");

                    return false;
                }
                else if (absVersion.Minor != this._absVersion.Minor)
                {
                    _logger.Warning($"Skip load pliugin {plugin.Name}, cause it's TTP.Container.Abstracts Minor version is:{absVersion.Minor}, but current container's Minor version is:{this._absVersion.Minor}.");
                    return false;
                }
                else if (this._absVersion.Revision != -1 && (absVersion.Build != this._absVersion.Build || absVersion.Revision != this._absVersion.Revision))
                {
                    _logger.Information($"Pliugin {plugin.Name}, it's TTP.Container.Abstracts version is:{absVersion}, but current container's version is:{this._absVersion}, check them, it maybe has some bug.");
                }

                pluginInstance.LoadAllPlugins();
                _items.Add(pluginInstance);
                _logger.Information($"Pliugin {plugin.Name} load completed.");
            }
            catch (Exception ex)
            {
                _logger.Error($"load plugin {plugin.Name} error, got {ex.Message}.");
            }
            return true;

        }
    }
}
