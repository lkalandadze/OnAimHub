using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.AdminServices.Domain;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public class DeleteEmailDomainCommandHandler : ICommandHandler<DeleteEmailDomainCommand, ApplicationResult<bool>>
{
    private readonly IDomainService _domainService;
    private readonly IValidator<DeleteEmailDomainCommand> _validator;

    public DeleteEmailDomainCommandHandler(IDomainService domainService, IValidator<DeleteEmailDomainCommand> validator)
    {
        _domainService = domainService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _domainService.DeleteEmailDomain(request.Ids);

        return new ApplicationResult<bool> { Success = result.Success };
    }
}
