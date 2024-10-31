using LevelService.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Configuration : BaseEntity<int>
{
    public Configuration(string currencyId, decimal experienceToGrant)
    {
        CurrencyId = currencyId;
        ExperienceToGrant = experienceToGrant;
    }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public decimal ExperienceToGrant { get; set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }

    public void Update(string currencyId, decimal experienceToGrant)
    {
        CurrencyId = currencyId;
        ExperienceToGrant = experienceToGrant;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
