using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shared.Application.Services.Abstract;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Application.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _accessor;

    public AuthService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public AuthorizedPlayer GetAuthorizedPlayer()
    {
        var authHeader = _accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

        if (string.IsNullOrEmpty(authHeader))
        {
            throw new InvalidOperationException();
        }

        var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException();
        }

        var jwtSecurityToken = new JwtSecurityToken(jwtEncodedString: token);

        if (jwtSecurityToken == null)
        {
            throw new ArgumentNullException();
        }

        return new AuthorizedPlayer
        {
            PlayerId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "PlayerId")?.Value!),
            UserName = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value!,
            SegmentId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "SegmentId")?.Value!),
        };
    }
}