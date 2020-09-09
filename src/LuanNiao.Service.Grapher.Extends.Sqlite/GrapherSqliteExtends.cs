using System;
using System.Collections.Generic;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    public class GrapherSqliteExtends
    {
        private static readonly object _lock = 1;
        public static void Init(string dbFilePath)
        {
            if (SqliteContext.Instance == null)
            {
                lock (_lock)
                {
                    if (SqliteContext.Instance == null)
                    {
                        SqliteContext.Instance = new SqliteContext(dbFilePath);
                        Grapher.DBWriter = SqliteContext.Instance;
                    }
                }
            }
        }
        public static event Action<int, int, long, string, string[], string, string, string, string, List<string>> OnWritten;
        internal static void Written(int keyID, int eventID, long tickets, string level, string[] keywords, string message, string op, string activityId, string relatedActivityId, List<string> customPayLoad)
        {
            OnWritten?.Invoke(keyID, eventID, tickets, level, keywords, message, op, activityId, relatedActivityId, customPayLoad);
        }
    }
}
