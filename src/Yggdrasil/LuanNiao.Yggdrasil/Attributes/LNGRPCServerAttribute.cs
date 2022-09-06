using System;

namespace LuanNiao.Yggdrasil.Attributes
{
    /// <summary>
    /// 容器GRPC服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LNGRPCServerAttribute : Attribute
    {
    }
}
