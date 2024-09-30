using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public class DeleteEmailDomainCommandHandler : BaseCommandHandler<DeleteEmailDomainCommand, ApplicationResult>
{
    private readonly IRepository<AllowedEmailDomain> _repository;

    public DeleteEmailDomainCommandHandler(
        CommandContext<DeleteEmailDomainCommand> context,
        IRepository<AllowedEmailDomain> repository
        ) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(DeleteEmailDomainCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var domain = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (domain == null)
            throw new NotFoundException("Domain Not Found!");

        domain.IsActive = false;
        domain.MarkAsDeleted();

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
