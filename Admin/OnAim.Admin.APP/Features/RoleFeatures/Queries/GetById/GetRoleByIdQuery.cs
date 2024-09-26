using MediatR;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;

public record GetRoleByIdQuery(int Id) : IQuery<ApplicationResult>;
