using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test.Api.Models;

namespace Test.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestAuthController : ControllerBase
{
    [HttpGet("player")]
    public ActionResult<PlayerModel> GetPlayer(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aVGh6J/J2eRt6N8yQgP5kE0ThKz+zR/G+gL4X1G+yKo="));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "https://localhost:7141/",
                ValidAudience = "https://localhost:7141/",
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var usernameClaim = claimsPrincipal.FindFirst("username")?.Value;
            var idClaim = claimsPrincipal.FindFirst("Id")?.Value;
            var segmentIdsClaim = claimsPrincipal.FindFirst("SegmentIds")?.Value;

            if (usernameClaim == null || idClaim == null || segmentIdsClaim == null)
            {
                return Unauthorized();
            }

            var segmentIds = segmentIdsClaim
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var player = new PlayerModel
            {
                Id = int.Parse(idClaim),
                UserName = usernameClaim,
                SegmentIds = segmentIds,
            };

            return Ok(player);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    public ActionResult<string> TestAuth()
    {
        var player = new PlayerModel
        {
            Id = Random.Shared.Next(1, 100),
            UserName = Random.Shared.Next(1000, 2000).ToString(),
            SegmentIds = new List<int> { 1, 2, 3 }
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aVGh6J/J2eRt6N8yQgP5kE0ThKz+zR/G+gL4X1G+yKo="));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("username", player.UserName),
            new("Id", player.Id.ToString()),
            new("SegmentIds", string.Join(",", player.SegmentIds)),
        };

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7141/",
            audience: "https://localhost:7141/",
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    [HttpGet("{id}/progress")]
    public ActionResult<ProgressModel> GetPlayerProgress([FromRoute] int id)
    {
        return new ProgressModel
        {
            Progress = new Dictionary<string, double>
            {
                { "SPN", 5 },
                { "FRSPN", 15 },
                { "USD", 2 },
            },
        };
    }
}