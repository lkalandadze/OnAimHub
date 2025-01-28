using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public sealed class UpdateEndpointCommandHandler : ICommandHandler<UpdateEndpointCommand, ApplicationResult<string>>
{
    private readonly IEndpointService _endpointService;
    private readonly IValidator<UpdateEndpointCommand> _validator;

    public UpdateEndpointCommandHandler(IEndpointService endpointService, IValidator<UpdateEndpointCommand> validator)
    {
        _endpointService = endpointService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _endpointService.Update(request.Id, request.Endpoint);
    }
}
