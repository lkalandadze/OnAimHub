using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public class GetLeaderBoardResultByPlayerIdQueryHandler : IQueryHandler<GetLeaderBoardResultByPlayerIdQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<LeaderboardResult> _readOnlyRepository;

    public GetLeaderBoardResultByPlayerIdQueryHandler(IReadOnlyRepository<LeaderboardResult> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetLeaderBoardResultByPlayerIdQuery request, CancellationToken cancellationToken)
    {
        var leaderboardResults = _readOnlyRepository.Query().Where(x => x.PlayerId == request.PlayerId);

        var total = leaderboardResults.Count();       

        return new ApplicationResult
        {
            Success = true,
            Data = await leaderboardResults.ToListAsync(),
        };
    }
}
