using LevelService.Application.Services.Abstract.BackgroundJobs;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using MediatR;

namespace LevelService.Application.Features.StageFeatures.Commands.Create;

public class CreateStageCommandHandler : IRequestHandler<CreateStageCommand>
{
    private readonly IStageRepository _stageRepository;
    private readonly IStageSchedulerService _stageSchedulerService;
    public CreateStageCommandHandler(IStageRepository stageRepository, IStageSchedulerService stageSchedulerService)
    {
        _stageRepository = stageRepository;
        _stageSchedulerService = stageSchedulerService;
    }

    public async Task Handle(CreateStageCommand request, CancellationToken cancellationToken)
    {
        var stage = new Stage(request.Name, request.Description, request.DateFrom, request.DateTo, request.isExpirable);


        if (request.isExpirable)
            _stageSchedulerService.ScheduleActJobs(stage);

        await _stageRepository.InsertAsync(stage);
        await _stageRepository.SaveChangesAsync();
    }
}