using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

public sealed class CreateLeaderboardCommand : IRequest
{
    public int? LeaderboardTemplateId { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public List<CreateLeaderboardPrizeCommandItem> LeaderboardPrizes { get; set; }
}

public class CreateLeaderboardPrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}