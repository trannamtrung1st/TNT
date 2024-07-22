using MediatR;

namespace TNT.Layers.Application.Models.Abstracts
{
    public interface ITransactionalCommand : IAutoSaveCommand, IRequest { }

    public interface ITransactionalCommand<T> : IAutoSaveCommand, IRequest<T> { }
}
