using System.Collections.Generic;
using System.Linq;

namespace LuanNiao.Yggdrasil.ConfigCenter.Operations
{
    /// <summary>
    /// 获取所有配置树信息请求
    /// </summary>
    public class GetAllConfigTreeDataRequest
    {

        internal GetAllConfigTreeDataResponse GetResult(AbsYggdrasilContext context)
        {

            var res = new GetAllConfigTreeDataResponse()
            {
                Key = -999,
                Title = "杰纳瑞",
                Type = -1,
                Value = "杰纳瑞"
            };
            var fullData = SystemConfigInfo.GetConfigTree(context);
            if (fullData is null)
            {
                return new GetAllConfigTreeDataResponse();
            }
            fullData.Where(item => item.ParentSCID == 0).ToList().ForEach(item =>
                {
                    var temp = new GetAllConfigTreeDataResponse()
                    {
                        Title = item.Name,
                        Type = item.Type,
                        Value = item.Value,
                        Key = item.SCID
                    };
                    res.Children.Add(temp);
                    ConstructData(item, temp);
                });

            return res;
        }

        private static void ConstructData(SystemConfigInfo node, GetAllConfigTreeDataResponse info)
        {
            node.Childs.ForEach(item =>
            {
                var temp = new GetAllConfigTreeDataResponse()
                {
                    Title = item.Name,
                    Type = item.Type,
                    Value = item.Value,
                    Key = item.SCID
                };
                info.Children.Add(temp);
                ConstructData(item, temp);
            });
        }


        /// <summary>
        /// 获取所有的配置树信息响应
        /// </summary>
        public class GetAllConfigTreeDataResponse
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; } = "";
            /// <summary>
            /// key值
            /// </summary>
            public long Key { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; } = "";
            /// <summary>
            /// 0,点位名称
            /// 1,配置节点
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 子
            /// </summary>
            public List<GetAllConfigTreeDataResponse> Children { get; set; } = new();
        }


    }
}
