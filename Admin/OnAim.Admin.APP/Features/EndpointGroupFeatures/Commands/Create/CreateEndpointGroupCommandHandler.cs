using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;

public class CreateEndpointGroupCommandHandler : ICommandHandler<CreateEndpointGroupCommand, ApplicationResult<string>>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<CreateEndpointGroupCommand> _validator;

    public CreateEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<CreateEndpointGroupCommand> validator) 
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _endpointGroupService.Create(request.Model);
    }
}
