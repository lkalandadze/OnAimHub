using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Update;

public sealed class UpdateLeaderboardRecordCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public List<UpdateLeaderboardRecordCommandItem> Prizes { get; set; }
}
public class UpdateLeaderboardRecordCommandItem
{
    public int Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}