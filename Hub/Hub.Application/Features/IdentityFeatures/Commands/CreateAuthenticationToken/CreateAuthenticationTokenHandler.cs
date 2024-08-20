using Hub.Application.Configurations;
using Hub.Application.Models.Player;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Lib.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public class CreateAuthenticationTokenHandler : IRequestHandler<CreateAuthenticationTokenRequest, CreateAuthenticationTokenResponse>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<JwtConfiguration> jwtConfiguration, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _jwtConfiguration = jwtConfiguration.Value;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<CreateAuthenticationTokenResponse> Handle(CreateAuthenticationTokenRequest request, CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetPlayer, request.CasinoToken);

        var recievedPlayer = await _httpClient.GetAsync<PlayerGetModel>(_casinoApiConfiguration.Host, endpoint);

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

        return new CreateAuthenticationTokenResponse(true, GenerateTokenString(player));
    }

    private string GenerateTokenString(Player player)
    {
        var claims = new List<Claim>
            {
                new("PlayerId", player.Id.ToString()),
                new("UserName", player.UserName),
            };

        RsaSecurityKey rsaSecurityKey = GetRsaKey();
        var signingCred = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            audience: _jwtConfiguration.Audience,
            issuer: _jwtConfiguration.Issuer,
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: signingCred
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private RsaSecurityKey GetRsaKey()
    {
        var rsaKey = RSA.Create();
        string xmlKey = File.ReadAllText(_jwtConfiguration.PrivateKeyPath);
        rsaKey.FromXmlString(xmlKey);
        var rsaSecurityKey = new RsaSecurityKey(rsaKey);
        return rsaSecurityKey;
    }
}