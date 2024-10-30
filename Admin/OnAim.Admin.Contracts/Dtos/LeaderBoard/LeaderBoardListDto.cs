namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public ICollection<PrizeDto> Prizes {  get; set; } 
    public DateTimeOffset EndsOn { get; set; }
    public List<PrizeConfigurationsDto> PrizeConfigurations { get; set; }
    public List<LeaderBoardItemsDto> LeaderBoardItems { get; set; }
}
public class PrizeDto
{
    public string PrizeType { get; set; }
    public int Count { get; set; }
}
public class LeaderBoardItemsListDto
{
    public List<LeaderBoardItemsDto> LeaderBoardItemsDtos { get; set; }
}
public class LeaderBoardItemsDto
{
    public int PlayerId { get; set; }
    public string UserName { get; set; }
    public string Segment {  get; set; }
    public int Place {  get; set; }
    public decimal Score { get; set; }
    public string PrizeType { get; set; }
    public string PrizeValue { get; set; }

}
public class PrizeConfigurationsDto
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string PrizeId { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
}

public class CreateLeaderboardTemplateDto
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
public enum JobTypeEnum
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Custom = 3
}

public class UpdateLeaderboardTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public List<UpdateLeaderboardTemplateCommandCommandItem> LeaderboardPrizes { get; set; }
}
public class UpdateLeaderboardTemplateCommandCommandItem
{
    public int Id { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string PrizeId { get; set; }
    public int Amount { get; set; }
}
public class CreateLeaderboardRecordDto
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
public enum LeaderboardRecordStatus
{
    Future = 0,
    Created = 1,
    Announced = 2,
    InProgress = 3,
    Finished = 4,
}
public enum LeaderboardType
{
    Win = 0
}
public class UpdateLeaderboardRecordDto
{
    public int Id { get; set; }
    public string Name { get; set; }
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