using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Domain.GetAll
{
    public class GetAllDomainQueryHandler : IQueryHandler<GetAllDomainQuery, ApplicationResult>
    {
        private readonly IRepository<AllowedEmailDomain> _repository;

        public GetAllDomainQueryHandler(IRepository<AllowedEmailDomain> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllDomainQuery request, CancellationToken cancellationToken)
        {
            var domains = _repository.Query(x => !x.IsDeleted);

            return new ApplicationResult { Success = true, Data = domains };
        }
    }
}
