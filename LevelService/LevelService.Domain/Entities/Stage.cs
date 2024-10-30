using LevelService.Domain.Enum;
using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class Stage : BaseEntity<int>
{
    public Stage(string name, string description, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, bool isExpirable)
    {
        Name = name;
        Description = description;
        DateFrom = dateFrom;
        DateTo = dateFrom;
        IsExpirable = isExpirable;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset? DateFrom { get; private set; }
    public DateTimeOffset? DateTo { get; private set; }
    public bool IsExpirable { get; private set; }
    public StageStatus Status { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }

    public ICollection<Level> Levels { get; set; }

    public void UpdateStatus(StageStatus status)
    {
        Status = status;
    }

    public void Update(string name, string description, DateTimeOffset datefrom, DateTimeOffset dateTo, bool isExpirable)
    {
        Name = name;
        Description = description;
        DateFrom = datefrom;
        DateTo = dateTo;
        IsExpirable = isExpirable;
    }

    public void UpdateLevel(int id, int number, int experienceToArchieve)
    {
        var level = Levels.FirstOrDefault(x => x.Id == id);

        if (level == null) return;

        level.Update(number, experienceToArchieve);
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
