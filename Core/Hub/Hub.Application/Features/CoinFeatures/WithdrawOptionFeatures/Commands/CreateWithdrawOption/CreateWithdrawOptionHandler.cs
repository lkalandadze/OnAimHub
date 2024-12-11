using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

public class CreateWithdrawOptionHandler : IRequestHandler<CreateWithdrawOption>
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWithdrawOptionHandler(
        IWithdrawOptionRepository withdrawOptionRepository,
        IWithdrawOptionGroupRepository withdrawOptionGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateWithdrawOption request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var option = new WithdrawOption(
            request.Title,
            request.Description,
            request.ImageUrl,
            request.Endpoint,
            request.EndpointContentType,
            request.EndpointContent,
            request.WithdrawOptionEndpointId);

        if (request.WithdrawOptionGroupIds != null && request.WithdrawOptionGroupIds.Any())
        {
            var withdrawOptionGroups = await _withdrawOptionGroupRepository.QueryAsync(wog => request.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id));

            option.AddWithdrawOptionGroups(withdrawOptionGroups);
        }

        await _withdrawOptionRepository.InsertAsync(option);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}