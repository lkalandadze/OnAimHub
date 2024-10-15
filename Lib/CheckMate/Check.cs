#nullable disable

using System.Linq.Expressions;

namespace CheckmateValidations;

public abstract class Check
{
    public string Message { get; set; }
    public string ValidationRule { get; set; }
    public abstract Predicate<object> GetPredicate();
}

public class Check<TEntity, TMember> : Check
{
    private readonly Expression<Func<TMember, bool>> _predicate;

    public Check(Expression<Func<TMember, bool>> predicate)
    {
        _predicate = predicate;
        ValidationRule = predicate.ToString();
    }

    public override Predicate<object> GetPredicate()
    {
        return obj =>
        {
            if (obj is TMember member)
            {
                return _predicate.Compile()(member);
            }

            return false;
        };
    }
}

public class FailedCheck
{
    public string Path { get; internal set; }
    public string Message { get; set; }
    public string ValidationRule { get; set; }
    public string MemberSelector { get; set; }
}