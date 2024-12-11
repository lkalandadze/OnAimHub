namespace OnAim.Admin.Contracts.Dtos.Game;

public class CreateGameConfigurationTemplateDto
{
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public List<CreatePriceDto> Prices { get; set; } = new List<CreatePriceDto>();
    public List<CreateRoundDto> Rounds { get; set; } = new List<CreateRoundDto>();
}
