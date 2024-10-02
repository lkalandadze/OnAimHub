using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public record GetAllDomainQuery(DomainFilter Filter) : IQuery<ApplicationResult>;
