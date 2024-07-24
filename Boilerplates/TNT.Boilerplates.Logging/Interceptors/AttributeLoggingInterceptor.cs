using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System.Linq;
using TNT.Boilerplates.Logging.Attributes;

namespace TNT.Boilerplates.Logging.Interceptors
{
    public class AttributeLoggingInterceptor : MethodLoggingInterceptor
    {
        public AttributeLoggingInterceptor(ILogger<MethodLoggingInterceptor> logger) : base(logger)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var logAttribute = method
                .GetCustomAttributes(typeof(LogAttribute), true)
                .FirstOrDefault() as LogAttribute;

            logAttribute = logAttribute ?? method.DeclaringType
                .GetCustomAttributes(typeof(LogAttribute), true)
                .FirstOrDefault() as LogAttribute;

            var shouldLog = logAttribute != null
                && !logAttribute.Disabled
                && (!logAttribute.PublicOnly || method.IsPublic);

            if (shouldLog)
            {
                base.Intercept(invocation);
                return;
            }

            invocation.Proceed();
        }
    }
}
