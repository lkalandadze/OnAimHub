using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetById;

public record GetWithdrawOptionByIdQuery(int Id) : IQuery<ApplicationResult<WithdrawOptionDto>>;