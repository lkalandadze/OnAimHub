namespace OnAim.Admin.Domain.HubEntities.DbEnums;

public class TransactionStatus : DbEnum<int, TransactionStatus>
{
    public static TransactionStatus Created => FromId(1);
    public static TransactionStatus Delivered => FromId(2);
    public static TransactionStatus Failed => FromId(3);
    public static TransactionStatus Cancelled => FromId(4);
}