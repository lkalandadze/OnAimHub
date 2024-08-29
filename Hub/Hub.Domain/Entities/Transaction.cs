using Shared.Lib.Entities;

namespace Hub.Domain.Entities;

public class Transaction : BaseEntity
{
    public override int Id { get; set; }
    public int GameVersionId { get; set; }
    public int CurrencyId { get; set; }
    public decimal Amount { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public int StatusId { get; set; }
    public TransactionStatus Status { get; set; }

    public int TypeId { get; set; }
    public TransactionType Type { get; set; }
}