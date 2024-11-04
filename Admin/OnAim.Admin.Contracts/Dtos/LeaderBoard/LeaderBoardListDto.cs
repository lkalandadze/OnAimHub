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
public enum JobTypeEnum
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Custom = 3
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