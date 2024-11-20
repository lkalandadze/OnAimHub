using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.Create;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISegmentRepository _segmentRepository;
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IWithdrawEndpointTemplateRepository _withdrawEndpointTemplateRepository;
    private readonly IPromotionRepository _promotionRepository;

    public CreatePromotionCommandHandler(IUnitOfWork unitOfWork, ISegmentRepository segmentRepository, ICoinTemplateRepository coinTemplateRepository, IWithdrawEndpointTemplateRepository withdrawEndpointTemplateRepository, IPromotionRepository promotionRepository)
    {
        _unitOfWork = unitOfWork;
        _segmentRepository = segmentRepository;
        _coinTemplateRepository = coinTemplateRepository;
        _withdrawEndpointTemplateRepository = withdrawEndpointTemplateRepository;
        _promotionRepository = promotionRepository;
    }

    public async Task<Unit> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
            throw new Exception("EndDate must be later than StartDate.");

        var segments = _segmentRepository.Query()
            .Where(s => request.SegmentIds.Contains(s.Id))
            .ToList();

        if (segments.Count != request.SegmentIds.Count())
            throw new Exception("One or more Segment IDs are invalid.");

        var promotion = new Promotion(
            request.Status,
            request.StartDate,
            request.EndDate,
            request.Title,
            request.Description
        )
        {
            Segments = segments,
            Coins = new List<PromotionCoin>()
        };

        foreach (var promotionCoinModel in request.PromotionCoin)
        {
            if (promotionCoinModel.IsTemplate)
            {
                var coinTemplate = new CoinTemplate
                {
                    Name = promotionCoinModel.Name,
                    ImageUrl = promotionCoinModel.ImageUrl,
                    CoinType = promotionCoinModel.CoinType
                };

                await _coinTemplateRepository.InsertAsync(coinTemplate);
            }

            var promotionCoin = new PromotionCoin
            {
                Id = promotion.Id + "_" + promotionCoinModel.Name,
                Name = promotionCoinModel.Name,
                ImageUrl = promotionCoinModel.ImageUrl,
                CoinType = promotionCoinModel.CoinType,
                PromotionId = promotion.Id,
                WithdrawOptionGroups = new List<WithdrawOptionGroup>()
            };

            foreach (var groupModel in promotionCoinModel.WithdrawOptionGroups)
            {
                var withdrawOptionGroup = new WithdrawOptionGroup
                {
                    Title = groupModel.Title,
                    Description = groupModel.Description,
                    ImageUrl = groupModel.ImageUrl,
                    WithdrawOptions = new List<WithdrawOption>()
                };

                foreach (var optionModel in groupModel.WithdrawOptions)
                {
                    var withdrawOption = new WithdrawOption
                    {
                        Title = optionModel.Title,
                        Description = optionModel.Description,
                        ImageUrl = optionModel.ImageUrl,
                        Endpoint = optionModel.Endpoint,
                        ContentType = optionModel.ContentType,
                        EndpointContent = optionModel.EndpointContent
                    };

                    if (optionModel.IsTemplate)
                    {
                        var endpointTemplate = new WithdrawEndpointTemplate
                        {
                            Name = optionModel.Title,
                            Endpoint = optionModel.Endpoint,
                            EndpointContent = optionModel.EndpointContent,
                            ContentType = optionModel.ContentType,
                        };

                        await _withdrawEndpointTemplateRepository.InsertAsync(endpointTemplate);
                    }

                    withdrawOptionGroup.WithdrawOptions.Add(withdrawOption);
                }

                promotionCoin.WithdrawOptionGroups.Add(withdrawOptionGroup);
            }

            promotion.Coins.Add(promotionCoin);
        }

        await _promotionRepository.InsertAsync(promotion);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}