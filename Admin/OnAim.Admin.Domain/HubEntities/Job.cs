#nullable disable

namespace OnAim.Admin.Domain.HubEntities;
public enum JobType
{
    Daily = 1,
    Weekly,
    Monthly,
    Custom,
}
public enum JobCategory
{
    DailyProgressReset = 1,
    CurrencyBalanceReset = 2,
}
public class Job : BaseEntity<int>
{
    public Job()
    {
        
    }

    public Job(string name, string description, bool isActive, JobType jobType, JobCategory jobCategory, TimeSpan? executionTime = null, string currencyId = null, int? intervalInDays = null)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        Type = jobType;
        Category = jobCategory;
        ExecutionTime = executionTime;
        CurrencyId = currencyId;
        IntervalInDays = intervalInDays;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public JobType Type { get; private set; }
    public JobCategory Category { get; private set; }
    public TimeSpan? ExecutionTime { get; private set; }
    public DateTimeOffset? LastExecutedTime { get; set; }
    public string CurrencyId { get; private set; }
    public int? IntervalInDays { get; private set; }

    public void SetLastExecutedTime()
    {
        LastExecutedTime = DateTime.Now;
    }

    public void Test()
    {
        Name = " dsffgsdf";
    }
}