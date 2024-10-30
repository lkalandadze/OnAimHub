using System.Linq.Expressions;

namespace OnAim.Admin.Contracts.Specification;

public sealed class OrSpecification<T> : Specification<T>
{
    public OrSpecification(Specification<T> firstSpec, Specification<T> secondSpec)
    {
        var firstExp = firstSpec.ToExpression();
        var secondExp = secondSpec.ToExpression();

        expression = Expression.Lambda<Func<T, bool>>(Expression.Or(firstExp
            .Body, Expression.Invoke(secondExp,
            firstExp
              .Parameters)),
          firstExp
            .Parameters);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return expression;
    }
}

