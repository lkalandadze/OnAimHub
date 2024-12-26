using CheckmateValidations;
using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

[CheckMate<CreatePromotionViewChecker>]
public record CreatePromotionView(
    string Name, 
    string ViewContent, 
    int PromotionId,
    int? CreatedByUserId,
    string? TemplateId) : IRequest<Response<CreatePromotionViewResponse>>;