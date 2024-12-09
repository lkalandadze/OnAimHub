using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using System.IdentityModel.Tokens.Jwt;

namespace GameLib.Application.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _accessor;

    private JwtSecurityToken Token
    {
        get
        {
            var authHeader = _accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new ApiException(ApiExceptionCodeTypes.AuthenticationFailed, "Authorization header is missing.");
            }

            var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(token))
            {
                throw new ApiException(ApiExceptionCodeTypes.AuthenticationFailed, "Authorization token is missing or invalid.");
            }

            try
            {
                return new JwtSecurityToken(jwtEncodedString: token);
            }
            catch (Exception ex)
            {
                throw new ApiException(ApiExceptionCodeTypes.InvalidInputData, $"Failed to parse token: {ex.Message}");
            }
        }
    }

    public AuthService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string GetCurrentPlayerUserName()
    {
        return Token.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value!;
    }

    public int GetCurrentPlayerId()
    {
        return int.Parse(Token.Claims.FirstOrDefault(x => x.Type == "PlayerId")?.Value!);
    }
}