using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandHandler : ICommandHandler<DeleteEndpointGroupCommand, ApplicationResult>
{
    private readonly IEndpointGroupService _endpointGroupService;
    private readonly IValidator<DeleteEndpointGroupCommand> _validator;

    public DeleteEndpointGroupCommandHandler(IEndpointGroupService endpointGroupService, IValidator<DeleteEndpointGroupCommand> validator)
    {
        _endpointGroupService = endpointGroupService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _endpointGroupService.Delete(request.GroupIds);

        return new ApplicationResult { Success = result.Success, Data = result };
    }
}
