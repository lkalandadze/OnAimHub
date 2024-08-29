namespace Shared.Application.Models.Transaction;

public class TransactionPostModel
{
    public int GameVersionId { get; set; }
    public int CurrencyId { get; set; }
    public decimal Amount { get; set; }
}