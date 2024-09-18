using Hub.Application.Configurations;
using Hub.Application.Models.Player;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public class CreateAuthenticationTokenHandler : IRequestHandler<CreateAuthenticationTokenCommand, Response<CreateAuthenticationTokenResponse>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPlayerSegmentRepository _playerSegmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository, ISegmentRepository segmentRepository, IPlayerSegmentRepository playerSegmentRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration, ITokenService tokenService)
    {
        _playerRepository = playerRepository;
        _segmentRepository = segmentRepository;
        _playerSegmentRepository = playerSegmentRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<Response<CreateAuthenticationTokenResponse>> Handle(CreateAuthenticationTokenCommand request, CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetPlayer, request.CasinoToken);

        var receivedPlayer = await _httpClient.CustomGetAsync<PlayerGetModel>(_casinoApiConfiguration.Host, endpoint);

        if (receivedPlayer == null)
            throw new ArgumentNullException();

        var player = await _playerRepository.OfIdAsync(receivedPlayer.Id);

        if (player == null)
        {
            var segments = _segmentRepository.Query(s => receivedPlayer.SegmentIds.Any(i => i == s.Id)).ToList();

            player = new Player(receivedPlayer.Id, receivedPlayer.UserName, playerSegments: []);

            foreach (var segment in segments)
            {
                var playerSegment = new PlayerSegment(receivedPlayer.Id, segment.Id);
                player.PlayerSegments.Add(playerSegment);
            }

            await _playerRepository.InsertAsync(player);
            await _unitOfWork.SaveAsync();
        }

        var (token, refreshToken) = await _tokenService.GenerateTokenStringAsync(player);

        var response = new CreateAuthenticationTokenResponse(token, refreshToken);
        return new Response<CreateAuthenticationTokenResponse>(response);
    }
}