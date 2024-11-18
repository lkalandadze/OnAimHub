#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionGroupMapping : BaseEntity<int>
{
    public WithdrawOptionGroupMapping()
    {
        
    }

    public WithdrawOptionGroupMapping(string title, string description, string imageUrl)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}