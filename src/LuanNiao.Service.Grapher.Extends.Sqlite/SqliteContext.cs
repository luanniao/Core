using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    internal class SqliteContext : IDBGrapher
    {
        internal static SqliteContext Instance { get; set; }
        private readonly string _connectionStr = null; 

        internal SqliteContext(string filePath, string password = null)
        {
            _connectionStr = new SqliteConnectionStringBuilder($"Data Source={filePath}")
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            }.ToString();
            using (var db = new SqliteConnection(_connectionStr))
            {
                using (var com = db.CreateCommand())
                {
                    com.CommandText = (SqlQuery.CREATE_TABLE);
                    com.ExecuteNonQuery();
                }
            }
        }

        public void Write(int eventID, long tickets, string level, string[] keywords, string message, string op, string ActivityId, string relatedActivityId, string customPayLoad)
        {

            throw new NotImplementedException();
        }
    }
}
