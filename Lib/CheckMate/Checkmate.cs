#nullable disable

using System.Linq.Expressions;
using System.Reflection;

namespace CheckmateValidations;

public class Checkmate
{
    public List<CheckContainer> CheckContainers { get; private set; } = [];

    public static bool IsValid<TEntity>(TEntity obj)
    {
        return !GetFailedChecks(obj).Any();
    }

    public static IEnumerable<Check> GetFailedChecks<TEntity>(TEntity obj)
    {
        var checkContainers = GetCheckContainers(obj);

        foreach (var checkContainer in checkContainers)
        {
            foreach (var check in checkContainer.Checks)
            {
                //TODO

                //if (!IsValid(obj, check))
                //{
                //    yield return check;
                //}

                yield return check;
            }
        }
    }

    private static bool IsValid<TEntity>(TEntity obj, CheckContainer checkContainer)
    {
        var val = checkContainer.GetExpression()(obj);

        foreach (var check in checkContainer.Checks)
        {
            var predicate = check.GetPredicate();

            return predicate(val);
        }

        return true;
    }

    private static object GetPropertyValue<T>(T obj, string propertyName)
    {
        PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
        }

        return propertyInfo.GetValue(obj);
    }

    public static List<CheckContainer> GetCheckContainers(object obj)
    {
        return GetCheckContainers(obj.GetType());
    }

    public static List<CheckContainer> GetCheckContainers(Type type)
    {
        if (type.GetCustomAttributes(typeof(CheckMateAttribute)).FirstOrDefault() is { } attr)
        {
            var checkmateType = ((CheckMateAttribute)attr).GetCheckmateType();

            var instance = (Checkmate)Activator.CreateInstance(checkmateType);

            return instance.CheckContainers;
        }

        return null;
    }
}

public abstract class Checkmate<TEntity> : Checkmate where TEntity : class
{
    public Checkmate()
    {

    }

    public CheckContainer<TEntity, TMember> Check<TMember>(Expression<Func<TEntity, TMember>> expression)
    {
        var checkContainer = new CheckContainer<TEntity, TMember>(expression);
        
        string propertyChain = GetPropertyChain(expression.Body);

        if (!string.IsNullOrEmpty(propertyChain))//???
        {
            CheckContainers.Add(checkContainer);
            return checkContainer;
        }

        throw new ArgumentException("Invalid expression");
    }

    private string GetPropertyChain(Expression expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            var parent = GetPropertyChain(memberExpression.Expression);
            return string.IsNullOrEmpty(parent) ? memberExpression.Member.Name : $"{parent}.{memberExpression.Member.Name}";
        }

        if (expression is MethodCallExpression methodCallExpression)
        {
            string parent = null;

            if (methodCallExpression.Object != null)
            {
                parent = GetPropertyChain(methodCallExpression.Object);
            }
            else if (methodCallExpression.Arguments.Count > 0)
            {
                parent = GetPropertyChain(methodCallExpression.Arguments[0]);
            }

            return parent != null ? $"{parent}.{methodCallExpression.Method.Name}()" : $"{methodCallExpression.Method.Name}()";
        }

        if (expression is UnaryExpression unaryExpression)
        {
            return GetPropertyChain(unaryExpression.Operand);
        }

        return null;
    }
}