using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.UpdateWithdrawEndpointTemplate;

public class UpdateWithdrawEndpointTemplateHandler : IRequestHandler<UpdateWithdrawEndpointTemplate>
{
    private readonly IWithdrawEndpointTemplateRepository _endpointTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWithdrawEndpointTemplateHandler(IWithdrawEndpointTemplateRepository endpointTemplateRepository, IUnitOfWork unitOfWork)
    {
        _endpointTemplateRepository = endpointTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateWithdrawEndpointTemplate request, CancellationToken cancellationToken)
    {
        var template = await _endpointTemplateRepository.OfIdAsync(request.Id);

        if (template == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Endpoint template with the specified ID: [{request.Id}] was not found.");
        }

        template.Update(request.Name, request.Endpoint, request.Content, request.ContentType);

        _endpointTemplateRepository.Update(template);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}