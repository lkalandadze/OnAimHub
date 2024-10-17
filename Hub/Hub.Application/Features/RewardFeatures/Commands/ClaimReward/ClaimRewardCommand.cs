using MediatR;

namespace Hub.Application.Features.RewardFeatures.Commands.ClaimReward;

public record ClaimRewardCommand(int RewardId) : IRequest;