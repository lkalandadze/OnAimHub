using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetById;

public class GetLeaderboardRecordByIdQueryHandler : IRequestHandler<GetLeaderboardRecordByIdQuery, GetLeaderboardRecordByIdQueryResponse>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    public GetLeaderboardRecordByIdQueryHandler(ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task<GetLeaderboardRecordByIdQueryResponse> Handle(GetLeaderboardRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var leaderboardRecord = await _leaderboardRecordRepository.Query().Include(x => x.LeaderboardPrizes).FirstOrDefaultAsync(x => x.Id == request.Id);

        if (leaderboardRecord == default)
            throw new Exception("Leaderboard record not found");

        var leaderboardRecordModel = LeaderboardRecordByIdModel.MapFrom(leaderboardRecord);

        var response = new GetLeaderboardRecordByIdQueryResponse
        {
            Data = leaderboardRecordModel
        };

        return response;
    }
}