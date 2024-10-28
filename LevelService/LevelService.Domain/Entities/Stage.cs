using LevelService.Domain.Enum;
using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Stage : BaseEntity<int>
{
    public Stage(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, bool isCustom)
    {
        DateFrom = dateFrom;
        DateTo = dateFrom;
        IsCustom = isCustom;
    }

    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public bool IsCustom { get; set; }
    public StageStatus Status { get; set; }

    public ICollection<Level> Levels { get; set; }

    public void UpdateStatus(StageStatus status)
    {
        Status = status;
    }

    public void Update(DateTimeOffset datefrom, DateTimeOffset dateTo, bool isCustom)
    {
        DateFrom = datefrom;
        DateTo = dateTo;
        IsCustom = isCustom;
    }


    public void UpdateLevel(int id, int number, int experienceToArchieve)
    {
        var level = Levels.FirstOrDefault(x => x.Id == id);

        if (level == null) return;

        level.Update(number, experienceToArchieve);
    }
}
