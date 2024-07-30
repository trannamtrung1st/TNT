using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TNT.Layers.Application.Models.Abstracts;
using TNT.Layers.Domain.Abstracts;
using System.Reflection;
using TNT.Layers.Persistence.Abstracts;
using System.Diagnostics;

namespace TNT.Layers.Persistence.Behaviors
{
    [DebuggerStepThrough]
    public abstract class BasePersistenceBehavior<TDbContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TDbContext : DbContext, IDbContext
    {
        private readonly TDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BasePersistenceBehavior(TDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IAutoSaveCommand == false)
                return await next();

            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            if (request is ITransactionalCommand)
            {
                if (_dbContext.HasActiveTransaction)
                    return await next();

                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _dbContext.BeginTransactionAsync();
                    try
                    {
                        response = await next();
                        await _unitOfWork.SaveEntitiesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            else
            {
                response = await next();
                await _unitOfWork.SaveEntitiesAsync(cancellationToken: cancellationToken);
            }

            return response;
        }
    }
}
