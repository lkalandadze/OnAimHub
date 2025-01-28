using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using ValidationException = FluentValidation.ValidationException;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, ApplicationResult<bool>>
{
    private readonly IEndpointService _endpointService;
    private readonly IValidator<DeleteEndpointCommand> _validator;

    public DeleteEndpointCommandHandler(IEndpointService endpointService, IValidator<DeleteEndpointCommand> validator) 
    {
        _endpointService = endpointService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _endpointService.Delete(request.Ids);
    }
}
