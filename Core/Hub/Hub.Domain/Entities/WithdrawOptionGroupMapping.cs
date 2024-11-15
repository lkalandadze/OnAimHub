using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionGroupMapping : BaseEntity<int>
{
    public int WithdrawOptionId { get; set; }
    public WithdrawOption WithdrawOption { get; set; }

    public int WithdrawOptionGroupId { get; set; }
    public WithdrawOptionGroup WithdrawOptionGroup { get; set; }
}