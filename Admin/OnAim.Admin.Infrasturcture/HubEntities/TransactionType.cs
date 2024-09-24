namespace OnAim.Admin.Infrasturcture.HubEntities;

public class TransactionType : DbEnum<int>
{
    public static TransactionType Bet => FromId(1);
    public static TransactionType Win => FromId(2);
    public static TransactionType Progress => FromId(3);

    private static TransactionType FromId(int id)
    {
        return new TransactionType { Id = id };
    }
}