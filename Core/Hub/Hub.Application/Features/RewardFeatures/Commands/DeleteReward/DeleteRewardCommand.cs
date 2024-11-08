using MediatR;

namespace Hub.Application.Features.PrizeClaimFeatures.Commands.DeleteReward;

public record DeleteRewardCommand(int Id) : IRequest;