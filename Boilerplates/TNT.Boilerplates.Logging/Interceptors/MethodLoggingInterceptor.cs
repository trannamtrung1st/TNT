using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using TNT.Boilerplates.Logging.Extensions;

namespace TNT.Boilerplates.Logging.Interceptors
{
    public class MethodLoggingInterceptor : IInterceptor
    {
        private readonly ILogger<MethodLoggingInterceptor> _logger;
        public MethodLoggingInterceptor(ILogger<MethodLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public virtual void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var methodDesc = method.GetDescription(invocation.Arguments);
            var targetType = invocation.TargetType.GetGenericTypeName();

            try
            {
                _logger.LogInformation("{targetType} --> {methodDescription}", targetType, methodDesc);
                invocation.Proceed();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{methodDescription}", methodDesc);
                throw;
            }
        }
    }
}
