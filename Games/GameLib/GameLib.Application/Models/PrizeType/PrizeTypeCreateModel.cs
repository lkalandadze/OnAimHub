#nullable disable

namespace GameLib.Application.Models.PrizeType;

public class PrizeTypeCreateModel
{
    public string Name { get; set; }
    public bool IsMultiplied { get; set; }
    public string CurrencyId { get; set; }
}