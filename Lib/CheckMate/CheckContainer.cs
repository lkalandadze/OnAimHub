﻿#nullable disable

using System.Linq.Expressions;

namespace CheckmateValidations;

public interface IToConditionCheckContainer<TEntity, TMember>
{
    ICheckContainer<TEntity, TMember> SetCondition(Expression<Func<TMember, bool>> predicate);
}

public interface IToWithMessageCheckContainer<TEntity, TMember>
{
    IToConditionCheckContainer<TEntity, TMember> WithMessage(string message);
}

public interface ICheckContainer<TEntity, TMember> : IToConditionCheckContainer<TEntity, TMember>, IToWithMessageCheckContainer<TEntity, TMember>
{

}

public abstract class CheckContainer
{
    public abstract List<Check> Checks { get; }
    public abstract string PropertyPath { get; set; }
    public string MemberSelector => GetExpressionAsString();
    public abstract Func<object, object> GetExpression();
    protected abstract string GetExpressionAsString();
}

public class CheckContainer<TEntity, TMember> : CheckContainer, ICheckContainer<TEntity, TMember>
{
    private List<Check<TEntity, TMember>> _checks { get; set; } = [];

    public override List<Check> Checks => _checks.Cast<Check>().ToList();
    public override string PropertyPath { get; set; }
    public Expression<Func<TEntity, TMember>> Expression { get; set; }
    
    public CheckContainer(Expression<Func<TEntity, TMember>> expression)
    {
        Expression = expression;
    }

    public void Add(Check<TEntity, TMember> check)
    {
        _checks.Add(check);
    }

    public override Func<object, object> GetExpression()
    {
        var compiledExpression = Expression.Compile();

        return obj =>
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The input object cannot be null.");
            }

            if (obj is TEntity entity)
            {
                return compiledExpression(entity);
            }
            else
            {
                throw new InvalidCastException($"The object must be of type {typeof(TEntity).Name}.");
            }
        };
    }

    protected override string GetExpressionAsString()
    {
        return Expression?.Body.ToString() ?? string.Empty;
    }

    public ICheckContainer<TEntity, TMember> SetCondition(Expression<Func<TMember, bool>> predicate)
    {
        _checks.Add(new Check<TEntity, TMember>(predicate));
        return this;
    }

    public IToConditionCheckContainer<TEntity, TMember> WithMessage(string message)
    {
        foreach (var check in _checks)
        {
            if (string.IsNullOrEmpty(check.Message))
            {
                check.Message = message;
            }
        }

        return this;
    }
}

public class CheckContainerWithInstance
{
    public CheckContainer CheckContainer { get; set; }
    internal object Instance { get; set; }
    public string Path { get; internal set; }
}