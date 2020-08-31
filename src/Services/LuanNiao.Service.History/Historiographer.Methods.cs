using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;

namespace LuanNiao.Service.History
{
    public sealed partial class Historiographer
    {

        [Event(EVENTID_TRACE,
          Opcode = EventOpcode.Info,
          Keywords = Keywords.EKW_TRACE,
          Level = EventLevel.Informational,
            Channel = EventChannel.Analytic)]
        public void Trace(string message)
        {
            if (IsEnabled(EventLevel.Verbose, Keywords.EKW_TRACE))
            {
                WriteEvent(EVENTID_TRACE, message);
            }
        }


    }
}
