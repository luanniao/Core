using System;

namespace LuanNiao.Yggdrasil.Attributes
{
    /// <summary>
    /// 容器API服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LNAPIServerAttribute : Attribute
    {
    }
}
