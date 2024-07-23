using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TNT.Layers.Application.Models.Abstracts;
using TNT.Layers.Domain.Abstracts;
using System.Reflection;

namespace TNT.Layers.Persistence.Behaviors
{
    public abstract class PersistenceBehavior<TDbContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TDbContext : BaseDbContext
    {
        private readonly ILogger<PersistenceBehavior<TDbContext, TRequest, TResponse>> _logger;
        private readonly TDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public PersistenceBehavior(
            TDbContext dbContext, IUnitOfWork unitOfWork,
            ILogger<PersistenceBehavior<TDbContext, TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IAutoSaveCommand == false)
            {
                return await next();
            }

            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            if (request is ITransactionalCommand)
            {
                try
                {
                    if (_dbContext.HasActiveTransaction)
                    {
                        return await next();
                    }

                    var strategy = _dbContext.Database.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        using var transaction = await _dbContext.BeginTransactionAsync();
                        try
                        {
                            _logger.LogInformation("[START] Begin transaction {transactionId} for {commandName}", transaction.TransactionId, typeName);

                            response = await next();

                            await _unitOfWork.SaveEntitiesAsync();

                            _logger.LogInformation("[END] Commit transaction {transactionId} for {commandName}", transaction.TransactionId, typeName);

                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();

                            throw;
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ERROR] Handling transaction for {commandName} ({@command})", typeName, request);
                    throw;
                }
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
