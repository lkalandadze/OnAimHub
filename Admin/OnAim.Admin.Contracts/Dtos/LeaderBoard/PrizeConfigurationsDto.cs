namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class PrizeConfigurationsDto
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string PrizeId { get; set; }
    public int StartRank { get; set; }
    public int EndRank { get; set; }
}
