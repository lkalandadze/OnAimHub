using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOptionGroup;

public class UpdateWithdrawOptionGroupHandler : IRequestHandler<UpdateWithdrawOptionGroup>
{
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWithdrawOptionGroupHandler(
        IWithdrawOptionGroupRepository withdrawOptionGroupRepository,
        IWithdrawOptionRepository withdrawOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateWithdrawOptionGroup request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var group = await _withdrawOptionGroupRepository.OfIdAsync(request.Id);

        if (group == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw group group with the specified ID: [{request.Id}] was not found.");
        }

        var withdrawOptions = await _withdrawOptionRepository.QueryAsync(wog => request.WithdrawOptionIds.Any(wogId => wogId == wog.Id));

        group.Update(
            request.Title,
            request.Description,
            request.ImageUrl,
            request.PriorityIndex,
            withdrawOptions);

        await _withdrawOptionGroupRepository.InsertAsync(group);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}