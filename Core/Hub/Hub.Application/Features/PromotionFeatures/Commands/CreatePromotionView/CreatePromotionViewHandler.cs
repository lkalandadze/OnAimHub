using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

public class CreatePromotionViewHandler : IRequestHandler<CreatePromotionView, Response<CreatePromotionViewResponse>>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IPromotionViewRepository _promotionViewRepository;
    private readonly IPromotionViewService _promotionViewService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionViewHandler(
        IPromotionRepository promotionRepository, 
        IPromotionViewRepository promotionViewRepository,
        IPromotionViewService promotionViewService,
        IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _promotionViewRepository = promotionViewRepository;
        _promotionViewService = promotionViewService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<CreatePromotionViewResponse>> Handle(CreatePromotionView request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var promotion = await _promotionRepository.OfIdAsync(request.PromotionId);

        if (promotion == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Promotion with the specified ID: [{request.PromotionId}] was not found.");
        }

        var viewUrl = _promotionViewService.GenerateViewUrl(request.ViewContent, promotion.Id);

        var promotionView = new PromotionView(request.Name, viewUrl, request.PromotionId, request.TemplateId);

        await _promotionViewRepository.InsertAsync(promotionView);

        await _unitOfWork.SaveAsync();

        var response = new CreatePromotionViewResponse(viewUrl);

        return new Response<CreatePromotionViewResponse>(response);
    }
}