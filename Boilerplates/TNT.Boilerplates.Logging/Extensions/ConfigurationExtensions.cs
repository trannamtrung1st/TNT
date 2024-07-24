using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Settings.Configuration;
using System;

namespace TNT.Boilerplates.Logging.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Logger ParseLogger(
            this IConfiguration configuration,
            string sectionName, IServiceProvider provider = null)
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, readerOptions: new ConfigurationReaderOptions
                {
                    SectionName = sectionName
                });

            if (provider != null)
                loggerConfig = loggerConfig.ReadFrom.Services(provider);

            return loggerConfig.CreateLogger();
        }
    }
}
