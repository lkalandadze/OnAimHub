using CheckmateValidations;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Checkmate;

public static class CheckExtensions
{
    public static CheckContainer<TEntity, TMember> SetCondition<TEntity, TMember>(this CheckContainer<TEntity, TMember> checkContainer, Expression<Func<TMember, bool>> predicate)
    {
        var check = new Check<TEntity, TMember>(predicate);
        
        checkContainer.Checks.Add(check);

        return checkContainer;
    }

    //private static CheckContainer<TEntity, TMember> SetCondition<TEntity, TMember>(this CheckContainer<TEntity, TMember> check, string predicate)
    //{
    //    return check.SetCondition(ToExpression<TMember>(predicate));
    //}

    //public static Check<TEntity, TMember> IsEqual<TEntity, TMember>(this Check<TEntity, TMember> check, TMember value)
    //{
    //    return check.SetCondition(ToExpression<TMember>($"x == {value}"));
    //}

    //public static Check<TEntity, TMember> LessThan<TEntity, TMember>(this Check<TEntity, TMember> check, TMember value)
    //{
    //    var exx = ToExpression<TMember>($"x < {value}");

    //    return check.SetCondition(ToExpression<TMember>($"x < {value}"));
    //}

    //public static Check<TEntity, TMember> GreaterThan<TEntity, TMember>(this Check<TEntity, TMember> check, TMember value)
    //{
    //    var exx = ToExpression<TMember>($"x > {value}");

    //    return check.SetCondition(ToExpression<TMember>($"x > {value}"));
    //}

    //public static Check<TEntity, TMember> Between<TEntity, TMember>(this Check<TEntity, TMember> check, TMember value1, TMember value2)
    //{
    //    return check.SetCondition(ToExpression<TMember>($"x >= {value1} && x <= {value2}"));
    //}

    //public static Check<TEntity, TMember> IsNotNull<TEntity, TMember>(this Check<TEntity, TMember> check)
    //{
    //    return check.SetCondition(ToExpression<TMember>("x != null"));
    //}

    //public static Check<TEntity, string> IsNotNullOrEmpty<TEntity>(this Check<TEntity, string> check)
    //{
    //    return check.SetCondition(ToExpression<string>("string.IsNullOrEmpty(x) == false"));
    //}

    public static CheckContainer<TEntity, TMember> WithMessage<TEntity, TMember>(this CheckContainer<TEntity, TMember> checkContainer, string message)
    {
        foreach (var check in checkContainer.Checks)
        {
            if (string.IsNullOrEmpty(check.Message))
            {
                check.Message = message;
            }
        }

        return checkContainer;
    }

    private static Expression<Func<T, bool>> ToExpression<T>(string exp)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var expression = DynamicExpressionParser.ParseLambda(
            [parameter],
            typeof(bool),
            exp
        );

        return (Expression<Func<T, bool>>)expression;
    }
}