using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class TransactionType : DbEnum<int, TransactionType>
{
    public static TransactionType Bet => FromId(1, nameof(Bet));
    public static TransactionType Win => FromId(2, nameof(Win));
    public static TransactionType Progress => FromId(3, nameof(Progress));
    public static TransactionType Reward => FromId(4, nameof(Reward));
    public static TransactionType Level => FromId(5, nameof(Level));
    public static TransactionType Mission => FromId(6, nameof(Mission));
    public static TransactionType Leaderboard => FromId(7, nameof(Leaderboard));
}


// playerId
// amount
// coinId
// transactionType
// promotionId
// timestamp
// serviceId
// 
//
//
//
//
//
//
//
//
//
