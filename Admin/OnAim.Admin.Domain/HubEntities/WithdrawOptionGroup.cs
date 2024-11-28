using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Domain.HubEntities;

public class WithdrawOptionGroup
{
    public WithdrawOptionGroup()
    {
    }

    public WithdrawOptionGroup(
        string title,
        string description,
        string imageUrl,
        IEnumerable<WithdrawOption> withdrawOptions = null,
         IEnumerable<CoinTemplate> coinTemplates = null)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
        CoinTemplates = coinTemplates?.ToList() ?? [];
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<CoinTemplate> CoinTemplates { get; set; }
    public ICollection<WithdrawOption> WithdrawOptions { get; set; }
}