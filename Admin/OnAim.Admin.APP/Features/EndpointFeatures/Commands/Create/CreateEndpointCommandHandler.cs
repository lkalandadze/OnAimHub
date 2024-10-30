using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommandHandler : ICommandHandler<CreateEndpointCommand, ApplicationResult>
{
    private readonly IEndpointService _permissionService;
    private readonly IValidator<CreateEndpointCommand> _validator;

    public CreateEndpointCommandHandler(IEndpointService permissionService, IValidator<CreateEndpointCommand> validator)
    {
        _permissionService = permissionService; 
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _permissionService.Create(request.Endpoints);

        return new ApplicationResult { Success = result.Success, Data = result };
    }
}
