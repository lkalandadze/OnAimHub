using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public record GetAllDomainQuery(DomainFilter Filter) : IQuery<ApplicationResult>;
