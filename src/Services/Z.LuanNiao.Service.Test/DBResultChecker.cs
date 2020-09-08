using LuanNiao.Service.Grapher.Extends.Sqlite;
using System.IO;
using System.Text;
using System.Threading;
using Xunit.Abstractions;

namespace Z.LuanNiao.Service.Test
{

    public class DBResultChecker 
    { 
        private string _data = null;
        public string OutPutInfo = ""; 
        private readonly AutoResetEvent _waitter = new AutoResetEvent(false);

        public DBResultChecker( )
        {
            GrapherSqliteExtends.OnWritten += GrapherSqliteExtends_OnWritten;
        }

        private void GrapherSqliteExtends_OnWritten(int keyID, int eventID, long tickets, string level, string[] keywords, string message, string op, string activityId, string relatedActivityId, string customPayLoad)
        {
            _data = customPayLoad;
        }

        ~DBResultChecker()
        {
            GrapherSqliteExtends.OnWritten -= GrapherSqliteExtends_OnWritten;
        }

        public void Wait()
        {
            _waitter.WaitOne();
        }
       
        public bool Result = false;

        public void Check(string value)
        {
            Result = _data.Contains(value);
        }
    }
}
