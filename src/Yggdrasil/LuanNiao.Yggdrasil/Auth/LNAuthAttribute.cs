
using System;

using Microsoft.AspNetCore.Mvc;

namespace LuanNiao.Yggdrasil.Auth
{
    /// <summary>
    /// 权限过滤特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LNAuthAttribute : TypeFilterAttribute
    {
        private static Type? _filter = null;

        /// <summary>
        /// 初始化权限模块
        /// </summary>
        /// <param name="filterType"></param>
        public static void Init(Type filterType)
        {
            _filter = filterType;
        }
        /// <summary>
        /// 权限过滤特性
        /// </summary>
        /// <param name="roles">允许的角色</param>
        /// <param name="allowOffline">是否允许离线,如果允许,如果权限中心为单点登录,那么将会造成脱离单点控制,当然手动可以认证</param>
        /// <param name="neesdTenantID">必须拥有商户ID</param>
        public LNAuthAttribute(bool allowOffline = false, bool neesdTenantID = false, params string[] roles) : base(_filter)
        {
            Arguments = new object[] { roles, allowOffline, neesdTenantID };
        }
    }
}
