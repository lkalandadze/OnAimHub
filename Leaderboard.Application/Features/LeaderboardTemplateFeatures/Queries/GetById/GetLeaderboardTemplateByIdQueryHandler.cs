using Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.GetById;

public class GetLeaderboardTemplateByIdQueryHandler : IRequestHandler<GetLeaderboardTemplateByIdQuery, GetLeaderboardTemplateByIdQueryResponse>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public GetLeaderboardTemplateByIdQueryHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task<GetLeaderboardTemplateByIdQueryResponse> Handle(GetLeaderboardTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var leaderboardTemplate = await _leaderboardTemplateRepository.Query().Include(x => x.LeaderboardTemplatePrizes).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (leaderboardTemplate == default)
            throw new Exception("Leaderboard template not found");

        var leaderboardTemplateModel = LeaderboardTemplateByIdModel.MapFrom(leaderboardTemplate);

        var response = new GetLeaderboardTemplateByIdQueryResponse
        {
            Data = leaderboardTemplateModel
        };

        return response;
    }
}