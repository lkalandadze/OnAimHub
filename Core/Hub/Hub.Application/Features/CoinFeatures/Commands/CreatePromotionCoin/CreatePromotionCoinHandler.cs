using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.CreatePromotionCoin;

public class CreatePromotionCoinHandler : IRequestHandler<CreatePromotionCoin>
{
    private readonly IPromotionCoinRepository _promotionCoinRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionCoinHandler(IPromotionCoinRepository promotionCoinRepository, IWithdrawOptionRepository withdrawOptionRepository, IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _promotionCoinRepository = promotionCoinRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreatePromotionCoin request, CancellationToken cancellationToken)
    {
        //TODO: It is not decided whether we need it or not

        return Unit.Value;
    }
}