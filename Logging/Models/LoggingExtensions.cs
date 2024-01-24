using Serilog;
using Serilog.Configuration;
using System;

namespace Logging.Models
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithContextEnricher(this LoggerEnrichmentConfiguration enrich)
        {
            return enrich != null ? enrich.With<SerilogContextEnricher>() : throw new ArgumentNullException(nameof(enrich));
        }
    }
}
