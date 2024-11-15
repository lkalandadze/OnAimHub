using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignPromotionCoinsToWithdrawOption;

public class AssignPromotionCoinsToWithdrawOptionHandler : IRequestHandler<AssignPromotionCoinsToWithdrawOption>
{
    private readonly IPromotionCoinWithdrawOptionRepository _promotionCoinWithdrawOptionRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IPromotionCoinRepository _promotionCoinRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignPromotionCoinsToWithdrawOptionHandler(IPromotionCoinWithdrawOptionRepository promotionCoinWithdrawOptionRepository, IWithdrawOptionRepository withdrawOptionRepository, IPromotionCoinRepository promotionCoinRepository, IUnitOfWork unitOfWork)
    {
        _promotionCoinWithdrawOptionRepository = promotionCoinWithdrawOptionRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _promotionCoinRepository = promotionCoinRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignPromotionCoinsToWithdrawOption request, CancellationToken cancellationToken)
    {
        var option = await _withdrawOptionRepository.OfIdAsync(request.WithdrawOptionId);

        if (option == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw option with the specified ID: [{request.WithdrawOptionId}] was not found.");
        }

        var promotionCoins = (await _promotionCoinRepository.QueryAsync(pc => request.PromotionCoinIds.Contains(pc.Id)))
                                                            .Where(c => c.CoinType == CoinType.Outgoing);

        if (promotionCoins == null || !promotionCoins.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No promotion coins were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing coin templates."
            );
        }

        if (promotionCoins == null || promotionCoins.Any(c => c.CoinType != CoinType.Outgoing))
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "One or more promotionCoins are not eligible for this operation. Only outgoing promotionCoins are allowed.");
        }

        foreach (var coin in promotionCoins)
        {
            var coinWithdrawOption = new PromotionCoinWithdrawOption(coin.Id, request.WithdrawOptionId);

            await _promotionCoinWithdrawOptionRepository.InsertAsync(coinWithdrawOption);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}