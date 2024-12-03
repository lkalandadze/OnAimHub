using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionEndpoint;

public class UpdateWithdrawOptionEndpointHandler : IRequestHandler<UpdateWithdrawOptionEndpoint>
{
    private readonly IWithdrawOptionEndpointRepository _withdrawOptionEndpointRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWithdrawOptionEndpointHandler(IWithdrawOptionEndpointRepository withdrawOptionEndpointRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionEndpointRepository = withdrawOptionEndpointRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateWithdrawOptionEndpoint request, CancellationToken cancellationToken)
    {
        var endpoint = await _withdrawOptionEndpointRepository.OfIdAsync(request.Id);

        if (endpoint == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw option endpoint with the specified ID: [{request.Id}] was not found.");
        }

        endpoint.Update(request.Name, request.Endpoint, request.Content, request.ContentType);

        _withdrawOptionEndpointRepository.Update(endpoint);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}