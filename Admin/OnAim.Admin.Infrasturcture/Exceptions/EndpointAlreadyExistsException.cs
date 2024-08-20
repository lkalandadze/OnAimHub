namespace OnAim.Admin.Infrasturcture.Exceptions
{
    public class EndpointAlreadyExistsException : Exception
    {
        public EndpointAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
