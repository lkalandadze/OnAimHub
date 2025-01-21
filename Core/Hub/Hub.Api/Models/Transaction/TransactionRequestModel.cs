namespace Hub.Api.Models.Transaction;

public class TransactionRequestModel
{
    public int? KeyId { get; set; }
    public string SourceServiceName { get; set; }
    public string CoinId { get; set; }
    public decimal Amount { get; set; }
    public Domain.Entities.DbEnums.AccountType FromAccount { get; set; }
    public Domain.Entities.DbEnums.AccountType ToAccount { get; set; }
    public Domain.Entities.DbEnums.TransactionType TransactionType { get; set; }
    public int PromotionId { get; set; }
    public string AdditionalData { get; set; }
}
