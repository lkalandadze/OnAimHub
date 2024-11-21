using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Enum;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.Commands.UpdateCoinTemplate;

public class UpdateCoinTemplateHandler : IRequestHandler<UpdateCoinTemplate>
{
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCoinTemplateHandler(ICoinTemplateRepository coinTemplateRepository, IWithdrawOptionRepository withdrawOptionRepository, IUnitOfWork unitOfWork)
    {
        _coinTemplateRepository = coinTemplateRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCoinTemplate request, CancellationToken cancellationToken)
    {
        var coinTemplate = await _coinTemplateRepository.OfIdAsync(request.Id);

        if (coinTemplate == null || coinTemplate.IsDeleted)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Coin template with the specified ID: [{request.Id}] was not found.");
        }

        coinTemplate.Update(request.Name, request.Description, request.ImageUrl, request.CoinType);

        if (request.WithdrawOptionIds != null && request.WithdrawOptionIds.Any() && request.CoinType == CoinType.Outgoing)
        {
            var withdrawOptions = (await _withdrawOptionRepository.QueryAsync(wo => request.WithdrawOptionIds.Any(woId => woId == wo.Id)));
            
            coinTemplate.SetWithdrawOptions(withdrawOptions);
        }

        _coinTemplateRepository.Update(coinTemplate);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}