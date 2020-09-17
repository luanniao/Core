using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Service.Grapher
{
    /// <summary>
    /// Grapher version of db's operation.
    /// </summary>
    public interface IDBGrapher : IDisposable
    {
        void Write(int eventID,
            long tickets,
            string level,
            string[] keywords,
            string message,
            string op,
            string activityId,
            string relatedActivityId,
            List<string> customPayLoad);
    }
}
