namespace OnAim.Admin.Contracts.Dtos.Game;

public class CreateRoundDto
{
    public List<int> Sequence { get; set; } = new List<int>();
    public int NextPrizeIndex { get; set; }
    public int ConfigurationId { get; set; }
    public List<CreatePrizeDto> Prizes { get; set; } = new List<CreatePrizeDto>();
    public string Name { get; set; }
}
