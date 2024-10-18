using Hub.Application.Models.Prize;
using MediatR;

namespace Hub.Application.Features.PrizeClaimFeatures.Commands.CreateReward;

public record CreateRewardCommand(int PlayerId, int SourceId, DateTime ExpirationDate, IEnumerable<CreateRewardPrizeModel> Prizes) : IRequest;