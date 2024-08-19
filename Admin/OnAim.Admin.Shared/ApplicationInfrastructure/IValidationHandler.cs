namespace OnAim.Admin.Shared.ApplicationInfrastructure
{
    public interface IValidationHandler<T>
       where T : class
    {
        void Validate(T model);
        IValidationHandler<T> NextValidate(IValidationHandler<T> nextValidation);
    }
}
