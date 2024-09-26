using System.Linq.Expressions;

namespace OnAim.Admin.Shared.Specification;

public abstract class Specification<T> : ISpecification<T>
{
    protected Expression<Func<T, bool>> expression = null;

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public static AndSpecification<T> operator &(Specification<T> spec1, Specification<T> spec2)
    {
        return new(spec1, spec2);
    }

    public static OrSpecification<T> operator |(Specification<T> spec1, Specification<T> spec2)
    {
        return new(spec1, spec2);
    }

    public static NotSpecification<T> operator !(Specification<T> spec)
    {
        return new(spec);
    }
}

