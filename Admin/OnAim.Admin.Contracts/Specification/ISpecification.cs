namespace OnAim.Admin.Contracts.Specification;

internal interface ISpecification<in T>
{
    bool IsSatisfiedBy(T entity);
}

