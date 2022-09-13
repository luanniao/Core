using System.Collections.Generic;

using LuanNiao.Yggdrasil.ConfigCenter.DBModels;

namespace LuanNiao.Yggdrasil.ConfigCenter.Operations
{
    /// <summary>
    /// 添加配置信息数据
    /// </summary>
    public class AddConfigDataRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = "";
        /// <summary>
        /// 0,点位名称
        /// 1,配置节点
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public long ParentID { get; set; }
        /// <summary>
        /// 子
        /// </summary>
        public List<AddConfigDataRequest> Children { get; set; } = new();

        /// <summary>
        /// 处理并获取结果
        /// </summary> 
        /// <returns></returns>
        internal bool GetResult(AbsYggdrasilContext context, long uid)
        {
            if (context.DB.Default is null)
            {
                return false;
            }
            if (ParentID != 0 && !context.DB.Default.Select<SystemConfig>().Any(item => item.SCID == ParentID))
            {
                return false;
            }
            else if (ParentID != 0 && context.DB.Default.Select<SystemConfig>().Any(item => item.ParentSCID == ParentID && item.Name == Name))
            {
                return false;
            }
            else if (ParentID == 0 && context.DB.Default.Select<SystemConfig>().Any(item => item.Name == Name && item.ParentSCID == 0))
            {
                return false;
            }
            List<SystemConfig> datas = new();
            var root = new SystemConfig()
            {
                Name = Name,
                ParentSCID = ParentID,
                Type = Type,
                Value = Value,
            };
            datas.Add(root);
            ConstructChild(datas, this, uid, root.SCID);
            var res = context.DB.Default.Insert(datas).ExecuteAffrows() != 0;
            if (res)
            {
                SystemConfigInfo.RefreshCache(context);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void ConstructChild(List<SystemConfig> target, AddConfigDataRequest data, long uid, long pid)
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
                        Value = item.Value,
                    };
                    target.Add(root);
                    ConstructChild(target, item, uid, root.SCID);
                });
            }
        }
    }
}
