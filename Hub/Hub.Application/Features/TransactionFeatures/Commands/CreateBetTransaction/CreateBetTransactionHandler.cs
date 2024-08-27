using Hub.Application.Models.Tansaction;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public class CreateBetTransactionHandler : IRequestHandler<CreateBetTransaction, TransactionResponseModel>
{
    public readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public CreateBetTransactionHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IAuthService authService)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<TransactionResponseModel> Handle(CreateBetTransaction request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            GameVersionId = request.GameVersionId,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount,
            PlayerId = _authService.GetCurrentPlayerId(),
            StatusId = TransactionStatus.Created.Id,
            TypeId = TransactionType.Bet.Id,
        };

        await _transactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveAsync();

        return new TransactionResponseModel
        {
            Id = transaction.Id,
            Success = true,
        };
    }
}