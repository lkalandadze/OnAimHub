using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetById;

public record GetWithdrawOptionByIdQuery(int Id) : IQuery<ApplicationResult>;