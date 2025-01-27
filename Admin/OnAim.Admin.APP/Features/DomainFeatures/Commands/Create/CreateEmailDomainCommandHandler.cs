using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.Domain;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public class CreateEmailDomainCommandHandler : ICommandHandler<CreateEmailDomainCommand, ApplicationResult<bool>>
{
    private readonly IDomainService _domainService;
    private readonly IValidator<CreateEmailDomainCommand> _validator;

    public CreateEmailDomainCommandHandler(IDomainService domainService, IValidator<CreateEmailDomainCommand> validator) 
    {
        _domainService = domainService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(CreateEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _domainService.CreateOrUpdateDomain(request.Domains, request.Domain, request.IsActive);

        return new ApplicationResult<bool> { Success = result.Success };
    }
}
