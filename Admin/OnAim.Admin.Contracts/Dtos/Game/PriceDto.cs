namespace OnAim.Admin.Contracts.Dtos.Game;

public class PriceDto
{
    public string Id { get; set; }
    public int Value { get; set; }
    public decimal Multiplier { get; set; }
    public string CoinId { get; set; }
}
