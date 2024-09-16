using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.ApplyReferralCode;

public class ApplyReferralCodeCommandHandler : IRequestHandler<ApplyReferralCodeCommand, Unit>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly ITransactionRepository _transactionRepository;

    public ApplyReferralCodeCommandHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork, IAuthService authService, ITransactionRepository transactionRepository)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _transactionRepository = transactionRepository;
    }

    public async Task<Unit> Handle(ApplyReferralCodeCommand request, CancellationToken cancellationToken)
    {
        int playerId = _authService.GetCurrentPlayerId();

        var recommendedByUser = _playerRepository.Query().FirstOrDefault(x => x.ReferralCode == request.ReferralCode);

        var player = _playerRepository.Query().FirstOrDefault(x => x.Id == playerId);

        var transaction = _transactionRepository.Query().FirstOrDefault(x => x.PlayerId == playerId);

        if (recommendedByUser == null)
            throw new Exception("Recommended by user not found");

        if (transaction == default)
            throw new Exception("Player has made a transaction");

        if (player == default)
            throw new Exception("Player not found");

        if (player.RecommendedById != null)
            throw new Exception("Player already applied referral code");

        player.UpdateRecommendedById(recommendedByUser.Id);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}