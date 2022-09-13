﻿namespace LuanNiao.Yggdrasil.Models.WebAPI
{
    /// <summary>
    /// 响应基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResponse<T>
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public ResponseCodeEnum Code { get; set; }
        /// <summary>
        /// 自定义消息
        /// </summary>
        public string? Msg { get; set; }
        /// <summary>
        /// 携带的数据
        /// </summary>
        public T? Data { get; set; }

    }
}