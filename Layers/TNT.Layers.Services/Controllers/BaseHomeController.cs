using Asp.Versioning;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace TNT.Layers.Services.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseHomeController : BaseApiController
    {
        protected virtual string GetWelcomeMessage(
            ApiVersion version, IWebHostEnvironment env, string apiWelcomeMessageFormat)
        {
            StringBuilder finalMessage = new StringBuilder()
                .AppendFormat(apiWelcomeMessageFormat, env.EnvironmentName, version);

            if (!env.IsProduction())
                BuildNonProductionMessage(finalMessage);

            return finalMessage.ToString();
        }

        protected virtual void BuildNonProductionMessage(StringBuilder finalMessage)
        {
            finalMessage
                .AppendLine()
                .Append(Messages.Welcome.SwaggerInstruction);
        }
    }
}
