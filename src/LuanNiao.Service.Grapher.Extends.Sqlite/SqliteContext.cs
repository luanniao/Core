using LuanNiao.Core;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    internal class SqliteContext : IDBGrapher
    {
        internal static SqliteContext Instance { get; set; }
        private readonly SqliteConnection _conn;
        internal SqliteContext(string filePath, string password = null)
        {
            var _connectionStr = new SqliteConnectionStringBuilder($"Data Source={filePath}")
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            }.ToString();
            _conn = new SqliteConnection(_connectionStr);
            _conn.Open();
            using (var com = _conn.CreateCommand())
            {
                com.CommandText = (SqlQuery.CREATE_TABLE);
                com.ExecuteNonQuery();
            }
        }
        ~SqliteContext()
        {
            _conn.Dispose();
        }

        public void Write(int eventID, long tickets, string level, string[] keywords, string message, string op, string activityId, string relatedActivityId, List<string> customPayLoad)
        {
            using (var com = _conn.CreateCommand())
            {
                com.CommandText = SqlQuery.INSERT_DATA;
                com.Parameters.AddRange(new SqliteParameter[] {
                     new SqliteParameter("@lid",IDGen.GetInstance().NextId()),
                     new SqliteParameter("@eventID",eventID),
                     new SqliteParameter("@tickets",tickets),
                     new SqliteParameter("@level",level),
                     new SqliteParameter("@keywords",string.Join(",",keywords)),
                     new SqliteParameter("@message",message??""),
                     new SqliteParameter("@op",op),
                     new SqliteParameter("@activityID",activityId),
                     new SqliteParameter("@relatedActivityID",relatedActivityId),
                     new SqliteParameter("@customPayload",string.Join(",",customPayLoad)),
                    });
                var res = com.ExecuteScalar();
                GrapherSqliteExtends.Written(
                    res == null ? -1 : Convert.ToInt32(res),
                    eventID,
                    tickets,
                    level,
                    keywords,
                    message,
                    op,
                    activityId,
                    relatedActivityId,
                    customPayLoad
                    );
            }
        }
    }
}
