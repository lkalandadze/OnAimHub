using LevelService.Application.Features.StageFeatures.Queries.Get;
using LevelService.Application.Models.Configurations;
using LevelService.Application.Models.Stages;
using LevelService.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.ConfigurationFeatures.Queries.Get;

public class GetConfigurationsQueryHandler : IRequestHandler<GetConfigurationsQuery, GetConfigurationsQueryResponse>
{
    private readonly IStageRepository _stageRepository;
    public GetConfigurationsQueryHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<GetConfigurationsQueryResponse> Handle(GetConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query()
                    .Include(x => x.Configurations)
                    .FirstOrDefaultAsync(x => x.Id == request.StageId && !x.IsDeleted, cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        var configurations = stage.Configurations
            .Select(ConfigurationModel.MapFrom)
            .ToList();

        return new GetConfigurationsQueryResponse
        {
            Configurations = configurations
        };
    }
}