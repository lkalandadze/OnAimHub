using Hub.Application.Configurations;
using Hub.Application.Models.Player;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public class CreateAuthenticationTokenHandler : IRequestHandler<CreateAuthenticationTokenRequest, CreateAuthenticationTokenResponse>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration, ITokenService tokenService)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _httpClient = httpClient;
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
                SegmentIds = recievedPlayer.SegmentIds ?? new List<int>()
            };

            await _playerRepository.InsertAsync(player);
            await _unitOfWork.SaveAsync();
        }

        return new CreateAuthenticationTokenResponse(true, _tokenService.GenerateTokenString(player));
    }
}