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
        ) : base(context)
    {
        _repository = repository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(CreateEmailDomainCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        // Update
        if (request.Domains != null && request.Domains.Any())
        {
            var domainEntities = await _repository.Query(x => request.Domains.Select(d => d.Id).Contains(x.Id)).ToListAsync();

            foreach (var domainEntity in domainEntities)
            {
                var updatedDomain = request.Domains.First(d => d.Id == domainEntity.Id);
                domainEntity.Domain = updatedDomain.Domain;
                domainEntity.IsActive = updatedDomain.IsActive;
            }

            await _repository.CommitChanges();
            return new ApplicationResult { Success = true };
        }

        //Create If Deleted
        var existingDomain = await _repository.Query(x => x.Domain == request.Domain).FirstOrDefaultAsync();
        if (existingDomain != null)
        {
            if (existingDomain.IsDeleted)
            {
                existingDomain.Domain = request.Domain;
                existingDomain.IsActive = true;
                existingDomain.IsDeleted = false;
                await _repository.CommitChanges();
                return new ApplicationResult { Success = true };
            }

            throw new BadRequestException("Domain Already Exists");
        }

        //Create New
        var newDomain = new AllowedEmailDomain(request.Domain, _context.SecurityContextAccessor.UserId);
        await _repository.Store(newDomain);
        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
