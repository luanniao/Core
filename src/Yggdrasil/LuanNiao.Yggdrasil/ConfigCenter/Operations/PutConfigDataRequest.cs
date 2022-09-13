using System.Collections.Generic;

using LuanNiao.Yggdrasil.ConfigCenter.DBModels;

namespace LuanNiao.Yggdrasil.ConfigCenter.Operations
{
    /// <summary>
    /// 删除一个配置信息
    /// </summary>
    public class PutConfigDataRequest
    {
        /// <summary>
        /// 配置主键ID
        /// </summary> 
        public long SCID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 子
        /// </summary>
        public List<PutConfigDataRequest> Children { get; set; } = new();

        /// <summary>
        /// 处理并获取结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        internal bool GetResult(AbsYggdrasilContext context, long uid)
        {
            if (context.DB.Default is null)
            {
                return false;
            }
            var res = context.DB.Default.Select<SystemConfig>().Where(item => item.SCID == SCID).First();
            if (res == null)
            {
                return false;
            }
            if (res.ParentSCID == 0 && context.DB.Default.Select<SystemConfig>().Any(item => item.Name == Name && item.ParentSCID == 0 && item.SCID != SCID))
            {
                return false;
            }
            else if (res.ParentSCID != 0 && context.DB.Default.Select<SystemConfig>().Any(item => item.Name == Name && item.ParentSCID == res.ParentSCID && item.SCID != SCID))
            {
                return false;
            }
            var allIDS = GetAllID(SCID, context);

            try
            {

                List<SystemConfig> datas = new();
                ConstructChild(datas, this, uid, SCID);
                context.DB.Default.Transaction(() =>
                {

                    _ = context.DB.Default.Delete<SystemConfig>()
                    .Where(item => allIDS.Contains(item.SCID)).ExecuteAffrows();
                    _ = context.DB.Default.Insert(datas).ExecuteAffrows();
                    _ = context.DB.Default.Update<SystemConfig>()
                   .Where(item => item.SCID == SCID)
                   .Set(item => item.Name, Name)
                   .Set(item => item.Name, Name)
                   .Set(item => item.Type, Type)
                   .Set(item => item.Value, Value)
                   .ExecuteAffrows();
                });


                SystemConfigInfo.RefreshCache(context);
                return true;
            }
            catch
            {
                return false;
            }

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


        private void ConstructChild(List<SystemConfig> target, PutConfigDataRequest data, long uid, long pid)
        {
            if (data.Children != null && data.Children.Count != 0)
            {
                data.Children.ForEach(item =>
                {
                    var root = new SystemConfig()
                    {
                        Name = item.Name,
                        ParentSCID = pid,
                        Type = item.Type,
                        Value = item.Value
                    };
                    target.Add(root);
                    ConstructChild(target, item, uid, root.SCID);
                });
            }
        }

    }
}
