using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public class UpdateEndpointGroupCommandHandler : ICommandHandler<UpdateEndpointGroupCommand, ApplicationResult<string>>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<UpdateEndpointGroupCommand> _validator;

    public UpdateEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<UpdateEndpointGroupCommand> validator)
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _endpointGroupService.Update(request.Id, request.Model);
    }
}
