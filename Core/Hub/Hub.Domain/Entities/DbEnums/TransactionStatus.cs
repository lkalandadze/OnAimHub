using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class TransactionStatus : DbEnum<int, TransactionStatus>
{
    public static TransactionStatus Created => FromId(1, nameof(Created));
    public static TransactionStatus Delivered => FromId(2, nameof(Delivered));
    public static TransactionStatus Failed => FromId(3, nameof(Failed));
    public static TransactionStatus Cancelled => FromId(4, nameof(Cancelled));
}