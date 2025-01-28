using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandHandler : ICommandHandler<DeleteEndpointGroupCommand, ApplicationResult<bool>>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<DeleteEndpointGroupCommand> _validator;

    public DeleteEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<DeleteEndpointGroupCommand> validator)
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _endpointGroupService.Delete(request.GroupIds);
    }
}
