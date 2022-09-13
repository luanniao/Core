﻿using System;
using System.Security.Cryptography;

namespace LuanNiao.Yggdrasil.Auth.Models
{
    /// <summary>
    /// 帮助函数
    /// </summary>
    public class ToolsMethod
    {
        /// <summary>
        /// 创建新的AES加密key与IV
        /// </summary>
        /// <returns></returns>
        public static (string key, string iv) CreateNewAesKeyAndVi()
        {
            var aes = Aes.Create();
            return (Convert.ToBase64String(aes.Key), Convert.ToBase64String(aes.IV));
        }

        /// <summary>
        /// 创建新的RSA密钥
        /// </summary>
        /// <returns></returns>
        public static string CreateNewRsaKey()
        {
            var rsa = new RSACryptoServiceProvider(512);
            return Convert.ToBase64String(rsa.ExportCspBlob(true));
        }
    }
}
