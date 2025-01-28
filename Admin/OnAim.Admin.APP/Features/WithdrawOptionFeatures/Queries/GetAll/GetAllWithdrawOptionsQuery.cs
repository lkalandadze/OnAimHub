using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetAll;

public record GetAllWithdrawOptionsQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<WithdrawOptionDto>>>;