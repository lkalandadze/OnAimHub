using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Act : BaseEntity<int>
{
    public Act(DateTimeOffset dateFrom, DateTimeOffset dateTo)
    {
        DateFrom = dateFrom.ToUniversalTime();
        DateTo = dateTo.ToUniversalTime();
    }

    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set;}
    public ActStatus Status { get; set; }

    public ICollection<Level> Levels { get; set; }

    public void UpdateStatus(ActStatus status)
    {
        Status = status;
    }

    public void Update(DateTimeOffset datefrom, DateTimeOffset dateTo)
    {
        DateFrom = datefrom;
        DateTo = dateTo;
    }


    public void UpdateLevel(int id, int number, int experienceToArchieve)
    {
        var level = Levels.FirstOrDefault(x => x.Id == id);

        if (level == null) return;

        level.Update(number, experienceToArchieve);
    }
}
