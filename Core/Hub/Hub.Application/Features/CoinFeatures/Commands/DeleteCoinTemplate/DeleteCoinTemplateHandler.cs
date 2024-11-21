using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.CoinFeatures.Commands.DeleteCoinTemplate;

public class DeleteCoinTemplateHandler : IRequestHandler<DeleteCoinTemplate>
{
    private readonly ICoinTemplateRepository _coinTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCoinTemplateHandler(ICoinTemplateRepository coinTemplateRepository, IUnitOfWork unitOfWork)
    {
        _coinTemplateRepository = coinTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCoinTemplate request, CancellationToken cancellationToken)
    {
        var coinTemplate = await _coinTemplateRepository.OfIdAsync(request.CoinTemplateId);

        if (coinTemplate == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Coin template with the specified ID: [{request.CoinTemplateId}] was not found.");
        }

        coinTemplate.Delete();

        _coinTemplateRepository.Update(coinTemplate);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}