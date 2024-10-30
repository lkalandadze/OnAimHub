namespace OnAim.Admin.Contracts.Models;

public class SystemDate
{
    public static DateTimeOffset Now
    {
        get
        {
            var date = DateTimeOffset.Now.ToUniversalTime();
            return date;
        }
    }
}
