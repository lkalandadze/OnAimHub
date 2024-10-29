using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

public sealed class CreateLeaderboardRecordCommand : IRequest
{
    public int? LeaderboardTemplateId { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public List<CreateLeaderboardRecordPrizeCommandItem> LeaderboardPrizes { get; set; }
}

public class CreateLeaderboardRecordPrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}