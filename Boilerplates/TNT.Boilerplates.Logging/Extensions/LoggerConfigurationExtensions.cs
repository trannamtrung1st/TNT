using Serilog.Configuration;
using Serilog.Events;
using System;

namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public const string HostLevelLogFile = "logs/host/host.txt";
        public const string HostLevelLogTemplate = "[{UtcTimestamp} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration WithUtcTimestamp(this LoggerEnrichmentConfiguration enrich)
        {
            ArgumentNullException.ThrowIfNull(enrich);

            return enrich.With<UtcTimestampEnricher>();
        }

        public static LoggerConfiguration SimpleHostLevelLog(this LoggerSinkConfiguration writeTo, bool isProduction)
        {
            var template = HostLevelLogTemplate;

            if (!isProduction)
                return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);

            return writeTo.File(HostLevelLogFile,
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
        }
    }
}
