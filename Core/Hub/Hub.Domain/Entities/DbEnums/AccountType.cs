using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class AccountType : DbEnum<int, AccountType>
{
    public static AccountType Player => FromId(1);
    public static AccountType Game => FromId(2);
    public static AccountType Casino => FromId(3);
    public static AccountType Reset => FromId(4);
    public static AccountType Leaderboard => FromId(5);
}