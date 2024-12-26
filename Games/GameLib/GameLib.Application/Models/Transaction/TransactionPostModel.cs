namespace GameLib.Application.Models.Transaction;

public class TransactionPostModel
{
    public int GameId { get; set; }
    public string CoinId { get; set; }
    public int PromotionId { get; set; }
    public decimal Amount { get; set; }
}