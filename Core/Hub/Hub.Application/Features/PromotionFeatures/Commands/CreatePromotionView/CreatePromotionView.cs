using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

public record CreatePromotionView(string Name, string ViewContent, int PromotionId, int? TemplateId, bool? SaveAsTemplate) : IRequest<Response<CreatePromotionViewResponse>>;