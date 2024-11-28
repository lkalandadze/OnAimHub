using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawEndpointTemplate;

//TODO: should be changed name
public class CreateWithdrawEndpointTemplateHandler : IRequestHandler<CreateWithdrawEndpointTemplate>
{
    private readonly IWithdrawOptionEndpointRepository _endpointTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWithdrawEndpointTemplateHandler(IWithdrawOptionEndpointRepository endpointTemplateRepository, IUnitOfWork unitOfWork)
    {
        _endpointTemplateRepository = endpointTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateWithdrawEndpointTemplate request, CancellationToken cancellationToken)
    {
        var template = new WithdrawOptionEndpoint(request.Name, request.Endpoint, request.Content, request.ContentType);

        await _endpointTemplateRepository.InsertAsync(template);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}