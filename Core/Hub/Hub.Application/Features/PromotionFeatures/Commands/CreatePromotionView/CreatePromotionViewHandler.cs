using Hub.Application.Configurations;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.Templates;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

public class CreatePromotionViewHandler : IRequestHandler<CreatePromotionView, Response<CreatePromotionViewResponse>>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IPromotionViewRepository _promotionViewRepository;
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;
    private readonly IPromotionViewService _promotionViewService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionViewHandler(
        IPromotionRepository promotionRepository, 
        IPromotionViewRepository promotionViewRepository,
        IPromotionViewTemplateRepository promotionViewTemplateRepository,
        IPromotionViewService promotionViewService,
        IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _promotionViewRepository = promotionViewRepository;
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _promotionViewService = promotionViewService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<CreatePromotionViewResponse>> Handle(CreatePromotionView request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.OfIdAsync(request.PromotionId);

        if (promotion == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Promotion with the specified ID: [{request.PromotionId}] was not found.");
        }

        var viewUrl = _promotionViewService.GenerateViewUrl(request.ViewContent, promotion.Id);

        var promotionView = new PromotionView(request.Name, viewUrl, request.PromotionId, request.TemplateId);

        await _promotionViewRepository.InsertAsync(promotionView);

        if (request.SaveAsTemplate != null && request.SaveAsTemplate.Value)
        {
            var templateViewUrl = _promotionViewService.GenerateTemplateViewUrl(request.ViewContent);

            var promotionViewTemplate = new PromotionViewTemplate(request.Name, templateViewUrl, [promotionView]);

            await _promotionViewTemplateRepository.InsertAsync(promotionViewTemplate);
        }

        await _unitOfWork.SaveAsync();

        var response = new CreatePromotionViewResponse(viewUrl);

        return new Response<CreatePromotionViewResponse>(response);
    }
}