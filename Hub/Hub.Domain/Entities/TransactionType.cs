using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class TransactionType : DbEnum
{
    public static TransactionType Bet => FromId(1);
    public static TransactionType Win => FromId(2);

    private static TransactionType FromId(int id)
    {
        return new TransactionType { Id = id };
    }
}
