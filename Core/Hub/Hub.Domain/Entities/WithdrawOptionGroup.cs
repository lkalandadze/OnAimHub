#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class WithdrawOptionGroup : BaseEntity<int>
{
    public WithdrawOptionGroup()
    {
        
    }

    public WithdrawOptionGroup(string title, string description, string imageUrl)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
    }

    public string Title { get; set; }
    public string Description { get; set; }    
    public string ImageUrl { get; set; }
}