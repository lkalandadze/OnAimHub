namespace OnAim.Admin.APP.Exceptions
{
    public class ValidationException(string message, System.Exception? innerException = null, params string[] errors)
    : BadRequestException(message);
}
