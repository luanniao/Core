namespace LuanNiao.Yggdrasil.Models.WebAPI
{
    /// <summary>
    /// 分页输局返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataResponse<T> : DataResponse<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int DataCount { get; set; }
    }
}
