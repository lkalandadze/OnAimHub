using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

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
        var withdrawOptionGroup = await _withdrawOptionGroupRepository.OfIdAsync(request.Id);

        if (withdrawOptionGroup == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"WithdrawOption group with the specified ID: [{request.Id}] was not found.");
        }

        withdrawOptionGroup.Delete();
        _withdrawOptionGroupRepository.Update(withdrawOptionGroup);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}