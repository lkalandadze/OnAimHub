namespace OnAim.Admin.APP.Exceptions
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
