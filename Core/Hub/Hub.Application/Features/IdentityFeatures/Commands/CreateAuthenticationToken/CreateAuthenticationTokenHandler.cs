using Hub.Application.Configurations;
using Hub.Application.Models.Player;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using Hub.IntegrationEvents.Player;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Commands;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Player;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;

public class CreateAuthenticationTokenHandler : IRequestHandler<CreateAuthenticationTokenCommand, Response<CreateAuthenticationTokenResponse>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPlayerSegmentRepository _playerSegmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerLogService _playerLogService;
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly HubSettings _hubSettings;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;


    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository,
                                            IPlayerBanRepository playerBanRepository,
                                            ISegmentRepository segmentRepository,
                                            IPlayerSegmentRepository playerSegmentRepository,
                                            IUnitOfWork unitOfWork,
                                            ITokenService tokenService,
                                            IPlayerLogService playerLogService,
                                            HttpClient httpClient,
                                            HubSettings hubSettings,
                                            IOptions<CasinoApiConfiguration> casinoApiConfiguration,
                                            IMediator mediator,
                                            IMessageBus messageBus)
    {
        _playerRepository = playerRepository;
        _playerBanRepository = playerBanRepository;
        _segmentRepository = segmentRepository;
        _playerSegmentRepository = playerSegmentRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _playerLogService = playerLogService;
        _httpClient = httpClient;
        _hubSettings = hubSettings;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
        _mediator = mediator;
        _messageBus = messageBus;
    }

    public async Task<Response<CreateAuthenticationTokenResponse>> Handle(CreateAuthenticationTokenCommand request, CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetPlayer, request.CasinoToken);

        var receivedPlayer = await _httpClient.CustomGetAsync<PlayerGetModel>(_casinoApiConfiguration.Host, endpoint);

        if (receivedPlayer == null)
            throw new ArgumentNullException();

        var player = await _playerRepository.GetPlayerWithSegmentsAsync(receivedPlayer.Id);

        if (player != null)
        {
            if (string.IsNullOrEmpty(player.UserName))
            {
                player.ChangeDetails(receivedPlayer.UserName);
            }
            if (player.RegistredOn == default)
            {
                player.SetRegistrationDate();
            }
            if (player.LastVisitedOn == default)
            {
                player.SetLastVisitDate();
            }

            var bannedPlayer = _playerBanRepository.Query().FirstOrDefault(x => x.PlayerId == player.Id && !x.IsRevoked && x.ExpireDate > DateTimeOffset.UtcNow && x.IsPermanent);

            if (bannedPlayer != null)
                throw new Exception("You have been banned");
        }

        int? recommendedById = null;

        if (!string.IsNullOrEmpty(request.PromoCode))
        {
            int referrerId = Player.PromoToId(request.PromoCode);

            var referringPlayer = _playerRepository.Query().FirstOrDefault(x => x.Id == referrerId);

            if (referringPlayer == default)
                throw new Exception("Invalid referral code provided.");

            recommendedById = referringPlayer.Id;
        }

        if (player == null)
        {
            var playerSegment = new PlayerSegment(receivedPlayer.Id, "default");
            player = new Player(receivedPlayer.Id, receivedPlayer.UserName, recommendedById, [playerSegment]);

            if (request.PromoCode != null)
            {
                var referralDistribution = new ReferralDistribution(
                    referrerId: recommendedById.GetValueOrDefault(),
                    referralId: player.Id,
                    referrerPrizeId: _hubSettings.ReferrerPrizeCurrencyId.Value,
                    referrerPrizeValue: _hubSettings.ReferrerPrizeAmount.Value,
                    referrerPrizeCurrency: Currency.FromId(_hubSettings.ReferrerPrizeCurrencyId.Value),
                    referralPrizeValue: _hubSettings.ReferralPrizeAmount.Value,
                    referralPrizeId: _hubSettings.ReferralPrizeCurrencyId.Value,
                    referralPrizeCurrency: Currency.FromId(_hubSettings.ReferralPrizeCurrencyId.Value)
                );
            }

            await _playerRepository.InsertAsync(player);
        }

        await _playerLogService.CreatePlayerLogAsync($"Player {receivedPlayer.UserName} logged.", receivedPlayer.Id, PlayerLogType.Auth);
        await _unitOfWork.SaveAsync();

        var (token, refreshToken) = await _tokenService.GenerateTokenStringAsync(player);
        var response = new CreateAuthenticationTokenResponse(token, refreshToken);

        var @events = new CreatePlayerEvent(Guid.NewGuid(), player.Id, player.UserName);
        await _messageBus.Publish(@events);
        //await _mediator.Send(events);

        return new Response<CreateAuthenticationTokenResponse>(response);
    }
}