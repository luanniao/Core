using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;

namespace LuanNiao.Service.History
{
    public sealed partial class Historiographer
    {
        [Event(EventID.TRACE,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.LogAlways
            )]
        public void Trace(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.LogAlways, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.TRACE, relatedActivityId, message);
            }
        }

        [Event(EventID.DEBUG,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.Verbose
            )]
        public void Debug(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Verbose, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.DEBUG, relatedActivityId, message);
            }
        }

        [Event(EventID.INFO,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.Informational
            )]
        public void Info(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Informational, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.INFO, relatedActivityId, message);
            }
        }

        [Event(EventID.WARNING,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.Warning
            )]
        public void Warning(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Warning, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.WARNING, relatedActivityId, message);
            }
        }

        [Event(EventID.ERROR,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.Error
            )]
        public void Error(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Error, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.ERROR, relatedActivityId, message);
            }
        }

        [Event(EventID.CRITICAL,
            Opcode = EventOpcode.Info,
            Keywords = Keywords.LUANNIAO_HISTORY,
            Level = EventLevel.Critical
            )]
        public void Critical(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Critical, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.CRITICAL, relatedActivityId, message);
            }
        }

    }
}
