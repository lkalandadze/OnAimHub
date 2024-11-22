using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;

public record CreatePromotionViewTemplate(string ViewContent, string Name) : IRequest<Response<CreatePromotionViewTemplateResponse>>;