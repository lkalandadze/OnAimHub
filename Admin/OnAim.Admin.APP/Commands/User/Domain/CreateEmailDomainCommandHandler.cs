using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Domain
{
    public class CreateEmailDomainCommandHandler : ICommandHandler<CreateEmailDomainCommand, ApplicationResult>
    {
        private readonly IRepository<AllowedDomain> _repository;

        public CreateEmailDomainCommandHandler(IRepository<AllowedDomain> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(CreateEmailDomainCommand request, CancellationToken cancellationToken)
        {
            var domain = new AllowedDomain
            {
                Domain = request.Domain,
            };

            await _repository.Store(domain);
            await _repository.CommitChanges();

            return new ApplicationResult { Success = true };
        }
    }
}
