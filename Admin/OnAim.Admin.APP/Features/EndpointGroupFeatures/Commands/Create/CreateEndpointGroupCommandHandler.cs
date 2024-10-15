using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;

public class CreateEndpointGroupCommandHandler : ICommandHandler<CreateEndpointGroupCommand, ApplicationResult>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<CreateEndpointGroupCommand> _validator;

    public CreateEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<CreateEndpointGroupCommand> validator) 
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _endpointGroupService.Create(request.Model);

        return new ApplicationResult { Success = result.Success, Data = result };
    }
}
