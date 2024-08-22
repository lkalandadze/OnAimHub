namespace OnAim.Admin.Infrasturcture.Exceptions
{
    public class EndpointNotFoundException : Exception
    {
        public EndpointNotFoundException(string message) : base(message)
        {
        }
    }
}
