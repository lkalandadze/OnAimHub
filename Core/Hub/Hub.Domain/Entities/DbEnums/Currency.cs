using Shared.Domain.Entities;

namespace Hub.Domain.Entities.DbEnums;

public class Currency : DbEnum<string, Currency>
{
    public static Currency OnAimCoin => FromId(nameof(OnAimCoin));
    public static Currency FreeSpin => FromId(nameof(FreeSpin));
}