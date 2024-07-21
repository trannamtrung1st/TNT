using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TNT.Boilerplates.Common.Mediator
{
    public class NullMediator : IMediator
    {
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            await Task.CompletedTask;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return default;
        }

        public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            await Task.CompletedTask;
        }

        public async Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return default;
        }
    }
}
