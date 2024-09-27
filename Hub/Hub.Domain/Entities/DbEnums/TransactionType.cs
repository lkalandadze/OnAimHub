using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class TransactionType : DbEnum<int, TransactionType>
{
    public static TransactionType Bet => FromId(1);
    public static TransactionType Win => FromId(2);
    public static TransactionType Progress => FromId(3);
}