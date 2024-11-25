using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Features.PromotionFeatures.Commands.Delete;

public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeletePromotionCommandHandler(IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.Query().FirstOrDefaultAsync(x => x.Correlationid == request.CorrelationId, cancellationToken);

        if (promotion == default)
            throw new Exception("Promotion not found with this correlationId");

        _promotionRepository.Delete(promotion);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
