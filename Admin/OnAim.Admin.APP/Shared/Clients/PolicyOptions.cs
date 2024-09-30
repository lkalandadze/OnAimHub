namespace OnAim.Admin.APP.Shared.Clients;

public class PolicyOptions : ICircuitBreakerPolicyOptions, IRetryPolicyOptions, ITimeoutPolicyOptions
{
    public int RetryCount { get; set; } = 3;
    public int BreakDuration { get; set; } = 30;
    public int TimeOutDuration { get; set; } = 15;
}
public interface ICircuitBreakerPolicyOptions
{
    int RetryCount { get; set; }
    int BreakDuration { get; set; }
}
public interface IRetryPolicyOptions
{
    int RetryCount { get; set; }
}
public interface ITimeoutPolicyOptions
{
    public int TimeOutDuration { get; set; }
}
