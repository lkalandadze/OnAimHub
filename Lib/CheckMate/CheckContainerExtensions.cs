using CheckmateValidations;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Checkmate;

public static class CheckContainerExtensions
{
    private static ICheckContainer<TEntity, TMember> SetCondition<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> check, string predicate)
    {
        return check.SetCondition(ToExpression<TMember>(predicate));
    }

    public static ICheckContainer<TEntity, TMember> IsEqual<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> checkContainer, TMember value)
    {
        return checkContainer.SetCondition(ToExpression<TMember>($"x == {value}"));
    }

    public static ICheckContainer<TEntity, TMember> LessThan<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> checkContainer, TMember value)
    {
        var exx = ToExpression<TMember>($"x < {value}");

        return checkContainer.SetCondition(ToExpression<TMember>($"x < {value}"));
    }

    public static ICheckContainer<TEntity, TMember> GreaterThan<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> checkContainer, TMember value)
    {
        var exx = ToExpression<TMember>($"x > {value}");

        return checkContainer.SetCondition(ToExpression<TMember>($"x > {value}"));
    }

    public static ICheckContainer<TEntity, TMember> Between<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> checkContainer, TMember value1, TMember value2)
    {
        return checkContainer.SetCondition(ToExpression<TMember>($"x >= {value1} && x <= {value2}"));
    }

    public static ICheckContainer<TEntity, TMember> IsNotNull<TEntity, TMember>(this IToConditionCheckContainer<TEntity, TMember> checkContainer)
    {
        return checkContainer.SetCondition(ToExpression<TMember>("x != null"));
    }

    private static Expression<Func<T, bool>> ToExpression<T>(string exp)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        try
        {
            var expression = DynamicExpressionParser.ParseLambda([parameter], typeof(bool), exp);
            return (Expression<Func<T, bool>>)expression;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}