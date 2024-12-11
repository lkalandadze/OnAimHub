using CheckmateValidations;
using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;

[CheckMate<CreatePromotionViewTemplateChecker>]
public record CreatePromotionViewTemplate(
    string Name, 
    string ViewContent) : IRequest<Response<CreatePromotionViewTemplateResponse>>;