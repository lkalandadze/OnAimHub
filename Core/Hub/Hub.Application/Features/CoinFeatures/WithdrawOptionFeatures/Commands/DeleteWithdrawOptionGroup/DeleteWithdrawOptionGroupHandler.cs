using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOptionGroup;

public class DeleteWithdrawOptionGroupHandler : IRequestHandler<DeleteWithdrawOptionGroup>
{
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWithdrawOptionGroupHandler(IWithdrawOptionGroupRepository withdrawOptionGroupRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteWithdrawOptionGroup request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var withdrawOptionGroup = await _withdrawOptionGroupRepository.OfIdAsync(id);

            if (withdrawOptionGroup == null)
            {
                continue;
            }

            withdrawOptionGroup.Delete();
            _withdrawOptionGroupRepository.Update(withdrawOptionGroup);
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}