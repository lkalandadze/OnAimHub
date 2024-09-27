namespace OnAim.Admin.Domain.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message)
        : base(message)
    {
    }
}
