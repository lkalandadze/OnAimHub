using Hub.Application.Features.LevelFeatures.DataModels;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.LevelFeatures.Queries.Get;

public class GetLevelsQueryHandler : IRequestHandler<GetLevelsQuery, GetLevelsQueryResponse>
{
    private readonly IActRepository _actRepository;
    public GetLevelsQueryHandler(IActRepository actRepository)
    {
        _actRepository = actRepository;
    }

    public async Task<GetLevelsQueryResponse> Handle(GetLevelsQuery request, CancellationToken cancellationToken)
    {
        var acts = _actRepository.Query().Include(x => x.Levels).ThenInclude(x => x.LevelPrizes);

        var total = acts.Count();

        var actList = acts.Pagination(request).ToList();

        var response = new GetLevelsQueryResponse
        {
            Data = new PagedResponse<GetLevelsModel>
            (
                actList?.Select(x => GetLevelsModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}