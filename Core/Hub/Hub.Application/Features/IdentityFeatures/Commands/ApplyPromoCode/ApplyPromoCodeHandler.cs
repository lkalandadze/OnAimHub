//using Hub.Application.Services.Abstract;
//using Hub.Domain.Abstractions;
//using Hub.Domain.Abstractions.Repository;
//using Hub.Domain.Entities;
//using Hub.Domain.Entities.DbEnums;
//using MediatR;
//using Shared.Application.Exceptions;
//using Shared.Application.Exceptions.Types;

//namespace Hub.Application.Features.IdentityFeatures.Commands.ApplyPromoCode;

//public class ApplyPromoCodeHandler : IRequestHandler<ApplyPromoCodeCommand, Unit>
//{
//    private readonly IPlayerRepository _playerRepository;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IAuthService _authService;
//    private readonly IReferralDistributionRepository _referralDistributionRepository;
//    private readonly IHubSettingRepository _settingRepository;
//    private readonly HubSettings _hubSettings;

//    public ApplyPromoCodeHandler(
//        IPlayerRepository playerRepository,
//        IUnitOfWork unitOfWork,
//        IAuthService authService,
//        IReferralDistributionRepository referralDistributionRepository,
//        IHubSettingRepository settingRepository,
//        HubSettings hubSettings)
//    {
//        _playerRepository = playerRepository;
//        _unitOfWork = unitOfWork;
//        _authService = authService;
//        _referralDistributionRepository = referralDistributionRepository;
//        _settingRepository = settingRepository;
//        _hubSettings = hubSettings;
//    }

//    public async Task<Unit> Handle(ApplyPromoCodeCommand request, CancellationToken cancellationToken)
//    {
//        int playerId = _authService.GetCurrentPlayerId();

//        int referrerId = Player.PromoToId(request.ReferralCode);

//        var recommendedByUser = _playerRepository.Query()
//                                                 .FirstOrDefault(x => x.Id == referrerId);

//        if (recommendedByUser == null)
//        {
//            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Recommended by user with the specified Code: [{request.ReferralCode}] was not found.");
//        }

//        var player = _playerRepository.Query()
//                                      .FirstOrDefault(x => x.Id == playerId);

//        if (player == default)
//        {
//            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{playerId}] was not found.");
//        }

//        if (player.HasPlayed)
//        {
//            throw new ApiException(
//                ApiExceptionCodeTypes.BusinessRuleViolation,
//                $"Played with the specified ID: [{playerId}] has already made transaction, unable to redeem promo come."
//            );
//        }

//        if (player.ReferrerId != null)
//        {
//            throw new ApiException(
//                ApiExceptionCodeTypes.BusinessRuleViolation,
//                $"Player with the specified ID: [{playerId}] already applied referral code."
//            );
//        }

//        var settings = _settingRepository.Query().ToList();

//        var recommendedCount = _playerRepository.Query()
//                                                .Count(x => x.ReferrerId == recommendedByUser.Id);

//        player.UpdateRecommendedById(recommendedByUser.Id);

//        var referralDistribution = new ReferralDistribution(
//            referrerId: recommendedByUser.Id,
//            referralId: playerId,
//            referrerPrizeId: _hubSettings.ReferrerPrizeCurrencyId.Value,
//            referrerPrizeValue: _hubSettings.ReferrerPrizeAmount.Value,
//            referrerPrizeCurrency: Currency.FromId(_hubSettings.ReferrerPrizeCurrencyId.Value),
//            referralPrizeValue: _hubSettings.ReferralPrizeAmount.Value,
//            referralPrizeId: _hubSettings.ReferralPrizeCurrencyId.Value,
//            referralPrizeCurrency: Currency.FromId(_hubSettings.ReferralPrizeCurrencyId.Value)
//        );

//        await _referralDistributionRepository.InsertAsync(referralDistribution);

//        await _unitOfWork.SaveAsync();

//        return Unit.Value;
//    }
//}