using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

public record CreatePromotionView(string viewContent, string Name, int PromotionId) : IRequest<Response<CreatePromotionViewResponse>>;