using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.Delete;

public record DeletePromotionCommand(Guid CorrelationId) : IRequest;