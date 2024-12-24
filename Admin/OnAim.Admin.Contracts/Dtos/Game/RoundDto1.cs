namespace OnAim.Admin.Contracts.Dtos.Game;

public class RoundDto
{
    public int Id { get; set; }
    public List<int> Sequence { get; set; }
    public int NextPrizeIndex { get; set; }
    public int ConfigurationId { get; set; }
    public List<PrizeDto> Prizes { get; set; }
    public string Name { get; set; }
}
