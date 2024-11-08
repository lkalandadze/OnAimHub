#nullable disable

namespace CheckmateValidations;

public abstract class CheckMateAttribute : Attribute
{
    public abstract Type GetCheckmateType();
}

public class CheckMateAttribute<T> : CheckMateAttribute where T : Checkmate
{
    public override Type GetCheckmateType()
    {
        return typeof(T);
    }
}