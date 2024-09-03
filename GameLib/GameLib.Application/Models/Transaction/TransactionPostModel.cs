namespace GameLib.Application.Models.Transaction;

public class TransactionPostModel
{
    public int GameId { get; set; }
    public string CurrencyId { get; set; }
    public decimal Amount { get; set; }
}