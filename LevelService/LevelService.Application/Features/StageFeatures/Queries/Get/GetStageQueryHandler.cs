using LevelService.Application.Models.Stages;
using LevelService.Domain.Abstractions.Repository;
using MediatR;

namespace LevelService.Application.Features.StageFeatures.Queries.Get;

public class GetStageQueryHandler : IRequestHandler<GetStageQuery, GetStageQueryResponse>
{
    private readonly IStageRepository _stageRepository;
    public GetStageQueryHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<GetStageQueryResponse> Handle(GetStageQuery request, CancellationToken cancellationToken)
    {
        var stages = _stageRepository.Query().Where(x => !x.IsDeleted);

        return new GetStageQueryResponse
        {
            Stages = stages.Select(x => StageModel.MapFrom(x))
        };
    }
}