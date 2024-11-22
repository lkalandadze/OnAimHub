using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.Templates;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

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
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"EndDate must be later than StartDate.");
        }

        var segments = await _segmentRepository.QueryAsync(s => request.SegmentIds.Any(sId => sId == s.Id));

        if (segments.Count != request.SegmentIds.Count())
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "One or more Segment IDs are invalid.");
        }

        var promotion = new Promotion(
            request.StartDate,
            request.EndDate,
            request.Title,
            request.Description,
            segments: segments
        );

        await _promotionRepository.InsertAsync(promotion);
        await _unitOfWork.SaveAsync();

        foreach (var promotionCoinModel in request.PromotionCoin)
        {
            //if (promotionCoinModel.IsTemplate)
            //{
            //    var coinTemplate = new CoinTemplate(
            //        promotionCoinModel.Name,
            //        promotionCoinModel.ImageUrl,
            //        promotionCoinModel.CoinType);

            //    await _coinTemplateRepository.InsertAsync(coinTemplate);
            //}

            var promotionCoinId = $"{promotion.Id}_{promotionCoinModel.Name}";

            var coinTemplate = promotionCoinModel.IsTemplate
                    ? new CoinTemplate(
                        promotionCoinModel.Name,
                        promotionCoinModel.Description,
                        promotionCoinModel.ImageUrl,
                        promotionCoinModel.CoinType)
                    : null;

            await _coinTemplateRepository.InsertAsync(coinTemplate);

            var promotionCoin = new PromotionCoin(
                promotionCoinId,
                promotionCoinModel.Name,
                promotionCoinModel.Description,
                promotionCoinModel.ImageUrl,
                promotionCoinModel.CoinType,
                promotion.Id);

            foreach (var groupModel in promotionCoinModel.WithdrawOptionGroups)
            {
                var withdrawOptionGroup = new WithdrawOptionGroup(
                    groupModel.Title,
                    groupModel.Description,
                    groupModel.ImageUrl);

                foreach (var optionModel in groupModel.WithdrawOptions)
                {
                    var withdrawOption = new WithdrawOption(
                        optionModel.Title,
                        optionModel.Description,
                        optionModel.ImageUrl,
                        optionModel.ContentType,
                        optionModel.Endpoint,
                        optionModel.EndpointContent)
                    {
                        WithdrawOptionGroups = [withdrawOptionGroup],
                        CoinTemplates = [coinTemplate]
                    };

                    if (optionModel.IsTemplate)
                    {
                        var endpointTemplate = new WithdrawEndpointTemplate(
                            optionModel.Title,
                            optionModel.Endpoint,
                            optionModel.EndpointContent,
                            optionModel.ContentType);

                        await _withdrawEndpointTemplateRepository.InsertAsync(endpointTemplate);
                    }

                    withdrawOptionGroup.WithdrawOptions.Add(withdrawOption);
                    promotionCoin.AddWithdrawOption([withdrawOption]);
                }
                //promotionCoin.AddWithdrawOption([withdrawOption]);
            }

            promotion.Coins.Add(promotionCoin);
        }

        _promotionRepository.Update(promotion);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}