using System.Linq.Expressions;

namespace OnAim.Admin.Shared.Specification;

public sealed class NotSpecification<T> : Specification<T>
{
    public NotSpecification(Specification<T> spec)
    {
        var exp = spec.ToExpression();

        expression = Expression.Lambda<Func<T, bool>>(Expression.Not(exp.Body), exp.Parameters);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return expression;
    }
}

