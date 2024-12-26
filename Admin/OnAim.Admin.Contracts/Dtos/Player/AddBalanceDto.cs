namespace OnAim.Admin.Contracts.Dtos.Player;

public class AddBalanceDto
{
    public int PlayerId { get; set; }
    public string CoinId { get; set; }
    public int Amount { get; set; }
}