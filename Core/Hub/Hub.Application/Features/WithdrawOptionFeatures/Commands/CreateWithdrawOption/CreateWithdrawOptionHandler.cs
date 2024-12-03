using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

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
            var withdrawOptionGroups = (await _withdrawOptionGroupRepository.QueryAsync(wog => request.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id)));

            option.AddWithdrawOptionGroups(withdrawOptionGroups);
        }

        await _withdrawOptionRepository.InsertAsync(option);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}