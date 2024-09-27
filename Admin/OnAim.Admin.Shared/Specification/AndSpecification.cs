using System.Linq.Expressions;

namespace OnAim.Admin.Shared.Specification;

public sealed class AndSpecification<T> : Specification<T>
{
    public AndSpecification(Specification<T> firstSpec, Specification<T> secondSpec)
    {
        var firstExp = firstSpec.ToExpression();
        var secondExp = secondSpec.ToExpression();

        expression = Expression.Lambda<Func<T, bool>>(
          Expression.And(firstExp.Body, Expression.Invoke(secondExp, firstExp.Parameters)),
          firstExp.Parameters);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return expression;
    }
}

