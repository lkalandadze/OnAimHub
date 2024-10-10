using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public class UpdateEndpointGroupCommandHandler : ICommandHandler<UpdateEndpointGroupCommand, ApplicationResult>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<UpdateEndpointGroupCommand> _validator;

    public UpdateEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<UpdateEndpointGroupCommand> validator)
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _endpointGroupService.Update(request.Id, request.Model);

        return new ApplicationResult { Success = result.Success, Data = result };
    }
}
