using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Job : BaseEntity<int>
{
    public Job(string name, string description, string currencyId, string cronExpression, bool isActive)
    {
        Name = name;
        Description = description;
        CurrencyId = currencyId;
        CronExpression = cronExpression;
        IsActive = isActive;
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrencyId { get; set; }
    public string CronExpression { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastExecutedTime { get; set; }
}
