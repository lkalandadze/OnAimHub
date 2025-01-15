using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;

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
        foreach (var id in request.Ids)
        {
            var withdrawOptionEndpoint = await _withdrawOptionEndpointRepository.OfIdAsync(id);

            if (withdrawOptionEndpoint == null)
            {
                continue;
            }

            withdrawOptionEndpoint.Delete();
            _withdrawOptionEndpointRepository.Update(withdrawOptionEndpoint);
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}