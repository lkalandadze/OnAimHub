namespace OnAim.Admin.Shared.ApplicationInfrastructure.Validation;

public abstract class ValidationHandler<T> : IValidationHandler<T>
     where T : class
{
    private IValidationHandler<T>? _nextValidation;

    public virtual void Validate(T model)
    {
        _nextValidation?.Validate(model);
    }

    public IValidationHandler<T> NextValidate(IValidationHandler<T> nextValidation)
    {
        _nextValidation = nextValidation;

        return _nextValidation;
    }
}
