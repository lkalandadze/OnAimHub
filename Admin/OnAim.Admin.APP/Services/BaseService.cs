namespace OnAim.Admin.APP.Services;

public abstract class BaseService
{
    protected Task<ApplicationResult<T>> Ok<T>(T data)
    {
        var result = new ApplicationResult<T>
        {
            Data = data,
            Success = true
        };
        return Task.FromResult(result);
    }

    protected Task<ApplicationResult<T>> Ok<T>(T data, IEnumerable<Error> errors)
    {
        var result = new ApplicationResult<T>
        {
            Data = data,
            Errors = errors,
            Success = true
        };
        return Task.FromResult(result);
    }

    protected Task<ApplicationResult<object>> Fail(params Error[] errorMessages)
    {
        var commandExecutionResult = new ApplicationResult<object>
        {
            Success = false,
            Errors = errorMessages.ToList()
        };
        return Task.FromResult(commandExecutionResult);
    }

    protected Task<ApplicationResult<T>> Fail<T>(T data, params Error[] errorMessages)
    {
        var commandExecutionResult = new ApplicationResult<T>
        {
            Success = false,
            Data = data,
            Errors = errorMessages.ToList()
        };
        return Task.FromResult(commandExecutionResult);
    }

    protected Task<ApplicationResult<T>> Fail<T>(params Error[] errorMessages)
    {
        return Fail(Activator.CreateInstance<T>(), errorMessages);
    }
}
