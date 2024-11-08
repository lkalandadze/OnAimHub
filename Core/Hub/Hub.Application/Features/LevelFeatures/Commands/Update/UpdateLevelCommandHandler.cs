using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.LevelFeatures.Commands.Update;

public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand>
{
    private readonly IActRepository _actRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateLevelCommandHandler(IActRepository actRepository, IUnitOfWork unitOfWork)
    {
        _actRepository = actRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateLevelCommand request, CancellationToken cancellationToken)
    {
        var act = _actRepository.Query().FirstOrDefault(x => x.Id == request.Act.Id);

        if (act == default)
            throw new Exception("Act not found");


        act.Update(request.Act.DateFrom, request.Act.DateTo, request.Act.IsCustom);
        foreach (var levelModel in request.Act.Level)
        {
            act.UpdateLevel(levelModel.Id, levelModel.Number, levelModel.ExperienceToArchive);

            var level = act.Levels.FirstOrDefault(l => l.Id == levelModel.Id);
            if (level != null)
            {
                foreach (var prizeModel in levelModel.Prize)
                {
                    level.UpdateLevelPrizes(prizeModel.Id, prizeModel.Amount, prizeModel.PrizeTypeId, prizeModel.PrizeDeliveryType);
                }
            }
        }

        return Unit.Value;
    }
}