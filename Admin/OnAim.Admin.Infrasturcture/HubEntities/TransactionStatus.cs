namespace OnAim.Admin.Infrasturcture.HubEntities;

public class TransactionStatus : DbEnum<int>
{
    public static TransactionStatus Created => FromId(1);
    public static TransactionStatus Delivered => FromId(2);
    public static TransactionStatus Failed => FromId(3);
    public static TransactionStatus Cancelled => FromId(4);

    private static TransactionStatus FromId(int id)
    {
        return new TransactionStatus { Id = id };
    }
}