using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotionView;

public record CreatePromotionView(string ViewContent, string Name, int PromotionId, bool SaveAsTemplate) : IRequest<Response<CreatePromotionViewResponse>>;