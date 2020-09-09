using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    public static class SqlQuery
    {
        public const string CREATE_TABLE = @"
        CREATE TABLE IF NOT EXISTS ln_grapher_log(
          [lid] INT PRIMARY KEY ASC UNIQUE, 
          [eventID] INT, 
          [tickets] INT64, 
          [level] NVARCHAR, 
          [keywords] NVARCHAR, 
          [message] NVARCHAR, 
          [op] NVARCHAR, 
          [activityID] NVARCHAR, 
          [relatedActivityID] NVARCHAR, 
          [customPayload] TEXT);
        CREATE INDEX IF NOT EXISTS [IDX_tickets] ON [ln_grapher_log]([tickets] ASC);
        CREATE INDEX IF NOT EXISTS [IDX_op] ON [ln_grapher_log]([op]);
        CREATE INDEX IF NOT EXISTS [IDX_level] ON [ln_grapher_log]([level]);
        CREATE INDEX IF NOT EXISTS [IDX_eventID] ON [ln_grapher_log]([eventID]);
        CREATE INDEX IF NOT EXISTS [IDX_keywords] ON [ln_grapher_log]([keywords]);
        ";

        public const string INSERT_DATA = @"
            insert into ln_grapher_log values(@lid,@eventID,@tickets,@level,@keywords,@message,@op,@activityID,@relatedActivityID,@customPayload);
                SELECT LAST_INSERT_ROWID() FROM ln_grapher_log
        ";
    }
}
