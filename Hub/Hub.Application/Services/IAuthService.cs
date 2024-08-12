using Hub.Application.Models.Auth;

namespace Hub.Application.Services;

public interface IAuthService
{
    Task<AuthResultModel> AuthAsync(string casinoToken);
}