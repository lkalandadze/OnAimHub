using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;
using Hub.Domain.Absractions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignCoinTemplatesToWithdrawOption;

public class AssignCoinTemplatesToWithdrawOptionHandler : IRequestHandler<AssignCoinTemplatesToWithdrawOption>
{
    private readonly ICoinTemplateWithdrawOptionRepository _coinTemplateWithdrawOptionRepository;
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignCoinTemplatesToWithdrawOptionHandler(ICoinTemplateWithdrawOptionRepository coinTemplateWithdrawOptionRepository, IWithdrawOptionRepository withdrawOptionRepository, ICoinTemplateRepository coinTemplateRepository, IUnitOfWork unitOfWork)
    {
        _coinTemplateWithdrawOptionRepository = coinTemplateWithdrawOptionRepository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _coinTemplateRepository = coinTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignCoinTemplatesToWithdrawOption request, CancellationToken cancellationToken)
    {
        var option = await _withdrawOptionRepository.OfIdAsync(request.WithdrawOptionId);

        if (option == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Withdraw option with the specified ID: [{request.WithdrawOptionId}] was not found.");
        }

        var coinTemplates = (await _coinTemplateRepository.QueryAsync(pc => request.CoinTemplateIds.Any(ctId => ctId == pc.Id)))
                                                          .Where(c => c.CoinType == CoinType.Outgoing);

        if (coinTemplates == null || !coinTemplates.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No coin templates were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing coin templates."
            );
        }

        foreach (var coin in coinTemplates)
        {
            var coinWithdrawOption = new CoinTemplateWithdrawOption(coin.Id, request.WithdrawOptionId);

            await _coinTemplateWithdrawOptionRepository.InsertAsync(coinWithdrawOption);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
