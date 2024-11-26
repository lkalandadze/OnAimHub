using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.PromotionFeatures.Commands.SoftDelete;

public class SoftDeletePromotionCommandHandler : IRequestHandler<SoftDeletePromotionCommand>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public SoftDeletePromotionCommandHandler(IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(SoftDeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.Query().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (promotion == default)
            throw new Exception("Promotion not found with this Id");

        if (promotion.Status == PromotionStatus.Started)
            throw new Exception("Promotion cannot be deleted when active");

        promotion.Delete();

        _promotionRepository.Update(promotion);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
