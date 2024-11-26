using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.PromotionFeatures.Commands.UpdateStatus;

public class UpdatePromotionStatusCommandHandler : IRequestHandler<UpdatePromotionStatusCommand>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePromotionStatusCommandHandler(IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdatePromotionStatusCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.Query().FirstOrDefaultAsync(x => x.Id == request.Id);

        if (promotion == default)
            throw new Exception("Promotion not found");

        if (promotion.Status == PromotionStatus.ToLaunch && request.Status != PromotionStatus.UpComing && request.Status != PromotionStatus.Paused)
            throw new Exception($"Invalid transition: Can only transition from {PromotionStatus.ToLaunch} to {PromotionStatus.UpComing} or {PromotionStatus.Paused}.");

        if (promotion.Status == PromotionStatus.UpComing && !(request.Status == PromotionStatus.Paused 
                                                             || request.Status == PromotionStatus.Cancelled))
            throw new Exception($"Invalid transition: Can only transition from {PromotionStatus.UpComing} to {PromotionStatus.Paused} or {PromotionStatus.Cancelled}.");

        if (promotion.Status == PromotionStatus.Started && request.Status != PromotionStatus.Paused)
            throw new Exception($"Invalid transition: Can only transition from {PromotionStatus.Started} to {PromotionStatus.Paused}.");

        if (promotion.Status == PromotionStatus.Paused && request.Status != PromotionStatus.Cancelled)
            throw new Exception($"Invalid transition: Can only transition from {PromotionStatus.Paused} to {PromotionStatus.Cancelled}.");

        promotion.UpdateStatus(request.Status);

        _promotionRepository.Update(promotion);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}