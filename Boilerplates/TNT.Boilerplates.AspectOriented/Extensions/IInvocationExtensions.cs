using Castle.DynamicProxy;
using System.Threading.Tasks;

namespace TNT.Boilerplates.AspectOriented.Extensions
{
    public static class IInvocationExtensions
    {
        public static T ProceedAsyncSync<T>(this IInvocation invocation)
        {
            invocation.Proceed();
            return invocation.ReturnValue is Task<T> task ? task.Result : default;
        }

        public static async Task<T> ProceedAsync<T>(this IInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is Task<T> task)
                return await task;
            return default;
        }

        public static void ProceedAsyncSync(this IInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is Task task)
                task.Wait();
        }

        public static async Task ProceedAsync(this IInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is Task task)
                await task;
        }
    }
}
