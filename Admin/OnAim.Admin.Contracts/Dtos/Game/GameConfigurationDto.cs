namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameConfigurationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public int PromotionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string FromTemplateId { get; set; } = string.Empty;
    public List<PriceDto> Prices { get; set; }
    public List<RoundDto> Rounds { get; set; }
}
