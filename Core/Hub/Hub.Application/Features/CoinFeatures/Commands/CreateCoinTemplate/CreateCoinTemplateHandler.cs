using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Templates;
using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.CoinFeatures.Commands.CreateCoinTemplate;

public class CreateCoinTemplateHandler : IRequestHandler<CreateCoinTemplate>
{
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCoinTemplateHandler(ICoinTemplateRepository coinTemplateRepository, IWithdrawOptionRepository withdrawOptionRepository, IUnitOfWork unitOfWork)
    {
        _coinTemplateRepository = coinTemplateRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateCoinTemplate request, CancellationToken cancellationToken)
    {
        var coinTemplate = new CoinTemplate(request.Name, request.Description, request.IconUrl, request.CoinType);

        if (request.WithdrawOptionIds != null && request.WithdrawOptionIds.Any() && request.CoinType == CoinType.Outgoing)
        {
            var withdrawOptions = (await _withdrawOptionRepository.QueryAsync(wo => request.WithdrawOptionIds.Any(woId => woId == wo.Id)));

            coinTemplate.AddWithdrawOptions(withdrawOptions);
        }

        await _coinTemplateRepository.InsertAsync(coinTemplate);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}