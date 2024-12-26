using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.CreateWithdrawOptionGroup;

public class CreateWithdrawOptionGroupHandler : IRequestHandler<CreateWithdrawOptionGroup>
{
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWithdrawOptionGroupHandler(
        IWithdrawOptionGroupRepository withdrawOptionGroupRepository,
        IWithdrawOptionRepository withdrawOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateWithdrawOptionGroup request, CancellationToken cancellationToken)
    {
        var group = new WithdrawOptionGroup(
            request.Title,
            request.Description,
            request.ImageUrl,
            request.PriorityIndex,
            createdByUserId: request.CreatedByUserId);

        if (request.WithdrawOptionIds != null && request.WithdrawOptionIds.Any())
        {
            var withdrawOptions = await _withdrawOptionRepository.QueryAsync(wo => request.WithdrawOptionIds.Any(woId => woId == wo.Id));

            group.AddWithdrawOptions(withdrawOptions);
        }

        await _withdrawOptionGroupRepository.InsertAsync(group);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}