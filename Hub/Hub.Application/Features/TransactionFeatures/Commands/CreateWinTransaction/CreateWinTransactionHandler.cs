using Hub.Domain.Absractions.Repository;
using Hub.Domain.Absractions;
using MediatR;
using Hub.Domain.Entities;
using Hub.Application.Services.Abstract;
using Hub.Application.Models.Tansaction;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;

public class CreateWinTransactionHandler : IRequestHandler<CreateWinTransaction, TransactionResponseModel>
{
    public readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public CreateWinTransactionHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IAuthService authService)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<TransactionResponseModel> Handle(CreateWinTransaction request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            GameVersionId = request.GameVersionId,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount,
            PlayerId = _authService.GetCurrentPlayerId(),
            StatusId = TransactionStatus.Created.Id,
            TypeId = TransactionType.Win.Id,
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