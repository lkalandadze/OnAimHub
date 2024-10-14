#nullable disable

using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace CheckmateValidations;

public class CheckContainerWithInstance
{
    public CheckContainer CheckContainer { get; set; }
    public object Instance { get; set; }
}

public class Checkmate
{
    public List<CheckContainer> CheckContainers { get; private set; } = [];

    public static bool IsValid<TEntity>(TEntity obj, bool isTree = false)
    {
        if (isTree)
        {
            return !GetTreeFailedChecks(obj).Any();
        }

        return !GetFailedChecks(obj).Any();
    }

    public static IEnumerable<Check> GetFailedChecks<TEntity>(TEntity obj, bool isTree = false)
    {
        /// es ra aris?????
        if (isTree)
        {
            GetTreeFailedChecks(obj);
        }

        var checkContainers = GetCheckContainers(obj);

        foreach (var checkContainer in checkContainers)
        {
            var val = checkContainer.CheckContainer.GetExpression()(checkContainer.Instance);

            foreach (var check in checkContainer.CheckContainer.Checks)
            {
                var predicate = check.GetPredicate();

                if (!predicate(val))
                {
                    yield return check;
                }
            }
        }
    }

    public static List<CheckContainerWithInstance> GetCheckContainers(object obj, bool isTree = false)
    {
        if (isTree)
        {
            return GetTreeCheckContainers(obj);
        }

        return GetRootCheckContainers(obj.GetType()).Select(x => new CheckContainerWithInstance
        {
            CheckContainer = x,
            Instance = obj
        }).ToList();

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

    public static IEnumerable<Check> GetTreeFailedChecks<TEntity>(TEntity obj)
    {
        var checkContainersWithInsance = GetTreeCheckContainers(obj);

        foreach (var checkContainerWithInsance in checkContainersWithInsance)
        {
            // კონფიგურაციისთვის მუშაობს, ფრაისზე რომ გადადის ვეღარ მოაქ ექსფრეშენი
            var val = checkContainerWithInsance.CheckContainer.GetExpression()(checkContainerWithInsance.Instance);

            foreach (var check in checkContainerWithInsance.CheckContainer.Checks)
            {
                var predicate = check.GetPredicate();

                if (!predicate(val))
                {
                    yield return check;
                }
            }
        }
    }

    private static List<CheckContainerWithInstance> GetTreeCheckContainers<TEntity>(TEntity obj)
    {
        if (obj == null)
        {
            return [];
        }

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var containers = GetCheckContainers(obj);

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
                        containers.AddRange(GetTreeCheckContainers(item));
                    }
                }
            }
        }

        return containers;
    }

    //TEST
    public static List<string> GetAddresses(object obj, string basePath = "")
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

    // OLD ???
    private static object GetPropertyValue<T>(T obj, string propertyName)
    {
        PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
        }

        return propertyInfo.GetValue(obj);
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