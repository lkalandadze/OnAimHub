using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOption;

public class DeleteWithdrawOptionHandler : IRequestHandler<DeleteWithdrawOption>
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWithdrawOptionHandler(IWithdrawOptionRepository withdrawOptionRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteWithdrawOption request, CancellationToken cancellationToken)
    {
        var withdrawOption = await _withdrawOptionRepository.OfIdAsync(request.Id);

        if (withdrawOption == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"WithdrawOption with the specified ID: [{request.Id}] was not found.");
        }

        withdrawOption.Delete();
        _withdrawOptionRepository.Update(withdrawOption);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}