using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetAll;

public record GetAllWithdrawOptionsQuery(BaseFilter Filter) : IQuery<ApplicationResult>;