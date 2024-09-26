using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Create;

public sealed class CreateLeaderboardTemplateCommand : IRequest
{
    public string Name { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public List<CreateLeaderboardTemplatePrizeCommandItem> LeaderboardPrizes { get; set; }
}

public class CreateLeaderboardTemplatePrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}