#nullable disable
using System.Linq.Expressions;

namespace CheckmateValidations
{
    public abstract class Check
    {
        public string Property { get; set; }
        public List<Object> Values { get; set; } = [];
        public ValidationType ValidationType { get; set; }
        public string Message { get; set; }
        public abstract Func<object, object> GetExpression();
        public abstract List<T> GetValues<T>();

    }

    public class Check<TEntity, TProperty> : Check
    {
        public override List<T> GetValues<T>()
        {
            return Values.Cast<T>().ToList();
        }

        public Expression<Func<TEntity, TProperty>> Expression { get; set; }

        public override Func<object, object> GetExpression()
        {
            var compiledExpression = Expression.Compile();

            return obj => compiledExpression((TEntity)obj); 
        }

        public Check<TEntity, TProperty> IsEqual(TProperty value)
        {
            ValidationType = ValidationType.Equals;
            Values.Add(value);

            return this;
        }

        public Check<TEntity, TProperty> LessThan(TProperty value)
        {
            ValidationType = ValidationType.LessThan;
            Values.Add(value);

            return this;
        }

        public Check<TEntity, TProperty> GreaterThan(TProperty value)
        {
            ValidationType = ValidationType.GreaterThan;
            Values.Add(value);

            return this;
        }

        public Check<TEntity, TProperty> Between(TProperty value1, TProperty value2)
        {
            ValidationType = ValidationType.Between;
            Values.Add(value1);
            Values.Add(value2);

            return this;
        }

        public Check<TEntity, TProperty> WithMessage(string message)
        {
            Message = message;
            return this;
        }
    }
}