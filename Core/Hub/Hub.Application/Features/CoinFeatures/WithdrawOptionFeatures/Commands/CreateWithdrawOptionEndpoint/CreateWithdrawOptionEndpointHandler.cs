using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionEndpoint;

public class CreateWithdrawOptionEndpointHandler : IRequestHandler<CreateWithdrawOptionEndpoint>
{
    private readonly IWithdrawOptionEndpointRepository _withdrawOptionEndpointRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWithdrawOptionEndpointHandler(IWithdrawOptionEndpointRepository withdrawOptionEndpointRepository, IUnitOfWork unitOfWork)
    {
        _withdrawOptionEndpointRepository = withdrawOptionEndpointRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateWithdrawOptionEndpoint request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var endpoint = new WithdrawOptionEndpoint(
            request.Name,
            request.Endpoint,
            request.Content,
            request.ContentType,
            createdByUserId: request.CreatedByUserId);

        await _withdrawOptionEndpointRepository.InsertAsync(endpoint);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}