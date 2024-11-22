using Hub.Application.Configurations;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;

public class CreatePromotionViewTemplateHandler : IRequestHandler<CreatePromotionViewTemplate, Response<CreatePromotionViewTemplateResponse>>
{
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;
    private readonly PromotionViewConfiguration _viewConfig;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionViewTemplateHandler(
        IPromotionViewTemplateRepository promotionViewTemplateRepository,
        IOptions<PromotionViewConfiguration> viewConfig,
        IUnitOfWork unitOfWork)
    {
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _viewConfig = viewConfig.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<CreatePromotionViewTemplateResponse>> Handle(CreatePromotionViewTemplate request, CancellationToken cancellationToken)
    {
        return null;
    }
}