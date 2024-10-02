using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public class CreateEmailDomainCommandHandler : BaseCommandHandler<CreateEmailDomainCommand, ApplicationResult>
{
    private readonly IRepository<AllowedEmailDomain> _repository;

    public CreateEmailDomainCommandHandler(
        CommandContext<CreateEmailDomainCommand> context,
        IRepository<AllowedEmailDomain> repository
        ) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(CreateEmailDomainCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        if (request.Id != 0)
        {
            var domainn = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

            domainn?.Update(request.Domain, request.IsActive ?? true);
            await _repository.CommitChanges();

            return new ApplicationResult { Success = true };
        }

        var existed = await _repository.Query(x => x.Domain == request.Domain).FirstOrDefaultAsync();

        if (existed != null)
        {
            if (existed?.IsDeleted == true)
            {
                existed.Update(request.Domain, true);
                await _repository.CommitChanges();

                return new ApplicationResult { Success = true };
            }

            throw new BadRequestException("Domain Already Exists");
        }

        var domain = new AllowedEmailDomain(request.Domain, _context.SecurityContextAccessor.UserId);

        await _repository.Store(domain);
        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
