using Hub.Application.Configurations;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
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
    private readonly PromotionViewConfiguration _viewConfig;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionViewHandler(
        IPromotionRepository promotionRepository, 
        IPromotionViewRepository promotionViewRepository,
        IPromotionViewTemplateRepository promotionViewTemplateRepository,
        IOptions<PromotionViewConfiguration> viewConfig, 
        IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _promotionViewRepository = promotionViewRepository;
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _viewConfig = viewConfig.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<CreatePromotionViewResponse>> Handle(CreatePromotionView request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.OfIdAsync(request.PromotionId);

        if (promotion == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Promotion with the specified ID: [{request.PromotionId}] was not found.");
        }

        var fileName = $"{promotion.Id}_{promotion.Title}_{request.Name}.html";
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), _viewConfig.Directory);
        var filePath = Path.Combine(uploadsDir, fileName);
        Directory.CreateDirectory(uploadsDir);

        File.WriteAllText(filePath, request.viewContent);

        var viewUrl = $"{_viewConfig.Host}/{_viewConfig.Directory}/{fileName}";

        var promotionView = new PromotionView(request.Name, viewUrl, request.PromotionId);

        await _promotionViewRepository.InsertAsync(promotionView);

        if (request.SaveAsTemplate)
        {
            // TODO: ცალკე უნდა შეექმნას ფოლდერი და ფაილიც!!!
            //var promotionViewTemplate = new PromotionViewTemplate(request.Name, viewUrl, [promotionView]);
            //await _promotionViewTemplateRepository.InsertAsync(promotionViewTemplate);
        }

        await _unitOfWork.SaveAsync();

        var response = new CreatePromotionViewResponse(viewUrl);

        return new Response<CreatePromotionViewResponse>(response);
    }
}