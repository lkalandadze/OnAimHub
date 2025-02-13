﻿using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Templates;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;

public class CreatePromotionViewTemplateHandler : IRequestHandler<CreatePromotionViewTemplate, Response<CreatePromotionViewTemplateResponse>>
{
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;
    private readonly IPromotionViewService _promotionViewService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionViewTemplateHandler(
        IPromotionViewTemplateRepository promotionViewTemplateRepository,
        IPromotionViewService promotionViewService,
        IUnitOfWork unitOfWork)
    {
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _promotionViewService = promotionViewService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<CreatePromotionViewTemplateResponse>> Handle(CreatePromotionViewTemplate request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var viewUrl = _promotionViewService.GenerateTemplateViewUrl(request.ViewContent);

        var promotionViewTemplate = new PromotionViewTemplate(request.Name, viewUrl);

        await _promotionViewTemplateRepository.InsertAsync(promotionViewTemplate);
        await _unitOfWork.SaveAsync();

        var response = new CreatePromotionViewTemplateResponse(viewUrl);

        return new Response<CreatePromotionViewTemplateResponse>(response);
    }
}