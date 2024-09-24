namespace OnAim.Admin.Shared.Specification
{
    internal interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T entity);
    }
}

