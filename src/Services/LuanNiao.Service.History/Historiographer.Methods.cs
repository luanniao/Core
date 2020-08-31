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
          Level = EventLevel.Informational,
            Channel = EventChannel.Analytic)]
        public void Trace(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Verbose, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.TRACE, relatedActivityId, message);
            }
        }

        [Event(EventID.DEBUG,
        Opcode = EventOpcode.Info,
        Keywords = Keywords.LUANNIAO_HISTORY,
        Level = EventLevel.Informational,
          Channel = EventChannel.Analytic)]
        public void Debug(Guid relatedActivityId, string message)
        {
            if (IsEnabled(EventLevel.Verbose, Keywords.LUANNIAO_HISTORY))
            {
                WriteEventWithRelatedActivityId(EventID.DEBUG, relatedActivityId, message);
            }
        }

    }
}
