using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.LevelFeatures.Commands.Create;

public class CreateLevelCommandHandler : IRequestHandler<CreateLevelCommand>
{
    private readonly IActRepository _actRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActSchedulerService _actSchedulerService;
    public CreateLevelCommandHandler(IActRepository actRepository, IUnitOfWork unitOfWork, IActSchedulerService actSchedulerService)
    {
        _actRepository = actRepository;
        _unitOfWork = unitOfWork;
        _actSchedulerService = actSchedulerService;
    }

    public async Task<Unit> Handle(CreateLevelCommand request, CancellationToken cancellationToken)
    {
        var act = new Act(request.Act.DateFrom, request.Act.DateTo)
        {
            Status = request.Act.Status,
            Levels = request.Act.Level.Select(l => new Level(l.Number, l.ExperienceToArchive)
            {
                LevelPrizes = l.Prize.Select(p => new LevelPrize(p.Amount, p.PrizeTypeId, p.PrizeDeliveryType)).ToList()
            }).ToList()
        };

        await _actRepository.InsertAsync(act);
        await _unitOfWork.SaveAsync();

        _actSchedulerService.ScheduleActJobs(act);

        return Unit.Value;
    }
}