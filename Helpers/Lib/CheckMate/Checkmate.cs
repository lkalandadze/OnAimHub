﻿#nullable disable

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

    public static IEnumerable<FailedCheck> GetFailedChecks<TEntity>(TEntity obj, bool isTree = false)
    {
        var checkContainersWithInstance = GetCheckContainersWithInstance(obj, "", isTree);
        var failedChecks = new List<FailedCheck>();

        foreach (var checkContainerWithInstance in checkContainersWithInstance)
        {
            var val = checkContainerWithInstance.CheckContainer.GetExpression()(checkContainerWithInstance.Instance);

            var containerFailedChecks = checkContainerWithInstance.CheckContainer.Checks.Where(x => !x.GetPredicate()(val))
                .Select(x => new FailedCheck
                {
                    Path = checkContainerWithInstance.Path,
                    Message = x.Message,
                    ValidationRule = x.ValidationRule,
                    MemberSelector = checkContainerWithInstance.CheckContainer.MemberSelector,
                });

            failedChecks.AddRange(containerFailedChecks);
        }

        return failedChecks;
    }

    public static List<CheckContainerWithInstance> GetCheckContainersWithInstance<TEntity>(TEntity obj, string path = "", bool isTree = false)
    {
        if (obj == null)
        {
            return new List<CheckContainerWithInstance>();
        }

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => !p.GetMethod.IsStatic && p.GetIndexParameters().Length == 0);
        var containers = new List<CheckContainerWithInstance>();

        if (GetRootCheckContainers(type) != null)
        {
            containers.AddRange(GetRootCheckContainers(type).Select(x => new CheckContainerWithInstance
            {
                CheckContainer = x,
                Instance = obj,
                Path = path,
            }));
        }

        if (isTree)
        {
            foreach (var property in properties)
            {
                try
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue is IEnumerable enumerable && propertyValue.GetType() != typeof(string))
                    {
                        foreach (var item in enumerable)
                        {
                            containers.AddRange(GetCheckContainersWithInstance(item, $"{path}.{property.Name}", true));
                        }
                    }
                    else if (propertyValue != null)
                    {
                        containers.AddRange(GetCheckContainersWithInstance(propertyValue, $"{path}.{property.Name}", true));
                    }
                }
                catch
                {
                    // Handle or log errors
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
            var aaa = memberExpression.Member;
            var bbb = memberExpression.Member.Name;

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