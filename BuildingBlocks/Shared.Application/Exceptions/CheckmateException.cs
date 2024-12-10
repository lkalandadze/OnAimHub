using CheckmateValidations;

namespace Shared.Application.Exceptions;

public class CheckmateException : Exception
{
    public IEnumerable<FailedCheck> FailedChecks { get; }

    public CheckmateException(IEnumerable<FailedCheck> failedChecks)
        : base("Validation failed.")
    {
        FailedChecks = failedChecks ?? new List<FailedCheck>();
    }
}