using LevelService.Domain.Entities;
using LevelService.Domain.Enum;

namespace LevelService.Application.Models.Stages;

public class StageModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public bool IsExpirable { get; set; }
    public StageStatus Status { get; set; }

    public static StageModel MapFrom(Stage stage)
    {
        return new StageModel
        {
            Id = stage.Id,
            Name = stage.Name,
            Description = stage.Description,
            DateFrom = stage.DateFrom,
            DateTo = stage.DateTo,
            IsExpirable = stage.IsExpirable,
            Status = stage.Status,
        };
    }
}