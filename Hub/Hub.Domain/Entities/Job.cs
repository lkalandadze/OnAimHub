using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Job : BaseEntity<int>
{
    public Job(string name, string description, string currencyId, bool isActive, TimeSpan? executionTime, int? intervalInDays, JobType jobType)
    {
        Name = name;
        Description = description;
        CurrencyId = currencyId;
        IsActive = isActive;
        ExecutionTime = executionTime;
        IntervalInDays = intervalInDays;
        JobType = jobType;
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrencyId { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan? ExecutionTime { get; set; }
    public int? IntervalInDays { get; set; }
    public JobType JobType { get; set; }
    public DateTime? LastExecutedTime { get; set; }
}
