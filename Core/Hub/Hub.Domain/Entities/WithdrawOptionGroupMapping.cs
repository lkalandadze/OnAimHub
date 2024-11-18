#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionGroupMapping : BaseEntity<int>
{
    public WithdrawOptionGroupMapping()
    {
        
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImgUrl { get; set; }
}