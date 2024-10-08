#nullable disable
using System.Linq.Expressions;
using System.Reflection;

namespace CheckmateValidations
{
    public class Checkmate
    {
        public List<Check> Checks { get; private set; } = [];

        public static bool Check<U>(U obj)
        {
            var checks = GetCheckers(obj);

            foreach (var check in checks)
            {
                if (!Check(obj, check))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Check<TEntity>(TEntity obj, Check check)
        {
            var val = (TEntity)check.GetExpression()(obj);

            switch (check.ValidationType)
            {
                case ValidationType.Equals:
                    return val.Equals((TEntity)check.Values.First());

                //case ValidationType.LessThan:
                //    return GetPropertyValue(obj, check.Property) < check.Values.First();

                //case ValidationType.GreaterThan:
                //    return GetPropertyValue(obj, check.Property) > check.Values.First();

                //case ValidationType.Between:
                //    return GetPropertyValue(obj, check.Property) >= check.Values.First()
                //              && GetPropertyValue(obj, check.Property) <= check.Values[1];
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

        public static List<Check> GetCheckers(object obj)
        {
            var type = obj.GetType();

            if (type.GetCustomAttributes(typeof(CheckMateAttribute)).FirstOrDefault() is { } attr)
            {
                var checkmateType = ((CheckMateAttribute)attr).GetCheckmateType();

                var instance = (Checkmate)Activator.CreateInstance(checkmateType);

                return instance.Checks;
            }

            return null;
        }
    }

    public abstract class Checkmate<SourceType> : Checkmate where SourceType : class
    {
        public Checkmate()
        {

        }

        public Check<SourceType, PropertyType> Check<PropertyType>(Expression<Func<SourceType, PropertyType>> expression)
        {
            string propertyChain = GetPropertyChain(expression.Body);

            if (!string.IsNullOrEmpty(propertyChain))
            {
                var newCheck = new Check<SourceType, PropertyType> { Property = propertyChain, Expression = expression };
                Checks.Add(newCheck);
                return newCheck;
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
}