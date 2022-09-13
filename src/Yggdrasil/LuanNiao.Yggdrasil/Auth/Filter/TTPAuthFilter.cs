using System;
using System.Linq;

using LuanNiao.Yggdrasil.Auth.Results;
using LuanNiao.Yggdrasil.Logger.AbsInfos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LuanNiao.Yggdrasil.Auth.Filter
{
    /// <summary>
    /// 容器权限过滤器
    /// </summary>
    public class TTPAuthFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly AbsLNLogger<TTPAuthFilter> _logger;

        private readonly bool _neesdTenantID;
        private readonly AbsYggdrasilContext _context;
        /// <summary>
        /// 容器权限过滤器
        /// </summary> 
        /// <param name="context">上下文</param>
        public TTPAuthFilter(bool neesdTenantID, AbsYggdrasilContext context, AbsLNLogger<TTPAuthFilter> logger)
        {
            _logger = logger;
            _context = context;
            _neesdTenantID = neesdTenantID;
        }
        /// <summary>
        /// 验证发生时触发的函数
        /// </summary>
        /// <param name="filterContext"></param> 
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                if (filterContext == null || filterContext.Filters.Any(item => item is IAllowAnonymous))
                {
                    return;
                }

                if (!filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var rawToken)
                    || rawToken.Count != 1
                    || !rawToken[0].StartsWith("Bearer ")
                    )
                {
                    filterContext.Result = new ForbidResult(401, "{\"msg\":\"token was empty. \"}");
                    return;
                }
                var token = rawToken[0].Replace("Bearer ", "");

                if (!_context.Auth.TryGetUserInfo(token, out var userInfo) || userInfo is null)
                {
                    filterContext.Result = new ForbidResult(401, "{\"msg\":\"token was illegal. \"}");
                    return;
                }

                if (_neesdTenantID && userInfo.CurrentTID is null)
                {
                    filterContext.Result = new ForbidResult(403, "{\"msg\":\"please select tenant first!\"}");
                    return;
                }
                return;
            }
            catch (Exception ex)
            {
                _logger?.Information($"User check faild.", exception: ex);
            }
            filterContext.Result = new ForbidResult(500, "{\"msg\":\"Unexpected request!\"}");
        }
    }
}
