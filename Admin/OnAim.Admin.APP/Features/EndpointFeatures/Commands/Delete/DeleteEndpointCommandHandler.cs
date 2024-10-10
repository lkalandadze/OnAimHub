using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, ApplicationResult>
{
    private readonly IEndpointService _endpointService;
    private readonly IValidator<DeleteEndpointCommand> _validator;

    public DeleteEndpointCommandHandler(IEndpointService endpointService, IValidator<DeleteEndpointCommand> validator) 
    {
        _endpointService = endpointService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _endpointService.Delete(request.Ids);

        return new ApplicationResult { Success = result.Success, Data = result };
    }
}
