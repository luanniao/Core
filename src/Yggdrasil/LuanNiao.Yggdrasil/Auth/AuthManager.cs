using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using LuanNiao.Yggdrasil.Auth.Models;

using Microsoft.IdentityModel.Tokens;

namespace LuanNiao.Yggdrasil.Auth
{
    /// <summary>
    /// 权限中心管理
    /// </summary>
    public class AuthManager
    {
        private readonly byte[] _jwtKey;
        private readonly JwtSecurityTokenHandler _hander = new();
        private readonly TokenValidationParameters _validationParams;
        private readonly EncryptHelper _toolsMethod;
        private readonly SigningCredentials _signingCredentials;
        private readonly string _audience;
        private readonly string _issuer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtKey">密钥RSA512</param>
        /// <param name="issuer">颁发人</param>
        /// <param name="audience">听众</param>
        /// <param name="tdListAesKey">数据信息AES Key</param>
        /// <param name="tdListAesIV">数据信息AES VI</param>
        public AuthManager(string jwtKey, string issuer, string audience, string tdListAesKey, string tdListAesIV)
        {
            _audience = audience;
            _issuer = issuer;
            _jwtKey = Encoding.UTF8.GetBytes(jwtKey);
            _toolsMethod = new EncryptHelper(tdListAesKey, tdListAesIV);
            var key = new SymmetricSecurityKey(_jwtKey);
            _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            _validationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtKey),
                LifetimeValidator = new LifetimeValidator((dt1, dt2, t, p) =>
                {
                    if (dt1 is not null && dt1.Value.ToLocalTime() > DateTime.Now)
                    {
                        return false;
                    }
                    if (dt2 is null)
                    {
                        return true;
                    }

                    return dt2 is not null && dt2.Value.ToLocalTime() > DateTime.Now;

                })
            };
        }


        /// <summary>
        /// 解密获取某一个用户信息
        /// </summary>
        public bool TryGetUserInfo(string rawToken, out LNUserInfo? userInfo)
        {
            userInfo = null;
            try
            {
                var res = _hander.ValidateToken(rawToken, _validationParams, out var tokenInfo);
                if (tokenInfo is not JwtSecurityToken token)
                {
                    return false;
                }
                userInfo = new LNUserInfo();
                var rawTid = token.Claims.FirstOrDefault(item => item.Type == "TID");
                var rawUID = token.Claims.FirstOrDefault(item => item.Type == "UID");
                var rawTDInfo = token.Claims.FirstOrDefault(item => item.Type == "TDInfo");
                if (rawUID is null || !long.TryParse(rawUID.Value, out var uid))
                {
                    return false;
                }
                userInfo.UID = uid;
                if (rawTid is not null)
                {
                    if (!long.TryParse(rawTid.Value, out var tid))
                    {
                        return false;
                    }
                    if (rawTDInfo is null)
                    {
                        return false;
                    }
                    userInfo.CurrentTID = tid;
                    var dbInfoStr = _toolsMethod.DecryptStringFromStr_Aes(rawTDInfo.Value);
                    if (string.IsNullOrWhiteSpace(dbInfoStr))
                    {
                        return false;
                    }
                    var tdList = System.Text.Json.JsonSerializer.Deserialize<List<TDItem>>(dbInfoStr, JsonConverterExtends.CommonSerializerOptions.CamelCaseChineseNameCaseInsensitive); ;
                    if (tdList is null || tdList.Count == 0)
                    {
                        return false;
                    }
                    userInfo.TDInfo = tdList;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试创建一个Token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="token"></param>
        /// <param name="delayWorkMS"></param>
        /// <param name="expireMS"></param>
        /// <returns></returns>
        public bool TryCreateToken(LNUserInfo userInfo, out string? token, int delayWorkMS = 0, int expireMS = 1000 * 60 * 5)
        {
            token = String.Empty;
            try
            {
                var claims = new List<Claim>(4) { new Claim("UID", userInfo.UID.ToString()) };

                if (userInfo.CurrentTID != null)
                {
                    claims.Add(new Claim("TID", userInfo.CurrentTID.Value.ToString()));
                    if (userInfo.TDInfo is null || userInfo.TDInfo.Count == 0)
                    {
                        return false;
                    }

                    var dbInfoStr = System.Text.Json.JsonSerializer.Serialize(userInfo.TDInfo, JsonConverterExtends.CommonSerializerOptions.CamelCaseChineseNameCaseInsensitive);
                    var dbInfoEStr = _toolsMethod.EncryptStringToBytes_Aes(dbInfoStr);
                    if (string.IsNullOrWhiteSpace(dbInfoEStr))
                    {
                        return false;
                    }
                    claims.Add(new Claim("TDInfo", dbInfoEStr));
                }


                var jwtToken = new JwtSecurityToken(issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    notBefore: DateTime.Now.AddMilliseconds(delayWorkMS),
                    expires: DateTime.Now.AddMilliseconds(delayWorkMS).AddMilliseconds(expireMS),
                    signingCredentials: _signingCredentials);
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
