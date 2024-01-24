using Serilog.Core;
using Serilog.Events;

namespace Logging.Models
{
    public class SerilogContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.RemovePropertyIfPresent("ActionId");
            logEvent.RemovePropertyIfPresent("RequestId");
            logEvent.RemovePropertyIfPresent("CorrelationId");
            logEvent.RemovePropertyIfPresent("ConnectionId");
        }
    }
}
