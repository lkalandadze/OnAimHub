using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class AccountType : DbEnum<int, AccountType>
{
    public static AccountType Player => FromId(1, nameof(Player));
    public static AccountType Game => FromId(2, nameof(Game));
    public static AccountType Casino => FromId(3, nameof(Casino));
    public static AccountType Reset => FromId(4, nameof(Reset));
    public static AccountType Leaderboard => FromId(5, nameof(Leaderboard));
    public static AccountType Admin => FromId(6, nameof(Admin));
}