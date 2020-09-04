using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Service.Grapher.Extends.Sqlite
{
    public static class SqlQuery
    {
        public const string CREATE_TABLE = @" CREATE TABLE IF NOT EXISTS USER
        (NAME TEXT,
        AGE INT,
        SALARY REAL); ";
    }
}
