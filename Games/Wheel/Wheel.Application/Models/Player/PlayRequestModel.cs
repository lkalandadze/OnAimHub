#nullable disable

namespace Wheel.Application.Models.Player;

public class PlayRequestModel
{
    public int GameId { get; set; }
    public int Amount { get; set; }
    public string CurrencyId { get; set; }
}