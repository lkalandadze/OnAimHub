using Hub.Application.Configurations;
using Hub.Application.Models.Player;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Concrete;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Lib.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public class CreateAuthenticationTokenHandler : IRequestHandler<CreateAuthenticationTokenRequest, CreateAuthenticationTokenResponse>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;
    private readonly JwtConfig _jwtTokenConfiguration;

    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository, ITokenService tokenService, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration, IOptions<JwtConfig> jwtTokenConfiguration)
    {
        _playerRepository = playerRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
        _jwtTokenConfiguration = jwtTokenConfiguration.Value;
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

        var signingCredentials = _tokenService.GetSigningCredentials();
        var claims = _tokenService.GetClaims(player);
        var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new CreateAuthenticationTokenResponse(true, token);
    }
}