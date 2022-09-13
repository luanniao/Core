using System.Collections.Generic;

using LuanNiao.Yggdrasil.ConfigCenter.DBModels;

namespace LuanNiao.Yggdrasil.ConfigCenter.Operations
{
    /// <summary>
    /// 删除一个配置信息
    /// </summary>
    public class DeleteConfigDataRequest
    {

        /// <summary>
        /// 配置ID
        /// </summary>
        public long SCID { get; set; }

        /// <summary>
        /// 处理并获取结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal bool GetResult(AbsYggdrasilContext context)
        {
            if (context.DB.Default is null)
            {
                return false;
            }

            var allIDS = GetAllID(SCID, context);

            try
            {
                _ = context.DB.Default.Delete<SystemConfig>()
                .Where(item => allIDS.Contains(item.SCID)).ExecuteAffrows();
            }
            catch
            {
                return false;
            }
            SystemConfigInfo.RefreshCache(context);
            return true;

        }

        private List<long> GetAllID(long targetID, AbsYggdrasilContext context)
        {
            var res = new List<long>();
            var allConfig = SystemConfigInfo.GetConfigTree(context);
            if (allConfig is null)
            {
                return res;
            }
            allConfig.ForEach(item =>
            {
                if (item.SCID == targetID)
                {
                    res.Add(item.SCID);
                    FindChild(res, item);
                }
                else
                {
                    FindChildWithTargetID(res, targetID, item);
                }
            });
            return res;
        }
        /// <summary>
        /// 在子目录查找目标
        /// </summary>
        /// <param name="res"></param>
        /// <param name="targetID"></param>
        /// <param name="info">当前节点</param>
        /// <returns></returns>
        private void FindChildWithTargetID(List<long> res, long targetID, SystemConfigInfo info)
        {
            if (info.Childs is not null && info.Childs.Count != 0)
            {
                info.Childs.ForEach(item =>
                {
                    if (item.SCID == targetID)
                    {
                        res.Add(item.SCID);
                        FindChild(res, item);
                    }
                    else
                    {
                        FindChildWithTargetID(res, targetID, item);
                    }
                });
            }
        }

        /// <summary>
        /// 遍历所有子
        /// </summary>
        /// <param name="target"></param>
        /// <param name="info">当前节点</param>
        /// <returns></returns>
        private void FindChild(List<long> target, SystemConfigInfo info)
        {
            if (info.Childs is not null && info.Childs.Count != 0)
            {
                info.Childs.ForEach(item =>
                {
                    target.Add(item.SCID);
                    FindChild(target, item);
                });
            }
        }

    }
}
