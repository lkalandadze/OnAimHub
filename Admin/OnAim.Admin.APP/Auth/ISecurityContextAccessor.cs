namespace OnAim.Admin.APP.Auth
{
    public interface ISecurityContextAccessor
    {
        int UserId { get; }
        string Role { get; }
        string JwtToken { get; }
        bool IsAuthenticated { get; }
    }
}
