using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    public static class SqlQuery
    {
        public const string CREATE_TABLE = @"
        CREATE TABLE ln_grapher_log(
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
        CREATE INDEX [IDX_tickets] ON [log]([tickets] ASC);
        CREATE INDEX [IDX_op] ON [log]([op]);
        CREATE INDEX [IDX_level] ON [log]([level]);
        CREATE INDEX [IDX_eventID] ON [log]([eventID]);
        CREATE INDEX [IDX_keywords] ON [log]([keywords]);
        ";

        public const string INSERT_DATA = @"
            insert into ln_grapher_log values(@lid,@eventID,@tickets,@level,@keywords,@message,@op,@activityID,@relatedActivityID,@customPayload);
                SELECT LAST_INSERT_ROWID() FROM ln_grapher_log
        ";
    }
}
