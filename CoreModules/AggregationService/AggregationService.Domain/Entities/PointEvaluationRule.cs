using Shared.Domain.Entities;

namespace AggregationService.Domain.Entities;

public class PointEvaluationRule : BaseEntity<int>
{
    public PointEvaluationRule(int step, int point)
    {
        Step = step;
        Point = point;
    }

    public int Step { get; set; }
    public int Point { get; set; }

    public void Update(int step, int point)
    {
        Step = step;
        Point = point;
    }
}