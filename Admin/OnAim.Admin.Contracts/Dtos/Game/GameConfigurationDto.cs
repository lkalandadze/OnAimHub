namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameConfigurationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public int PromotionId { get; set; }
    public string? FromTemplateId { get; set; }
    public List<PriceDto> Prices { get; set; }
    public List<RoundDto> Rounds { get; set; }
}