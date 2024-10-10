#nullable disable

using System.Linq.Expressions;

namespace CheckmateValidations;

public abstract class CheckContainer
{
    public List<Check> Checks { get; set; } = [];
    public string MemberSelector => GetExpression().ToString();
    public abstract Func<object, object> GetExpression();
}

public class CheckContainer<TEntity, TMember> : CheckContainer
{
    public new List<Check<TEntity, TMember>> Checks { get; set; } = []; // x > 10
    public Expression<Func<TEntity, TMember>> Expression { get; set; } //x

    public CheckContainer(Expression<Func<TEntity, TMember>> expression)
    {
        Expression = expression;
    }

    public void Add(Check<TEntity, TMember> check)
    {
        Checks.Add(check);
    }

    public override Func<object, object> GetExpression()
    {
        var compiledExpression = Expression.Compile();

        return obj => compiledExpression((TEntity)obj);
    }
}

public abstract class Check
{
    public string Message { get; set; }
    public string ValidationRule { get; set; }
    public abstract Predicate<object> GetPredicate();
}

public class Check<TEntity, TMember> : Check
{
    public Expression<Func<TMember, bool>> Predicate { get; set; }

    public Check(Expression<Func<TMember, bool>> predicate)
    {
        Predicate = predicate;
        ValidationRule = predicate.ToString();
    }

    public override Predicate<object> GetPredicate()
    {
        return obj =>
        {
            if (obj is TMember member)
            {
                return Predicate.Compile()(member);
            }

            return false;
        };
    }
}