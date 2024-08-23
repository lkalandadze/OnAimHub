﻿using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shared.Application.Services.Abstract;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Application.Services.Concrete;

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

    public int GetCurrentPlayerSegmentId()
    {
        return int.Parse(Token.Claims.FirstOrDefault(x => x.Type == "SegmentId")?.Value!);
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