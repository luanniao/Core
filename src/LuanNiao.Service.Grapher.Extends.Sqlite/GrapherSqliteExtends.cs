using System;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    public class GrapherSqliteExtends
    {
        private static readonly object _lock = 1;
        public static void Init(string dbFilePath)
        {
            if (SqliteContext.Instance==null)
            {
                lock (_lock)
                {
                    if (SqliteContext.Instance==null)
                    {
                        SqliteContext.Instance = new SqliteContext(dbFilePath);
                        Grapher.DBWriter = SqliteContext.Instance;
                    }
                }
            }
        }
    }
}
