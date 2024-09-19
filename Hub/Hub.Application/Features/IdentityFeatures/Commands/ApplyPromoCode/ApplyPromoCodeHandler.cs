using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.ApplyPromoCode;

public class ApplyPromoCodeHandler : IRequestHandler<ApplyPromoCodeCommand, Unit>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IReferralDistributionRepository _referralDistributionRepository;
    private readonly ISettingRepository _settingRepository;
    public ApplyPromoCodeHandler(IPlayerRepository playerRepository,
                                           IUnitOfWork unitOfWork,
                                           IAuthService authService,
                                           IReferralDistributionRepository referralDistributionRepository,
                                           ISettingRepository settingRepository)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _referralDistributionRepository = referralDistributionRepository;
        _settingRepository = settingRepository;
    }

    public async Task<Unit> Handle(ApplyPromoCodeCommand request, CancellationToken cancellationToken)
    {
        int playerId = _authService.GetCurrentPlayerId();

        int referrerId = Player.PromoToId(request.ReferralCode);

        var recommendedByUser = _playerRepository.Query()
                                                 .FirstOrDefault(x => x.Id == referrerId);

        if (recommendedByUser == null)
            throw new Exception("Recommended by user not found");

        var player = _playerRepository.Query()
                                      .FirstOrDefault(x => x.Id == playerId);

        if (player == default)
            throw new Exception("Player not found");

        if (player.HasPlayed)
            throw new Exception("Played has already made transaction, unable to redeem promo come");

        if (player.ReferrerId != null)
            throw new Exception("Player already applied referral code");

        var settings = _settingRepository.Query().ToList();

        var recommendedCount = _playerRepository.Query()
                                                .Count(x => x.ReferrerId == recommendedByUser.Id);

        player.UpdateRecommendedById(recommendedByUser.Id);

        var referralDistribution = new ReferralDistribution(
            referrerId: recommendedByUser.Id,
            referralId: playerId,
            referrerPrizeId: DbSettings.Instance.ReferrerPrizeCurrencyId,
            referrerPrizeValue: DbSettings.Instance.ReferrerPrizeAmount,
            referrerPrizeCurrency: Currency.FromId(DbSettings.Instance.ReferrerPrizeCurrencyId),
            referralPrizeValue: DbSettings.Instance.ReferralPrizeAmount,
            referralPrizeId: DbSettings.Instance.ReferralPrizeCurrencyId,
            referralPrizeCurrency: Currency.FromId(DbSettings.Instance.ReferralPrizeCurrencyId)
        );

        await _referralDistributionRepository.InsertAsync(referralDistribution);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}