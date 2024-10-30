namespace OnAim.Admin.CrossCuttingConcerns.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
