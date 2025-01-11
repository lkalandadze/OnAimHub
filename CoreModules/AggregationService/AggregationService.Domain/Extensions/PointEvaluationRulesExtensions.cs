using AggregationService.Domain.Entities;

namespace AggregationService.Domain.Extensions;

public static class PointEvaluationRulesExtensions
{
    public static PointEvaluationRule? CurrentStep(this IEnumerable<PointEvaluationRule> rules, double currentTotal)
    {
        return rules.OrderBy(x => x.Step).FirstOrDefault(x => x.Point <= currentTotal);
    }

    public static int CurrentPoints(this IEnumerable<PointEvaluationRule> rules, double currentTotal)
    {
        return rules.CurrentStep(currentTotal)?.Point ?? 0;
    }
}