using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.PromotionFeatures.Commands.UpdateStatus;

public record UpdatePromotionStatusCommand(
        int Id,
        PromotionStatus Status
    ) : IRequest;