using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.LevelFeatures.Commands.Create;

public sealed class CreateLevelCommand : IRequest
{
    public CreateActModel Act { get; set; }
}
public sealed class CreateActModel
{
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public ActStatus Status { get; set; }
    public List<CreateLevelModel> Level { get; set; }
}

public class CreateLevelModel
{
    public int Number { get; set; }
    public int ExperienceToArchive { get; set; }
    public List<CreateLevelPrizeModel> Prize { get; set; }
}

public class CreateLevelPrizeModel
{
    public int Amount { get; set; }
    public string CurrencyId { get; set; }
    public PrizeDeliveryType PrizeDeliveryType { get; set; }
}