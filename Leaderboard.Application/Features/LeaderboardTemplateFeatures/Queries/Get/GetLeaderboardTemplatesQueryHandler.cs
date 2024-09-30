using Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.Get;

public class GetLeaderboardTemplatesQueryHandler : IRequestHandler<GetLeaderboardTemplatesQuery, GetLeaderboardTemplatesQueryResponse>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public GetLeaderboardTemplatesQueryHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task<GetLeaderboardTemplatesQueryResponse> Handle(GetLeaderboardTemplatesQuery request, CancellationToken cancellationToken)
    {
        var leaderboardTemplates = _leaderboardTemplateRepository.Query();

        var total = leaderboardTemplates.Count();

        var leaderboardTemplateList = leaderboardTemplates.Pagination(request).ToList();

        var response = new GetLeaderboardTemplatesQueryResponse
        {
            Data = new PagedResponse<LeaderboardTemplatesModel>
            (
                leaderboardTemplateList?.Select(x => LeaderboardTemplatesModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}