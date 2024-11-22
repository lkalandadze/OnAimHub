using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionViewTemplate;

public record CreatePromotionViewTemplate(string Name, string ViewContent) : IRequest<Response<CreatePromotionViewTemplateResponse>>;