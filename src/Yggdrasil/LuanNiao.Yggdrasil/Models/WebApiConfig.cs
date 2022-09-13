namespace LuanNiao.Yggdrasil.Models
{
    /// <summary>
    /// WebApi配置信息
    /// </summary>
    public class WebApiConfig
    {
        /// <summary>
        /// 主机https端口
        /// </summary>
        public ushort HttpsPort { get; set; } = 5000;
        /// <summary>
        /// 主机http端口
        /// </summary>
        public ushort HttpPort { get; set; } = 5001;
        /// <summary>
        /// 主机httpv2端口
        /// </summary>
        public ushort HttpV2Port { get; set; } = 5002;
        /// <summary>
        /// 证书文件地址
        /// </summary>
        public string CertificateFile { get; set; } = string.Empty;
        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertificatePassword { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用API文档
        /// </summary>
        public bool UseSwagger { get; set; } = true;

        /// <summary>
        /// SwaggerURL前缀
        /// </summary>
        public string SwaggerUrlPrefix { get; set; } = string.Empty;

        /// <summary>
        /// api全局前缀
        /// </summary>
        public string ApiPrefix { get; set; } = string.Empty;
    }
}
