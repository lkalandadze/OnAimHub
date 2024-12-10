#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Job;

public class CreateJobModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public JobType JobType { get; set; }
    public JobCategory JobCategory { get; set; }
    public TimeSpan? ExecutionTime { get; set; }
    public string CoinId { get; set; }
    public int? IntervalInDays { get; set; }
}
