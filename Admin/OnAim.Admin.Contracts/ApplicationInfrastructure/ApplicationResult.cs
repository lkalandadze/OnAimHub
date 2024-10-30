using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;

namespace OnAim.Admin.Contracts.ApplicationInfrastructure;

public class ApplicationResult<T>
{
    public bool Success { get; set; }
    public IEnumerable<Error> Errors { get; set; }
    public T Data { get; set; }


    public static ApplicationResult<T> Default()
    {
        return new ApplicationResult<T>();
    }
}

public class ApplicationResult : ApplicationResult<dynamic>
{
    public new static ApplicationResult Default()
    {
        return new ApplicationResult();
    }
}
