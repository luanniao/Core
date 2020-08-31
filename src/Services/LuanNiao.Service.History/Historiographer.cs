using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace LuanNiao.Service.History
{
    [EventSource(Name = Common.Constants.LOGGER_EVENT_SOURCE_NAME)]
    public sealed partial class Historiographer : EventSource
    {
        private Historiographer() { }

        public static readonly Historiographer Instance = new Historiographer();

        public class EventID
        {
            public const int TRACE = 1;
            public const int DEBUG = 2;
            public const int INFO = 3;
            public const int WARNING = 4;
            public const int ERROR = 5;
            public const int CRITICAL = 6;
        }

        public class Keywords
        {
            public const EventKeywords LUANNIAO_HISTORY = (EventKeywords)1; 
        }
    }
}
