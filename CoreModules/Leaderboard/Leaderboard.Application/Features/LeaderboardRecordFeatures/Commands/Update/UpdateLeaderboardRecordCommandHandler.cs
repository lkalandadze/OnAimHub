﻿using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Update;

public class UpdateLeaderboardRecordCommandHandler : IRequestHandler<UpdateLeaderboardRecordCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    public UpdateLeaderboardRecordCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task Handle(UpdateLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        //var leaderboard = await _leaderboardRecordRepository.Query().Include(x => x.LeaderboardRecordPrizes).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        //if (leaderboard == default)
        //    throw new Exception("Leaderboard not found");

        //leaderboard.Update(request.Name,
        //                   request.Description,
        //                   request.CreationDate.ToUniversalTime(), 
        //                   request.AnnouncementDate.ToUniversalTime(), 
        //                   request.StartDate.ToUniversalTime(),
        //                   request.EndDate.ToUniversalTime(),
        //                   request.LeaderboardType
        //                   //request.JobType
        //                   );

        //foreach(var prize in request.Prizes)
        //{
        //    leaderboard.UpdateLeaderboardPrizes(prize.Id, prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        //}

        //_leaderboardRecordRepository.Update(leaderboard);
        //await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);
    }
}