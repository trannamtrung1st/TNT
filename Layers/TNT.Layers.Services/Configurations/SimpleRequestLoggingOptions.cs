using System.Collections.Generic;
using Serilog.AspNetCore;

namespace TNT.Layers.Services.Configurations
{
    public class SimpleRequestLoggingOptions : RequestLoggingOptions
    {
        public const string DefaultConfigKey = "RequestLogging";
        public IDictionary<string, string> EnrichHeaders { get; set; } = new Dictionary<string, string>();

        public void CopyTo(RequestLoggingOptions options)
        {
            options.MessageTemplate = MessageTemplate;
            options.EnrichDiagnosticContext = EnrichDiagnosticContext;
            options.GetLevel = GetLevel;
            options.GetMessageTemplateProperties = GetMessageTemplateProperties;
            options.IncludeQueryInRequestPath = IncludeQueryInRequestPath;
            options.Logger = Logger;
        }
    }
}
