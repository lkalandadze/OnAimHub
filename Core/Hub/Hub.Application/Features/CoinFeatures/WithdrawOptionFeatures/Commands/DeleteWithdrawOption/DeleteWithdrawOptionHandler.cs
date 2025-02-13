﻿using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;

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
        foreach (var id in request.Ids)
        {
            var withdrawOption = await _withdrawOptionRepository.OfIdAsync(id);

            if (withdrawOption == null)
            {
                continue;
            }

            withdrawOption.Delete();
            _withdrawOptionRepository.Update(withdrawOption);
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}