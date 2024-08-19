namespace OnAim.Admin.Infrasturcture.Exceptions
{
    public partial class Exceptions
    {
        public class RoleNotFoundException : Exception
        {
            public RoleNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
