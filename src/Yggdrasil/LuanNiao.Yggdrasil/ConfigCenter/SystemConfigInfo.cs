using System;
using System.Collections.Generic;
using System.Linq;

using LuanNiao.Yggdrasil.Cache;
using LuanNiao.Yggdrasil.ConfigCenter.DBModels;

namespace LuanNiao.Yggdrasil.ConfigCenter
{
    /// <summary>
    /// 系统配置信息----不支持分布式.
    /// </summary>
    public class SystemConfigInfo
    {
        /// <summary>
        /// 配置主键ID
        /// </summary> 
        public long SCID { get; set; } = Core.IDGen.Instance.NextId();
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = string.Empty;
        /// <summary>
        /// 父级配置ID
        /// </summary>
        public long ParentSCID { get; set; }

        /// <summary>
        /// 类型枚举
        /// </summary> 
        public int Type { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<SystemConfigInfo> Childs { get; set; } = new List<SystemConfigInfo>();



        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<SystemConfigInfo>? GetConfigTree(AbsYggdrasilContext context)
        {
            return context.Cache.GetOrSet($"SystemConfigCenter", () =>
              {
                  if (context.DB.Default is null)
                  {
                      return new List<SystemConfigInfo>();
                  }
                  var allConfigs = context.DB.Default.Select<SystemConfig>().ToList();

                  var tempConfigs = new List<SystemConfigInfo>();
                  allConfigs.Where(item => item.ParentSCID == 0).ToList().ForEach(item =>
                  {
                      var temp = new SystemConfigInfo()
                      {
                          Name = item.Name,
                          ParentSCID = item.ParentSCID,
                          Type = item.Type,
                          Value = item.Value,
                          SCID = item.SCID
                      };
                      ConstructData(allConfigs, temp);
                      tempConfigs.Add(temp);
                  });

                  return tempConfigs;
              }, new CacheItemSettings()
              {
                  AbsoluteExpiration = DateTime.Now.AddYears(1)
              });

        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="context"></param>
        public static void RefreshCache(AbsYggdrasilContext context)
        {
            context.Cache.Remove("SystemConfigCenter");
            _ = GetConfigTree(context);
        }

        /// <summary>
        /// 构造数据
        /// </summary>
        /// <param name="allConfigs">所有的配置</param>
        /// <param name="info">当前的节点</param>
        private static void ConstructData(List<SystemConfig> allConfigs, SystemConfigInfo info)
        {
            allConfigs.Where(item => item.ParentSCID == info.SCID).ToList().ForEach(item =>
            {
                var temp = new SystemConfigInfo()
                {
                    Name = item.Name,
                    ParentSCID = item.ParentSCID,
                    Type = item.Type,
                    Value = item.Value,
                    SCID = item.SCID
                };
                info.Childs.Add(temp);
                ConstructData(allConfigs, temp);
            });
        }

        /// <summary>
        /// 根据路径获取数据
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetByPath(AbsYggdrasilContext context, string[] path)
        {
            var data = GetConfigTree(context);
            if (data is null)
            {
                return string.Empty;
            }
            SystemConfigInfo? current = null;

            foreach (var pathItem in path)
            {
                if (current is null)
                {
                    current = data.FirstOrDefault(item => item.Name.Equals(pathItem, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    current = current.Childs.FirstOrDefault(item => item.Name.Equals(pathItem, StringComparison.OrdinalIgnoreCase));
                }
                if (current is null)
                {
                    return string.Empty;
                }
            }
            if (current is null)
            {
                return string.Empty;
            }

            return current.Value;
        }
    }
}
