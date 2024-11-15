using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;
using Hub.Domain.Absractions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;

public class UpdateWithdrawOptionHandler : IRequestHandler<UpdateWithdrawOption>
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IPromotionCoinRepository _promotionCoinRepository;
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWithdrawOptionHandler(IWithdrawOptionRepository withdrawOptionRepository, IPromotionCoinRepository promotionCoinRepository, ICoinTemplateRepository coinTemplateRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _promotionCoinRepository = promotionCoinRepository;
        _coinTemplateRepository = coinTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateWithdrawOption request, CancellationToken cancellationToken)
    {
        var option = await _withdrawOptionRepository.OfIdAsync(request.Id);

        if (option == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw option with the specified ID: [{request.Id}] was not found.");
        }

        var promotionCoins = (await _promotionCoinRepository.QueryAsync(pc => request.PromotionCoinIds.Any(pcId => pcId == pc.Id)))
                                                            .Where(c => c.CoinType == CoinType.Outgoing);

        var coinTemplates = (await _coinTemplateRepository.QueryAsync(ct => request.CoinTemplateIds.Any(ctId => ctId == ct.Id)))
                                                          .Where(c => c.CoinType == CoinType.Outgoing);

        option.Update(request.Title, request.Description, request.ImageUrl, request.Endpoint, request.EndpointContent, request.TemplateId, promotionCoins, coinTemplates);

        await _withdrawOptionRepository.InsertAsync(option);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}