namespace OnAim.Admin.Contracts.Dtos.Game;

public class FilterGamesDto
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Name { get; set; }
    public int? PromotionId { get; set; }
}
