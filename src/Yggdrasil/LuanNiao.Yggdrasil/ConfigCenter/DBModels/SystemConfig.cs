using FreeSql.DataAnnotations;

namespace LuanNiao.Yggdrasil.ConfigCenter.DBModels
{
    /// <summary>
    /// 配置信息
    /// </summary>
    [Table(Name = "SystemConfigs")]
    public class SystemConfig
    {
        /// <summary>
        /// 配置主键ID
        /// </summary>
        [Column(IsPrimary = true)]
        public long SCID { get; set; } = Core.IDGen.Instance.NextId();
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
        /// 父级配置ID
        /// </summary>
        public long ParentSCID { get; set; }
    }
}
