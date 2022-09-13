using System.Collections.Generic;

using LuanNiao.Yggdrasil.Auth.Models;

namespace LuanNiao.Yggdrasil.Auth
{
    /// <summary>
    /// 容器用户信息
    /// </summary>
    public class LNUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UID { get; set; }

        /// <summary>
        /// 当前正在使用的商户ID
        /// </summary>
        public long? CurrentTID { get; set; }

        /// <summary>
        /// 当前商户的数据库信息
        /// </summary>
        public List<TDItem> TDInfo { get; set; } = new();

    }
}
