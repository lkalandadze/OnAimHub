#nullable disable

using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace CheckmateValidations;

public class Checkmate
{
    public List<CheckContainer> CheckContainers { get; private set; } = [];

    public static bool IsValid<TEntity>(TEntity obj, bool isTree = false)
    {
        return !GetFailedChecks(obj, isTree).Any();
    }

    public static IEnumerable<Check> GetFailedChecks<TEntity>(TEntity obj, bool isTree = false)
    {
        var checkContainersWithInstance = GetCheckContainersWithInstance(obj, "", isTree);

        foreach (var checkContainerWithInstance in checkContainersWithInstance)
        {
            var val = checkContainerWithInstance.CheckContainer.GetExpression()(checkContainerWithInstance.Instance);

            foreach (var check in checkContainerWithInstance.CheckContainer.Checks)
            {
                var predicate = check.GetPredicate();

                if (!predicate(val))
                {
                    yield return check;
                }
            }
        }
    }

    public static List<CheckContainerWithInstance> GetCheckContainersWithInstance<TEntity>(TEntity obj, string path = "", bool isTree = false)
    {
        if (obj == null)
        {
            return [];
        }

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var containers = GetRootCheckContainers(obj.GetType()).Select(x => new CheckContainerWithInstance
        {
            CheckContainer = x,
            Instance = obj,
            Path = path,
        }).ToList();

        if (!isTree)
        {
            return containers;
        }

        foreach (var property in properties)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                var collection = property.GetValue(obj) as IEnumerable;

                if (collection != null)
                {
                    var index = 0;

                    foreach (var item in collection)
                    {
                        containers.AddRange(GetCheckContainersWithInstance(item, $"{path}.{property.Name}[{index}]"));
                        index++;
                    }
                }
            }
        }

        return containers;
    }

    public static List<CheckContainer> GetRootCheckContainers(Type type)
    {
        if (type.GetCustomAttributes(typeof(CheckMateAttribute)).FirstOrDefault() is { } attr)
        {
            var checkmateType = ((CheckMateAttribute)attr).GetCheckmateType();

            var instance = (Checkmate)Activator.CreateInstance(checkmateType);

            return instance.CheckContainers;
        }

        return null;
    }

    //TEST
    private static List<string> GetAddresses(object obj, string basePath = "")
    {
        var addresses = new List<string>();

        if (obj == null) return addresses;

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                var collection = property.GetValue(obj) as IEnumerable;

                if (collection != null)
                {
                    int index = 0;

                    foreach (var item in collection)
                    {
                        string path = $"{basePath}.{property.Name}[{index}]";

                        addresses.Add(path);
                        addresses.AddRange(GetAddresses(item, path));
                        index++;
                    }
                }
            }
        }

        return addresses;
    }
}

public abstract class Checkmate<TEntity> : Checkmate where TEntity : class
{
    public Checkmate()
    {

    }

    public IToConditionCheckContainer<TEntity, TMember> Check<TMember>(Expression<Func<TEntity, TMember>> expression)
    {
        var checkContainer = new CheckContainer<TEntity, TMember>(expression);

        string propertyChain = GetPropertyChain(expression.Body);

        if (!string.IsNullOrEmpty(propertyChain)) //TODO: ???
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