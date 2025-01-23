namespace OnAim.Admin.Contracts.Dtos.Game;

public class ConfigurationsRequest
{
    public string Name { get; set; }
    public int? PromotionId { get; set; }
    public int? ConfigurationId { get; set; }
}