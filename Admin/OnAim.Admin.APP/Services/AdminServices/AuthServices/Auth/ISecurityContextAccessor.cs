namespace OnAim.Admin.APP.Services.Admin.AuthServices.Auth;

public interface ISecurityContextAccessor
{
    int UserId { get; }
    string Role { get; }
    string JwtToken { get; }
    bool IsAuthenticated { get; }
}
