namespace OnAim.Admin.Infrasturcture.HubEntities;

public class AccountType : DbEnum<int>
{
    public static AccountType Player => FromId(1);
    public static AccountType Game => FromId(2);
    public static AccountType Casino => FromId(3);
    public static AccountType Reset => FromId(4);

    private static AccountType FromId(int id)
    {
        return new AccountType { Id = id };
    }
}