using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOptionEndpoint;

public class DeleteWithdrawOptionEndpointHandler : IRequestHandler<DeleteWithdrawOptionEndpoint>
{
    private readonly IWithdrawOptionEndpointRepository _withdrawOptionEndpointRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWithdrawOptionEndpointHandler(IWithdrawOptionEndpointRepository withdrawOptionEndpointRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionEndpointRepository = withdrawOptionEndpointRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteWithdrawOptionEndpoint request, CancellationToken cancellationToken)
    {
        var withdrawOptionEndpoint = await _withdrawOptionEndpointRepository.OfIdAsync(request.Id);

        if (withdrawOptionEndpoint == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"WithdrawOption endpoint with the specified ID: [{request.Id}] was not found.");
        }

        withdrawOptionEndpoint.Delete();
        _withdrawOptionEndpointRepository.Update(withdrawOptionEndpoint);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}