using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.SoftDelete;

public record SoftDeletePromotionCommand(int Id) : IRequest;