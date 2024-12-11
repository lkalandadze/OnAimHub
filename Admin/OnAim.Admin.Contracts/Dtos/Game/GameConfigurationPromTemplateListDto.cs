namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameConfigurationPromTemplateListDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public List<RoundDto> Rounds { get; set; } = new List<RoundDto>();
}
