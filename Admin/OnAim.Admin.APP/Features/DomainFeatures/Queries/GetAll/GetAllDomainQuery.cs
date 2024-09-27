using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public record GetAllDomainQuery() : IQuery<ApplicationResult>;
