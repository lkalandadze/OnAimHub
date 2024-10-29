using LevelService.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LevelService.Application.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly LevelDbContext _context;

    public TransactionBehaviour(
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger,
        LevelDbContext context)
    {
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);

        try
        {
            var strategy = _context.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"Begin transaction {typeof(TRequest).Name}");

                await _context.BeginTransactionAsync(cancellationToken);

                response = await next();

                await _context.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation($"Committed transaction {typeof(TRequest).Name}");
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}; \n {ex.ToString()}");

            await _context.RollbackTransaction();
            throw;
        }
    }
}