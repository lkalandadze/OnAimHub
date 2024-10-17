using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.LevelFeatures.Commands.Update;

public sealed class UpdateLevelCommand : IRequest
{
    public UpdateActModel Act { get; set; }
}
public sealed class UpdateActModel
{
    public int Id { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public ActStatus Status { get; set; }
    public List<CreateLevelModel> Level { get; set; }
}

public class CreateLevelModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int ExperienceToArchive { get; set; }
    public List<CreateLevelPrizeModel> Prize { get; set; }
}

public class CreateLevelPrizeModel
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public string CurrencyId { get; set; }
    public PrizeType PrizeType { get; set; }
}