using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance
{
    public class GetPlayerBalanceQueryHandler : IQueryHandler<GetPlayerBalanceQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<PlayerBalance> _readOnlyRepository;

        public GetPlayerBalanceQueryHandler(IReadOnlyRepository<PlayerBalance> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ApplicationResult> Handle(GetPlayerBalanceQuery request, CancellationToken cancellationToken)
        {
            var balances = _readOnlyRepository.Query(x => x.PlayerId == request.Id);

            var result = balances.Select(x => new PlayerBalanceDto
            {
                Id = x.Id,
                Amount = x.Amount,
                Currency = x.Currency.Name,
            });

            return new ApplicationResult
            {
                Success = true,
                Data = await result.ToListAsync(cancellationToken)
            };
        }
    }
}
