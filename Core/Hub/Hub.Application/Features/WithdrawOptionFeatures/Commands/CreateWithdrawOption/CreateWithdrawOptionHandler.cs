using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

public class CreateWithdrawOptionHandler : IRequestHandler<CreateWithdrawOption>
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IPromotionCoinRepository _promotionCoinRepository;
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWithdrawOptionHandler(IWithdrawOptionRepository withdrawOptionRepository, IPromotionCoinRepository promotionCoinRepository, ICoinTemplateRepository coinTemplateRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _promotionCoinRepository = promotionCoinRepository;
        _coinTemplateRepository = coinTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateWithdrawOption request, CancellationToken cancellationToken)
    {
        // Create withdraw option
        var option = new WithdrawOption(request.Title, request.Description, request.ImageUrl, request.ContentType, request.Endpoint, request.EndpointContent, request.TemplateId);

        // Add promotion coins if exists
        if (request.PromotionCoinIds != null && request.PromotionCoinIds.Any())
        {
            var promotionCoins = (await _promotionCoinRepository.QueryAsync(pc => request.PromotionCoinIds.Any(pcId => pcId == pc.Id)))
                                                                .Where(c => c.CoinType == CoinType.Outgoing);

            //option.AddPromotionCoins(promotionCoins);
        }

        //Add coin templates if exists
        if (request.CoinTemplateIds != null && request.CoinTemplateIds.Any())
        {
            var coinTemplates = (await _coinTemplateRepository.QueryAsync(ct => request.CoinTemplateIds.Any(ctId => ctId == ct.Id)))
                                                              .Where(c => c.CoinType == CoinType.Outgoing);

            option.AddCoinTemplates(coinTemplates);
        }

        // Save to database
        await _withdrawOptionRepository.InsertAsync(option);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}