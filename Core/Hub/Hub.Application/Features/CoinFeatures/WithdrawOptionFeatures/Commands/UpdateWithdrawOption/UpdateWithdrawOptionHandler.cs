﻿using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.UpdateWithdrawOption;

public class UpdateWithdrawOptionHandler : IRequestHandler<UpdateWithdrawOption>
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWithdrawOptionHandler(
        IWithdrawOptionRepository withdrawOptionRepository,
        IWithdrawOptionGroupRepository withdrawOptionGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateWithdrawOption request, CancellationToken cancellationToken)
    {
        if (!CheckmateValidations.Checkmate.IsValid(request, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        }

        var option = await _withdrawOptionRepository.OfIdAsync(request.Id);

        if (option == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw option with the specified ID: [{request.Id}] was not found.");
        }

        var withdrawOptionGroups = await _withdrawOptionGroupRepository.QueryAsync(wog => request.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id));

        option.Update(
            request.Title,
            request.Description,
            request.ImageUrl,
            request.Value,
            request.Endpoint,
            request.EndpointContentType,
            request.EndpointContent,
            request.WithdrawOptionEndpointId,
            withdrawOptionGroups);

        _withdrawOptionRepository.Update(option);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}