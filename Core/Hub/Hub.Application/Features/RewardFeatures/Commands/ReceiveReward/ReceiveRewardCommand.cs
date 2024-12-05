using MediatR;

namespace Hub.Application.Features.RewardFeatures.Commands.ReceiveReward;

public record ReceiveRewardCommand : IRequest
{
    public List<RewardData> Rewards { get; set; }
};

public record RewardData
{
    public int PlayerId { get; set; }
    public string CoinId { get; set; }
    public int Amount { get; set; }
}