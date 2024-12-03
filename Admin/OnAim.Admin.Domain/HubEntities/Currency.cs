using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities
{
    // Generated Code
    public class Currency : DbEnum<string, Currency>
    {
        public static Currency OnAimCoin => FromId(nameof(OnAimCoin));
        public static Currency FreeSpin => FromId(nameof(FreeSpin));
    }
}
