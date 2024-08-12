using Hub.Application.Configurations;
using Hub.Application.Extensions;
using Hub.Application.Models.Auth;
using Hub.Application.Models.Player;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hub.Application.Services;

public class AuthService : IAuthService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;
    private readonly JwtTokenConfiguration _jwtTokenConfiguration;

    public AuthService(IPlayerRepository playerRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration, IOptions<JwtTokenConfiguration> jwtTokenConfiguration)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
        _jwtTokenConfiguration = jwtTokenConfiguration.Value;
    }

    public async Task<AuthResultModel> AuthAsync(string casinoToken)
    {
        var queryParams = new Dictionary<string, string>
        { 
            { "token", casinoToken }
        };

        var recievedPlayer = await _httpClient.GetAsync<PlayerGetModel>(_casinoApiConfiguration.GetPlayer, queryParams);

        if (recievedPlayer == null)
        {
            throw new ArgumentNullException();
        }

        var player = await _playerRepository.OfIdAsync(recievedPlayer.Id);

        if (player == null)
        {
            player = new Player
            {
                Id = recievedPlayer.Id,
                UserName = recievedPlayer.UserName,
            };

            await _playerRepository.InsertAsync(player);
            await _unitOfWork.SaveAsync();
        }

        return new AuthResultModel() { Success = true, Token = GenerateToken(player) };
    }

    private string GenerateToken(Player player)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, player.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, player.UserName),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtTokenConfiguration.Issuer,
            audience: _jwtTokenConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_jwtTokenConfiguration.DurationInMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}