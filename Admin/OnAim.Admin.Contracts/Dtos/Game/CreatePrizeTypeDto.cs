namespace OnAim.Admin.Contracts.Dtos.Game;

public class CreatePrizeTypeDto
{
    public string Name { get; set; }
    public bool IsMultiplied { get; set; }
    public string CurrencyId { get; set; }
}
