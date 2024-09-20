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
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPlayerSegmentRepository _playerSegmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public CreateAuthenticationTokenHandler(IPlayerRepository playerRepository,
                                            IPlayerBanRepository playerBanRepository
                                            ISegmentRepository segmentRepository,
                                            IPlayerSegmentRepository playerSegmentRepository,
                                            IUnitOfWork unitOfWork, HttpClient httpClient,
                                            IOptions<CasinoApiConfiguration> casinoApiConfiguration,
                                            ITokenService tokenService)
    {
        _playerRepository = playerRepository;
        _playerBanRepository = playerBanRepository;
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

        if(player != null)
        {
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
            var playerSegment = new PlayerSegment(receivedPlayer.Id, "Default");
            player = new Player(receivedPlayer.Id, receivedPlayer.UserName, recommendedById, [playerSegment]);

            if (request.PromoCode != null) 
            {
                var referralDistribution = new ReferralDistribution(
                    referrerId: recommendedById.GetValueOrDefault(),
                    referralId: player.Id,
                    referrerPrizeId: DbSettings.Instance.ReferrerPrizeCurrencyId,
                    referrerPrizeValue: DbSettings.Instance.ReferrerPrizeAmount,
                    referrerPrizeCurrency: Currency.FromId(DbSettings.Instance.ReferrerPrizeCurrencyId),
                    referralPrizeValue: DbSettings.Instance.ReferralPrizeAmount,
                    referralPrizeId: DbSettings.Instance.ReferralPrizeCurrencyId,
                    referralPrizeCurrency: Currency.FromId(DbSettings.Instance.ReferralPrizeCurrencyId)
                );
            }

            await _playerRepository.InsertAsync(player);
            await _unitOfWork.SaveAsync();
        }

        var (token, refreshToken) = await _tokenService.GenerateTokenStringAsync(player);

        var response = new CreateAuthenticationTokenResponse(token, refreshToken);
        return new Response<CreateAuthenticationTokenResponse>(response);
    }
}