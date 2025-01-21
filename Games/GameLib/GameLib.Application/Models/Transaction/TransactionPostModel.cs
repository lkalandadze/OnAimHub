namespace GameLib.Application.Models.Transaction;

public class TransactionPostModel
{
    public int KeyId { get; set; }
    public string SourceServiceName { get; set; }
    public string CoinId { get; set; }
    public int PromotionId { get; set; }
    public decimal Amount { get; set; }
}