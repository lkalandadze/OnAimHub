namespace Hub.Application.Models.Job;

public class CreateJobModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrencyId { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string CronExpression { get; set; }
    public bool IsActive { get; set; }
}
