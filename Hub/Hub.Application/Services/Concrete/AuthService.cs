using Hub.Application.Services.Abstract;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace Hub.Application.Services.Concrete;

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
                throw new InvalidOperationException();
            }

            var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException();
            }

            return new JwtSecurityToken(jwtEncodedString: token);
        }
    }

    public AuthService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Player GetCurrentPlayer()
    {
        return new Player(GetCurrentPlayerId(), GetCurrentPlayerUserName(), GetCurrentPlayerSegmentIds());
    }

    public List<string> GetCurrentPlayerSegmentIds()
    {
        var segmentIds = Token.Claims.FirstOrDefault(x => x.Type == "SegmentIds")?.Value;
        return segmentIds?.Split(',').ToList() ?? new List<string>();
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