using System.Diagnostics.Tracing;

namespace LuanNiao.Service.History
{
    public sealed partial class Historiographer
    {
        [Event(EVENTID_TRACE,
          Message = "We go the new tunnel {1} from the channel {0}",
          Opcode = EventOpcode.Info,
          Keywords = EKW_TRACE,
          Level = EventLevel.Informational)]
        public void Trace(params object[] args)
        {
            if (IsEnabled(EventLevel.Verbose, EKW_TRACE))
            {
                WriteEvent(EVENTID_TRACE, args);
            }
        } 
    }
}
