using Hub.Domain.Enum;

namespace Hub.Application.Models.Job;

public class CreateJobModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrencyId { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan? ExecutionTime { get; set; }
    public int? IntervalInDays { get; set; }
    public JobType JobType { get; set; }
}
