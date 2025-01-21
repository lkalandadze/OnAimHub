#nullable disable

using System.Linq.Expressions;
using System.Text.RegularExpressions;

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
        ValidationRule = ToLowerFirstCharOfMethod(predicate.ToString());
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

    private string ToLowerFirstCharOfMethod(string input)
    {
        var members = new[] { "Count", "Sum", "Average", "Min", "Max", "Where", "Select", "OrderBy", "OrderByDescending", "ThenBy", "ThenByDescending", "GroupBy", "Join", "First", "Last", "Any", "All", "Length" };

        var regex = new Regex(@"\b(" + string.Join("|", members) + @")\b");

        return regex.Replace(input, match =>
        {
            string memberName = match.Groups[1].Value;
            return $"{char.ToLower(memberName[0])}{memberName.Substring(1)}";
        });
    }
}

public class FailedCheck
{
    public string Path { get; internal set; }
    public string Message { get; set; }
    public string ValidationRule { get; set; }
    public string MemberSelector { get; set; }
}