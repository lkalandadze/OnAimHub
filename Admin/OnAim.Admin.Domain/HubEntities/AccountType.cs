using OnAim.Admin.Domain.HubEntities.Enum;

public class AccountType : DbEnum<int, AccountType>
{
    public static AccountType Player => FromId(1);
    public static AccountType Game => FromId(2);
    public static AccountType Casino => FromId(3);
    public static AccountType Reset => FromId(4);
}